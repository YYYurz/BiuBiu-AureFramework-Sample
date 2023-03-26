//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework;
using AureFramework.ObjectPool;
using AureFramework.Resource;
using UnityEngine;

namespace DrunkFish
{
	/// <summary>
	/// 特效模块
	/// </summary>
	public sealed partial class EffectModule : AureFrameworkModule, IEffectModule
	{
		private readonly List<EffectObject> usingEffectObjectList = new List<EffectObject>();
		private readonly List<int> loadingEffectList = new List<int>();
		private IObjectPool<EffectObject> effectObjectPool;
		private InstantiateGameObjectCallbacks instantiateGameObjectCallbacks;
		
		[SerializeField] private int effectObjectPoolCapacity;
		[SerializeField] private float effectObjectPoolExpireTime;
		
		/// <summary>
		/// 获取或设置特效对象池容量
		/// </summary>
		public int EffectObjectPoolCapacity
		{
			get
			{
				return effectObjectPool.Capacity;
			}
			set
			{
				effectObjectPool.Capacity = effectObjectPoolCapacity = value;
			}
		}
		
		/// <summary>
		/// 获取或设置特效对象池过期时间
		/// </summary>
		public float EffectObjectPoolExpireTime
		{
			get
			{
				return effectObjectPool.ExpireTime;
			}
			set
			{
				effectObjectPool.ExpireTime = effectObjectPoolExpireTime = value;
			}
		}
		
		/// <summary>
		/// 模块优先级，最小的优先轮询
		/// </summary>
		public override int Priority => 10;

		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init()
		{
			effectObjectPool = GameMain.ObjectPool.CreateObjectPool<EffectObject>("Effect Pool", effectObjectPoolCapacity, effectObjectPoolExpireTime);
			instantiateGameObjectCallbacks = new InstantiateGameObjectCallbacks(OnInstantiateEffectBegin, OnInstantiateEffectSuccess, null, null);
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime)
		{
			CheckUnusedEffectObject();
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			ClearAllEffect();
		}

		/// <summary>
		/// 清理所有特效
		/// </summary>
		public void ClearAllEffect()
		{
			foreach (var loadingTaskId in loadingEffectList)
			{
				GameMain.Resource.ReleaseTask(loadingTaskId);
			}
			
			foreach (var effectObject in usingEffectObjectList)
			{
				effectObjectPool.Recycle(effectObject);
			}
			
			loadingEffectList.Clear();
			usingEffectObjectList.Clear();
			effectObjectPool.ReleaseAllUnused();
		}

		/// <summary>
		/// 播放特效
		/// </summary>
		/// <param name="effectAsset"> 特效资源名称 </param>
		/// <param name="pos"> 特效播放位置 </param>
		/// <param name="rot"> 特效播放角度 </param>
		/// <param name="parentTrans"> 特效父物体 </param>
		public void PlayEffect(string effectAsset, Vector3 pos, Quaternion rot, Transform parentTrans = null)
		{
			if(string.IsNullOrEmpty(effectAsset))
			{
				Debug.Log("EffectModule : Effect asset name is invalid.");
				return;
			}

			if (InternalTryGetAvailableEffectObject(effectAsset, out var effectObject))
			{
				var effectTrans = effectObject.EffectGameObject.transform;
				effectTrans.SetParent(parentTrans);
				effectTrans.localPosition = pos;
				effectTrans.rotation = rot;
				return;
			}
			
			GameMain.Resource.InstantiateAsync(effectAsset, instantiateGameObjectCallbacks, PlayEffectInfo.Create(parentTrans, pos, rot));
		}

		/// <summary>
		/// 暂停所有特效
		/// </summary>
		public void PauseAllEffect()
		{
			foreach (var effectObject in usingEffectObjectList)
			{
				effectObject.Pause();
			}
		}

		/// <summary>
		/// 恢复所有特效
		/// </summary>
		public void ResumeAllEffect()
		{
			foreach (var effectObject in usingEffectObjectList)
			{
				effectObject.Resume();
			}
		}
		
		private void CheckUnusedEffectObject()
		{
			for (var i = usingEffectObjectList.Count - 1; i >= 0; i--)
			{
				var effectObject = usingEffectObjectList[i];
				if (effectObject.IsPlayComplete)
				{
					effectObjectPool.Recycle(effectObject);
					usingEffectObjectList.Remove(effectObject);
				}
			}
		}

		private void OnInstantiateEffectBegin(string effectAssetName, int taskId)
		{
			if (!loadingEffectList.Contains(taskId))
			{
				loadingEffectList.Add(taskId);
			}
		}

		private void OnInstantiateEffectSuccess(string effectAssetName, int taskId, GameObject effectGameObject, object userData)
		{
			if (effectObjectPool.UsingCount + effectObjectPool.UnusedCount >= effectObjectPool.Capacity)
			{
				Debug.LogError($"EffectModule : Effect object pool has reached its maximum capacity.");
				loadingEffectList.Remove(taskId);
				GameMain.Resource.ReleaseAsset(effectGameObject);
				return;
			}
			
			var playEffectInfo = (PlayEffectInfo) userData;
			var effectObject = EffectObject.Create(effectGameObject, transform);
			effectObjectPool.Register(effectObject, true, effectAssetName);
			usingEffectObjectList.Add(effectObject);

			var effectTrans = effectObject.EffectGameObject.transform;
			effectTrans.SetParent(playEffectInfo.ParentTransform);
			effectTrans.localPosition = playEffectInfo.Position;
			effectTrans.rotation = playEffectInfo.Rotation;
		}
		
		private bool InternalTryGetAvailableEffectObject(string effectAssetName, out EffectObject effectObject)
		{
			effectObject = null;
			if (effectObjectPool.CanSpawn(effectAssetName))
			{
				effectObject = effectObjectPool.Spawn(effectAssetName);
				usingEffectObjectList.Add(effectObject);
					
				return true;
			}

			return false;
		}
	}
}
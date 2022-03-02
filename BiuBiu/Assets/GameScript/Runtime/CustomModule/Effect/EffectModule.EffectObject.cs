//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework;
using AureFramework.ObjectPool;
using AureFramework.Resource;
using UnityEngine;

namespace BiuBiu
{
	public sealed partial class EffectModule : AureFrameworkModule, IEffectModule
	{
		/// <summary>
		/// 内部特效对象
		/// </summary>
		private class EffectObject : ObjectBase
		{
			private ParticleSystem[] particleSystemList;
			private Transform effectModuleTransform;
			
			/// <summary>
			/// 特效游戏物体
			/// </summary>
			public GameObject EffectGameObject
			{
				get;
				private set;
			}

			/// <summary>
			/// 是否播放完毕了
			/// </summary>
			public bool IsPlayComplete
			{
				get
				{
					foreach (var particleSystem in particleSystemList)
					{
						if (!particleSystem.isStopped)
						{
							return false;
						}
					}

					return true;
				}
			}

			public void Pause()
			{
				if (IsPlayComplete)
				{
					return;
				}
				
				foreach (var particleSystem in particleSystemList)
				{
					if (!particleSystem.isStopped)
					{
						particleSystem.Pause();
					}
				}
			}

			public void Resume()
			{
				if (IsPlayComplete)
				{
					return;
				}
				
				foreach (var particleSystem in particleSystemList)
				{
					if (!particleSystem.isStopped)
					{
						particleSystem.Play();
					}
				}
			}

			public static EffectObject Create(GameObject effectGameObject, Transform moduleTransform)
			{
				var effectObject = GameMain.ReferencePool.Acquire<EffectObject>();
				effectObject.EffectGameObject = effectGameObject;
				effectObject.effectModuleTransform = moduleTransform;
				effectObject.particleSystemList = effectGameObject.GetComponentsInChildren<ParticleSystem>(true);
				
				return effectObject;
			}

			public override void OnSpawn()
			{
				base.OnSpawn();
				
				foreach (var particleSystem in particleSystemList)
				{
					particleSystem.Play();
				}
				
				EffectGameObject.SetActive(true);
			}

			public override void OnRecycle()
			{
				base.OnRecycle();
				
				foreach (var particleSystem in particleSystemList)
				{
					particleSystem.Stop();
				}
				
				EffectGameObject.SetActive(false);
				EffectGameObject.transform.SetParent(effectModuleTransform);
			}

			public override void OnRelease()
			{
				base.OnRelease();

				Aure.GetModule<IResourceModule>().ReleaseAsset(EffectGameObject);
			}

			public override void Clear()
			{
				base.Clear();

				EffectGameObject = null;
				particleSystemList = null;
			}
		}
	}
}
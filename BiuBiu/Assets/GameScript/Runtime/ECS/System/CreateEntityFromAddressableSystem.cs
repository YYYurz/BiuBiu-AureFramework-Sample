//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.Resource;
using Unity.Entities;
using UnityEngine;

namespace BiuBiu
{
	public class CreateEntityFromAddressableSystem : ComponentSystem
	{
		private readonly Dictionary<string, Entity> originalEntityDic = new Dictionary<string, Entity>();
		private readonly Dictionary<string, int> loadingAssetDic = new Dictionary<string, int>();
		private readonly List<GameObject> originalGameObjectList = new List<GameObject>();
		private readonly List<Entity> cacheEntityList = new List<Entity>();
		private Action preloadOverCallback;
		private InstantiateGameObjectCallbacks instantiateGameObjectCallbacks;

		protected override void OnCreate()
		{
			base.OnCreate();
			
			instantiateGameObjectCallbacks = new InstantiateGameObjectCallbacks(OnInstantiateGameObjectBegin, OnInstantiateGameObjectSuccess, null, OnInstantiateGameObjectFailed);
		}

		protected override void OnUpdate()
		{
			Entities.ForEach((Entity entity, ref ConvertToEntityTagComponent convertToEntityTagComponent) =>
			{
				EntityManager.RemoveComponent<ConvertToEntityTagComponent>(entity);
				
				// 不需要克隆的物体，只需要缓存一下切场景的时候清掉就好了
				if (convertToEntityTagComponent.EntityId == 0)
				{
					cacheEntityList.Add(entity);
					return;
				}
				
				EntityManager.SetEnabled(entity, false);
				var entityData = GameMain.DataTable.GetDataTableReader<EntityTableReader>().GetInfo(convertToEntityTagComponent.EntityId);
				var assetName = entityData.AssetName;
				originalEntityDic.Add(assetName, entity);
				loadingAssetDic.Remove(assetName);
				
				if (loadingAssetDic.Count == 0)
				{
					preloadOverCallback?.Invoke();
				}
			});			
		}

		/// <summary>
		/// 先用Addressable预加载游戏物体，并等待其ConvertToEntity（只有这里预加载过的才能克隆）
		/// </summary>
		/// <param name="entityIdList"> 预加载实体Id列表 </param>
		/// <param name="callback"> 完成回调 </param>
		public void PreConvertEntityFromAddressable(IEnumerable<uint> entityIdList, Action callback)
		{
			preloadOverCallback = callback;
			foreach (var entityId in entityIdList)
			{
				var entityData = GameMain.DataTable.GetDataTableReader<EntityTableReader>().GetInfo(entityId);
				var assetName = entityData.AssetName;
				if (!loadingAssetDic.ContainsKey(assetName))
				{
					GameMain.Resource.InstantiateAsync(assetName, instantiateGameObjectCallbacks, null);
				}
			}
		}

		/// <summary>
		/// 创建实体
		/// </summary>
		/// <param name="entityId"> 实体配表Id </param>
		public Entity InstantiateEntity(uint entityId)
		{
			var entityData = GameMain.DataTable.GetDataTableReader<EntityTableReader>().GetInfo(entityId);
			var assetName = entityData.AssetName;
			if (originalEntityDic.ContainsKey(assetName))
			{
				var instantiateEntity = EntityManager.Instantiate(originalEntityDic[assetName]);
				cacheEntityList.Add(instantiateEntity);
				EntityManager.SetEnabled(instantiateEntity, true);
				return instantiateEntity;
			}

			Debug.LogError($"CreateEntityFromAddressableSystem : This entity is not preloaded, entity Id : {entityId}");
			return default;
		}

		/// <summary>
		/// 销毁实体
		/// </summary>
		/// <param name="entity"> 实体 </param>
		public void DestroyEntity(Entity entity)
		{
			if (!cacheEntityList.Contains(entity))
			{
				Debug.LogError("CreateEntityFromAddressableSystem : Destroy entity failed, this entity was not created by this system.");
				return;
			}
			
			EntityManager.DestroyEntity(entity);
			cacheEntityList.Remove(entity);
		}

		/// <summary>
		/// 清除所有缓存的实体和GameObject以及加载中的任务
		/// </summary>
		public void ClearAllEntity()
		{
			foreach (var loadingTask in loadingAssetDic)
			{
				GameMain.Resource.ReleaseTask(loadingTask.Value);
			}
			
			foreach (var gameObject in originalGameObjectList)
			{
				GameMain.Resource.ReleaseAsset(gameObject);
			}

			foreach (var entity in originalEntityDic)
			{
				EntityManager.DestroyEntity(entity.Value);
			}
			
			foreach (var entity in cacheEntityList)
			{
				EntityManager.DestroyEntity(entity);
			}
			
			loadingAssetDic.Clear();
			originalGameObjectList.Clear();
			originalEntityDic.Clear();
			cacheEntityList.Clear();
			preloadOverCallback = null;
		}

		private void OnInstantiateGameObjectBegin(string assetName, int taskId)
		{
			if (!loadingAssetDic.ContainsValue(taskId))
			{
				loadingAssetDic.Add(assetName, taskId);
			}
		}

		private void OnInstantiateGameObjectSuccess(string assetName, int taskId, GameObject gameObject, object userData)
		{
			if (loadingAssetDic.ContainsValue(taskId))
			{
				gameObject.SetActive(false);
				originalGameObjectList.Add(gameObject);
			}
		}

		private void OnInstantiateGameObjectFailed(string assetName, int taskId, string errorMessage, object userData)
		{
			Debug.LogError($"CreateEntityFromAddressableSystem : Create entity failed, error message : {errorMessage}");
			loadingAssetDic.Remove(assetName);
			if (loadingAssetDic.Count == 0)
			{
				preloadOverCallback?.Invoke();
			}
		}
	}
}
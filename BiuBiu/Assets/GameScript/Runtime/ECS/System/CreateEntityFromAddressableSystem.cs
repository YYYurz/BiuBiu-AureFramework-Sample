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
		private readonly Dictionary<string, List<Action<Entity>>> callbackDic = new Dictionary<string, List<Action<Entity>>>();
		private readonly Dictionary<int, string> loadingAssetDic = new Dictionary<int, string>();
		private readonly Dictionary<string, Entity> originalEntityDic = new Dictionary<string, Entity>();
		private readonly List<GameObject> originalGameObjectList = new List<GameObject>();
		private readonly List<Entity> cacheEntityList = new List<Entity>();
		private InstantiateGameObjectCallbacks instantiateGameObjectCallbacks;

		protected override void OnCreate()
		{
			base.OnCreate();
			
			instantiateGameObjectCallbacks = new InstantiateGameObjectCallbacks(OnInstantiateGameObjectBegin, OnInstantiateGameObjectSuccess, null, OnInstantiateGameObjectFailed);
		}

		private int i = 1;
		protected override void OnUpdate()
		{
			i++;
			Entities.ForEach((Entity entity, ref ConvertToEntityTagComponent convertToEntityTagComponent) =>
			{
				EntityManager.SetEnabled(entity, false);
				EntityManager.RemoveComponent<ConvertToEntityTagComponent>(entity);

				var entityData = GameMain.DataTable.GetDataTableReader<EntityTableReader>().GetInfo(convertToEntityTagComponent.EntityId);
				var assetName = entityData.AssetName;
				if (originalEntityDic.ContainsKey(assetName))
				{
					EntityManager.DestroyEntity(entity);
				}
				else
				{
					originalEntityDic.Add(assetName, entity);
				}

				foreach (var callback in callbackDic[assetName])
				{
					var instantiateEntity = EntityManager.Instantiate(originalEntityDic[assetName]);
					cacheEntityList.Add(instantiateEntity);
					EntityManager.SetEnabled(instantiateEntity, true);
					callback.Invoke(instantiateEntity);
				}
				callbackDic.Remove(assetName);
			});			
		}

		/// <summary>
		/// 创建实体
		/// </summary>
		/// <param name="entityId"> 实体配表Id </param>
		/// <param name="callback"> 完成回调 </param>
		public void CreateEntity(uint entityId, Action<Entity> callback)
		{
			var entityData = GameMain.DataTable.GetDataTableReader<EntityTableReader>().GetInfo(entityId);
			var assetName = entityData.AssetName;
			if (originalEntityDic.ContainsKey(assetName))
			{
				var instantiateEntity = EntityManager.Instantiate(originalEntityDic[assetName]);
				cacheEntityList.Add(instantiateEntity);
				EntityManager.SetEnabled(instantiateEntity, true);
				callback.Invoke(instantiateEntity);
				return;
			}

			if (!callbackDic.ContainsKey(assetName))
			{
				callbackDic.Add(assetName, new List<Action<Entity>>());	
			}
			callbackDic[assetName].Add(callback);

			if (!loadingAssetDic.ContainsValue(assetName))
			{
				GameMain.Resource.InstantiateAsync(assetName, instantiateGameObjectCallbacks, null);
			}
		}

		/// <summary>
		/// 清除所有缓存的实体和GameObject
		/// </summary>
		public void ClearCacheEntity()
		{
			foreach (var loadingTask in loadingAssetDic)
			{
				GameMain.Resource.ReleaseTask(loadingTask.Key);
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
			callbackDic.Clear();
		}

		private void OnInstantiateGameObjectBegin(string assetName, int taskId)
		{
			if (!loadingAssetDic.ContainsKey(taskId))
			{
				loadingAssetDic.Add(taskId, assetName);
			}
		}

		private void OnInstantiateGameObjectSuccess(string assetName, int taskId, GameObject gameObject, object userData)
		{
			if (loadingAssetDic.ContainsKey(taskId))
			{
				gameObject.SetActive(false);
				originalGameObjectList.Add(gameObject);
				loadingAssetDic.Remove(taskId);
			}
		}

		private void OnInstantiateGameObjectFailed(string assetName, int taskId, string errorMessage, object userData)
		{
			Debug.LogError($"CreateEntityFromAddressableSystem : Create entity failed, error message : {errorMessage}");
			loadingAssetDic.Remove(taskId);
			callbackDic.Remove(assetName);
		}
	}
}
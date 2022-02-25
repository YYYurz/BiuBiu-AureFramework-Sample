//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework;
using AureFramework.Utility;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// ECS实体模块
	/// </summary>
	public sealed partial class EntityModule : AureFrameworkModule, IEntityModule
	{
		private readonly Dictionary<string, EntityArchetype> presetArchetypeDic = new Dictionary<string, EntityArchetype>();
		private readonly List<Entity> entityCacheList = new List<Entity>();
		private EntityManager entityManager;
		
		[SerializeField] private EntityPreset[] entityPresetList;

		/// <summary>
		/// 模块优先级，最小的优先轮询
		/// </summary>
		public override int Priority => 99;
		
		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init()
		{
			entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			foreach (var entityPreset in entityPresetList)
			{
				if (!presetArchetypeDic.ContainsKey(entityPreset.entityName))
				{
					var entityName = entityPreset.entityName;
					var componentDataTypeNameList = entityPreset.componentDataTypeNameList;
					var componentTypeArray = new ComponentType[componentDataTypeNameList.Length];
					for (var i = 0; i < componentDataTypeNameList.Length; i++)
					{
						componentTypeArray[i] = Assembly.GetType(componentDataTypeNameList[i]);
					}
					var entityArchetype = entityManager.CreateArchetype(componentTypeArray);
					presetArchetypeDic.Add(entityName, entityArchetype);
				}
				else
				{
					Debug.LogError($"EntityModule : Entity preset name is already exist, name : {entityPreset.entityName}.");
				}
			}
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime)
		{
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			DestroyAllCacheEntity();
		}

		/// <summary>
		/// 创建实体
		/// </summary>
		/// <param name="archetype"> 原型类型 </param>
		/// <returns></returns>
		public Entity CreateEntity(EntityArchetype archetype)
		{
			var entity = entityManager.CreateEntity(archetype);
			entityCacheList.Add(entity);

			return entity;
		}

		/// <summary>
		/// 创建实体
		/// </summary>
		/// <param name="archetype"> 原型类型 </param>
		/// <param name="num"> 数量 </param>
		/// <returns></returns>
		public NativeArray<Entity> CreateEntity(EntityArchetype archetype, int num)
		{
			var entityArray = new NativeArray<Entity>(num, Allocator.Temp);
			entityManager.CreateEntity(archetype, entityArray);
			
			foreach (var entity in entityArray)
			{
				entityCacheList.Add(entity);
			}

			return entityArray;
		}

		/// <summary>
		/// 创建实体
		/// </summary>
		/// <param name="entityPresetName"> 实体预设名称 </param>
		/// <returns></returns>
		public Entity CreateEntity(string entityPresetName)
		{
			if (!presetArchetypeDic.ContainsKey(entityPresetName))
			{
				Debug.LogError($"EntityModule : Entity preset name is invalid, name : {entityPresetName}.");
				return default;
			}

			var entity = entityManager.CreateEntity(presetArchetypeDic[entityPresetName]);
			entityCacheList.Add(entity);

			return entity;
		}
		
		/// <summary>
		/// 创建实体
		/// </summary>
		/// <param name="entityPresetName"> 实体预设名称 </param>
		/// <param name="num"> 数量 </param>
		/// <returns></returns>
		public NativeArray<Entity> CreateEntity(string entityPresetName, int num)
		{
			if (!presetArchetypeDic.ContainsKey(entityPresetName))
			{
				Debug.LogError($"EntityModule : Entity preset name is invalid, name : {entityPresetName}.");
				return default;
			}
			
			var entityArray = new NativeArray<Entity>(num, Allocator.Temp);
			entityManager.CreateEntity(presetArchetypeDic[entityPresetName], entityArray);
			
			foreach (var entity in entityArray)
			{
				entityCacheList.Add(entity);
			}

			return entityArray;
		}

		/// <summary>
		/// 销毁所有实体
		/// </summary>
		public void DestroyAllCacheEntity()
		{
			entityManager.DestroyEntity(entityCacheList.ToNativeArray(Allocator.Temp));
			entityCacheList.Clear();
		}
	}
}
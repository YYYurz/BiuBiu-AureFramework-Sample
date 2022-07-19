//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace TheLoner
{
	public sealed partial class Ecs : IEcs
	{
		private class EntityManager : IEntityManager
		{
			private readonly ConcurrentDictionary<Type, IEntityCreator> entityCreatorDic = new ConcurrentDictionary<Type, IEntityCreator>();
			private readonly ConcurrentDictionary<Type, int> componentTypeIndexDic = new ConcurrentDictionary<Type, int>();
			private readonly ConcurrentDictionary<int, IComponent[]> entityDic = new ConcurrentDictionary<int, IComponent[]>();
			private readonly ConcurrentQueue<IComponent[]> entityCacheQueue = new ConcurrentQueue<IComponent[]>();
			private readonly ConcurrentQueue<List<IComponent>> usingComponentListQueue = new ConcurrentQueue<List<IComponent>>();
			private readonly ConcurrentQueue<List<IComponent>> cacheComponentListQueue = new ConcurrentQueue<List<IComponent>>();
			private readonly ConcurrentQueue<int> waitingRemoveEntityQueue = new ConcurrentQueue<int>();
			private readonly World world;

			private const string assemblyName = "TheLoner";
			private int entityIdCalculator;
			
			public EntityManager(World world)
			{
				var assembly = Assembly.Load(assemblyName);
				var types = assembly.GetTypes();
				var entityCreatorType = typeof(IEntityCreator);
				var componentType = typeof(IComponent);
				var componentIndexCalculator = 0;

				foreach (var type in types)
				{
					if (!type.IsClass || type.IsInterface || type.IsAbstract)
					{
						continue;
					}
					
					if (entityCreatorType.IsAssignableFrom(type))
					{
						var creator = (IEntityCreator)Activator.CreateInstance(type);
						entityCreatorDic.TryAdd(type, creator);
					}
					
					if (componentType.IsAssignableFrom(type))
					{
						componentTypeIndexDic.TryAdd(type, componentIndexCalculator++);
					}
				}

				this.world = world;
			}

			public void Update()
			{
				ReleaseUsingComponentList();
				CheckMarkRemoveComponent();
				CheckCanRemoveEntity();
			}

			public void CreateEntity<T>(ICreateEntityData createEntityData) where T : IEntityCreator
			{
				if (!entityCreatorDic.TryGetValue(typeof(T), out var entityCreator))
				{
					Logger.LogError("Can not find entity creator.");
					return;
				}

				var entityId = CreateEntityId();
				entityCreator.OnCreateEntity(entityId, createEntityData);

				var componentArray = GetEntityComponentArray();
				entityDic.TryAdd(entityId, componentArray);
			}

			public void RemoveEntity(int entityId)
			{
				if (entityDic.TryGetValue(entityId, out var componentArray))
				{
					foreach (var component in componentArray)
					{
						if (component != null)
						{
							component.MarkRemove = true;
						}
					}
				}
			}

			public void RemoveAllEntity()
			{
				foreach (var components in entityDic)
				{
					RemoveEntity(components.Key);
				}
			}

			public void AddComponent(int entityId, IComponent component)
			{
				var componentType = component.GetType();
				if (!componentTypeIndexDic.TryGetValue(componentType, out var componentTypeIndex))
				{
					Logger.LogError("Component type is invalid.");
					return;
				}
				
				if (!entityDic.TryGetValue(entityId, out var componentArray))
				{
					return;
				}

				if (componentArray[componentTypeIndex] != null)
				{
					Logger.LogError($"This component is already exist, entity Id :{entityId}.");
					return;
				}

				componentArray[componentTypeIndex] = component;
			}

			public List<IComponent> GetComponentList<T>() where T : class, IComponent
			{
				var componentType = typeof(T);
				if (!componentTypeIndexDic.TryGetValue(componentType, out var componentTypeIndex))
				{
					Logger.LogError("Component type is invalid.");
					return null;
				}

				var componentList = GetComponentList();
				foreach (var entityComponentArray in entityDic)
				{
					var component = entityComponentArray.Value[componentTypeIndex];
					if (component != null)
					{
						componentList.Add(component);
					}
				}

				return componentList;
			}

			public T GetComponentByEntityId<T>(int entityId) where T : class, IComponent
			{
				var componentType = typeof(T);
				if (!componentTypeIndexDic.TryGetValue(componentType, out var componentTypeIndex))
				{
					Logger.LogError("Component type is invalid.");
					return null;
				}
				
				if (entityDic.TryGetValue(entityId, out var entityComponentArray))
				{
					return entityComponentArray[componentTypeIndex] as T;
				}

				return null;
			}

			private void CheckMarkRemoveComponent()
			{
				foreach (var entityComponentArray in entityDic)
				{
					for (var i = entityComponentArray.Value.Length - 1; i >= 0; i--)
					{
						if (entityComponentArray.Value[i] != null && entityComponentArray.Value[i].MarkRemove)
						{
							world.ReferencePool.Release(entityComponentArray.Value[i]);
							entityComponentArray.Value[i] = null;
						}
					}
				}
			}

			private void CheckCanRemoveEntity()
			{
				foreach (var entityComponentArray in entityDic)
				{
					var canRemove = true;
					foreach (var component in entityComponentArray.Value)
					{
						if (component != null)
						{
							canRemove = false;
						}
					}

					if (canRemove)
					{
						waitingRemoveEntityQueue.Enqueue(entityComponentArray.Key);
					}
				}

				while (waitingRemoveEntityQueue.Count > 0)
				{
					waitingRemoveEntityQueue.TryDequeue(out var removeEntityId);
					if (entityDic.TryRemove(removeEntityId, out var entityComponentArray))
					{
						CacheEntityComponentArray(entityComponentArray);
					}
				}
			}

			private IComponent[] GetEntityComponentArray()
			{
				if (entityCacheQueue.TryDequeue(out var componentArray))
				{
					return componentArray;
				}
				
				componentArray = new IComponent[componentTypeIndexDic.Count];

				return componentArray;
			}

			private void CacheEntityComponentArray(IComponent[] componentArray)
			{
				for (var i = componentArray.Length - 1; i >= 0; i--)
				{
					var component = componentArray[i];
					if (component != null)
					{
						world.ReferencePool.Release(component);
					}

					componentArray[i] = null;
				}
				
				entityCacheQueue.Enqueue(componentArray);
			}

			private List<IComponent> GetComponentList()
			{
				if (cacheComponentListQueue.TryDequeue(out var componentList))
				{
					return componentList;
				}
				
				componentList = new List<IComponent>();
				usingComponentListQueue.Enqueue(componentList);

				return componentList;
			}
			
			private void ReleaseUsingComponentList()
			{
				while (usingComponentListQueue.Count > 0)
				{
					if (!usingComponentListQueue.TryDequeue(out var componentList))
					{
						break;
					}
					
					componentList.Clear();
					cacheComponentListQueue.Enqueue(componentList);
				}
			}

			private int CreateEntityId()
			{
				if (entityIdCalculator == int.MaxValue)
				{
					entityIdCalculator = 0;
				}

				return ++entityIdCalculator;
			}
		}
	}
}
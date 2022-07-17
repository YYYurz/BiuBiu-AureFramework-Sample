//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace TheLoner
{
	public sealed partial class Ecs : IEcs
	{
		private class SystemManager : ISystemManager
		{
			private readonly LinkedList<AbstractSystem> systemLinked = new LinkedList<AbstractSystem>();
			private readonly World world;

			public SystemManager(World world)
			{
				this.world = world;
			}
			
			public void Update()
			{
				foreach (var system in systemLinked)
				{
					system.OnUpdate();
				}
			}
			
			public void AddSystem<T>() where T : AbstractSystem
			{
				var curNode = systemLinked.First;
				while (curNode != null)
				{
					if (curNode.Value.GetType() == typeof(T))
					{
						Logger.LogError("System is already exist.");
						return;
					}

					curNode = curNode.Next;
				}

				var system = Activator.CreateInstance<T>();
				system.OnAwake();

				systemLinked.AddLast(system);
			}

			public void RemoveSystem<T>(int entityId) where T : AbstractSystem
			{
				var curNode = systemLinked.First;
				while (curNode != null)
				{
					if (curNode.Value.GetType() == typeof(T))
					{
						curNode.Value.OnDestroy();
						systemLinked.Remove(curNode);
						break;
					}

					curNode = curNode.Next;
				}
			}

			public void RemoveAllSystem()
			{
				var curNode = systemLinked.First;
				while (curNode != null)
				{
					curNode.Value.OnDestroy();
					curNode = curNode.Next;
				}
				
				systemLinked.Clear();
			}
		}
	}
}
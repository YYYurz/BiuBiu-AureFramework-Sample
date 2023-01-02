//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Threading;

namespace TheLoner
{
	public sealed partial class Ecs
	{
		private class World : IWorld
		{
			private readonly EntityManager entityManager;
			private readonly SystemManager systemManager;
			private readonly UtilityManager utilityManager;
			private readonly ReferencePool referencePool;
			private readonly IInitData initData;
			private bool isRunning;

			public World(IInitData initData)
			{
				this.initData = initData;
				referencePool = new ReferencePool();
				entityManager = new EntityManager(this);
				systemManager = new SystemManager(this);
				utilityManager = new UtilityManager(this);
				isRunning = true;
				ThreadPool.QueueUserWorkItem(ThreadProc);
			}

			public ReferencePool ReferencePool
			{
				get
				{
					return referencePool;
				}
			}
			
			public IEntityManager EntityManager
			{
				get
				{
					return entityManager;
				}
			}

			public ISystemManager SystemManager
			{
				get
				{
					return systemManager;
				}
			}

			public IUtilityManager UtilityManager
			{
				get
				{
					return utilityManager;
				}
			}

			public object InitData
			{
				get
				{
					return initData;
				}
			}

			public void Start()
			{
				isRunning = true;
				try
				{
					utilityManager.AwakeAllUtility();
					systemManager.AwakeAllSystem();
				}
				catch (Exception e)
				{
					Logger.LogError(e.Message);
				}
			}
			
			private void ThreadProc(object state)
			{
				while (isRunning)
				{
					Thread.Sleep(50);
					
					try
					{
						systemManager.Update();
						entityManager.Update();
					}
					catch (Exception e)
					{
						Logger.LogError(e.Message);
					}
				}
			}

			public void Clear()
			{
				isRunning = false;
				entityManager.RemoveAllEntity();
				systemManager.RemoveAllSystem();
			}
		}
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Threading;

namespace TheLoner
{
	public sealed partial class Ecs
	{
		private class World : IWorld
		{
			private readonly EntityManager entityManager;
			private readonly SystemManager systemManager;
			private readonly ReferencePool referencePool;
			private readonly ManualResetEvent manualResetEvent;

			public World()
			{
				referencePool = new ReferencePool();
				entityManager = new EntityManager(this);
				systemManager = new SystemManager(this);
				manualResetEvent = new ManualResetEvent(true);

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

			private void ThreadProc(object state)
			{
				while (true)
				{
					systemManager.Update();
					entityManager.Update();
					manualResetEvent.Set();
					Thread.Sleep(50);
				}
			}

			public void Clear()
			{
				manualResetEvent.WaitOne();
				entityManager.RemoveAllEntity();
				systemManager.RemoveAllSystem();
			}
		}
	}
}
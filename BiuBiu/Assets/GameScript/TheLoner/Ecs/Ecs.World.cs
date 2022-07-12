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

			public World()
			{
				entityManager = new EntityManager();
				systemManager = new SystemManager();

				ThreadPool.QueueUserWorkItem(ThreadPro);
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

			private void ThreadPro(object state)
			{
				while (true)
				{
					systemManager.Update();
					Thread.Sleep(50);
				}
			}

			public void Clear()
			{
				entityManager.DestroyAllEntity();
				systemManager.DestroyAllSystem();
			}
		}
	}
}
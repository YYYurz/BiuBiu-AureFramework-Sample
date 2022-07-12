//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;

namespace TheLoner
{
	public sealed partial class Ecs : IEcs
	{
		private class SystemManager : ISystemManager
		{
			private readonly LinkedList<AbstractSystem> systemLinked = new LinkedList<AbstractSystem>();
			
			public void Update()
			{
				
			}
			
			public void AddSystem<T>() where T : ISystem
			{
				
			}

			public void DeleteSystem<T>(int entityId) where T : ISystem
			{
				
			}

			public void DestroyAllSystem()
			{
				
			}
		}
	}
}
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
	public sealed partial class Ecs
	{
		private readonly List<World> worldList = new List<World>();
		
		public void Clear()
		{
			foreach (var world in worldList)
			{
				world.Clear();
			}
			
			worldList.Clear();
		}

		/// <summary>
		/// 创建世界
		/// </summary>
		/// <param name="initData"></param>
		/// <returns></returns>
		public IWorld CreateWorld(IInitData initData)
		{
			var world = new World(initData);
			worldList.Add(world);

			return world;
		}

		/// <summary>
		/// 销毁世界
		/// </summary>
		/// <param name="world"></param>
		public void DestroyWorld(IWorld world)
		{
			var internalWorld = (World) world;
			internalWorld.Clear();
			worldList.Remove(internalWorld);
		}
	}
}
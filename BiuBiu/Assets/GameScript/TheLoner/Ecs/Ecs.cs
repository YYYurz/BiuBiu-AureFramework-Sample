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
	/// <summary>
	/// ECS实体模块
	/// </summary>
	public sealed partial class Ecs : IEcs
	{
		private readonly List<World> worldList = new List<World>();
		
		public void Init()
		{
			
		}

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
		/// <returns></returns>
		public IWorld CreateWorld()
		{
			var world = new World();
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
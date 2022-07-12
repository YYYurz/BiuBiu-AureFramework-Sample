//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public sealed partial class Ecs : IEcs
	{
		private class EntityManager : IEntityManager
		{
			public void CreateEntity<T>() where T : IEntityCreator
			{
				
			}

			public void DeleteEntity(int entityId)
			{
				
			}

			public void DestroyAllEntity()
			{
				
			}
		}
	}
}
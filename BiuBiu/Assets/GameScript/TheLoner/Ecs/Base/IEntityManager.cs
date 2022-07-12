//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public interface IEntityManager
	{
		/// <summary>
		/// 创建实体
		/// </summary>
		void CreateEntity<T>() where T : IEntityCreator;

		/// <summary>
		/// 删除实体
		/// </summary>
		/// <param name="entityId"></param>
		void DeleteEntity(int entityId);

		/// <summary>
		/// 销毁所有实体
		/// </summary>
		void DestroyAllEntity();
	}
}
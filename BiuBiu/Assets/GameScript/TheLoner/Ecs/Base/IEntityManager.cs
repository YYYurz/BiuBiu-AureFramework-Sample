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
	public interface IEntityManager
	{
		/// <summary>
		/// 创建实体
		/// </summary>
		void CreateEntity<T>(ICreateEntityData createEntityData) where T : IEntityCreator;

		/// <summary>
		/// 移除实体
		/// </summary>
		/// <param name="entityId"></param>
		void RemoveEntity(int entityId);

		/// <summary>
		/// 移除所有实体
		/// </summary>
		void RemoveAllEntity();

		/// <summary>
		/// 往实体添加组件
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="component"></param>
		void AddComponent(int entityId, IComponent component);

		/// <summary>
		/// 按类型获取组件列表
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		List<T> GetComponentList<T>() where T : IComponent;
		
		/// <summary>
		/// 按类型和实体Id获取组件
		/// </summary>
		/// <param name="entityId"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T GetComponentByEntityId<T>(int entityId) where T : IComponent;
	}
}
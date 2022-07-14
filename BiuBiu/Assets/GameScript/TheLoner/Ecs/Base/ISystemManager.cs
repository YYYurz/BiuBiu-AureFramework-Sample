//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public interface ISystemManager
	{
		/// <summary>
		/// 添加系统
		/// </summary>
		void AddSystem<T>() where T : AbstractSystem;

		/// <summary>
		/// 删除系统
		/// </summary>
		/// <param name="entityId"></param>
		void DeleteSystem<T>(int entityId) where T : AbstractSystem;

		/// <summary>
		/// 销毁所有系统
		/// </summary>
		void DestroyAllSystem();
	}
}
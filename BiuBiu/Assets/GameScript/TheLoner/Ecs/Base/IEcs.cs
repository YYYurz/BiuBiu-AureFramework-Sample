//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	/// <summary>
	/// Ecs实体模块接口
	/// </summary>
	public interface IEcs
	{
		/// <summary>
		/// 创建世界（由）
		/// </summary>
		/// <returns></returns>
		IWorld CreateWorld();

		/// <summary>
		/// 销毁世界
		/// </summary>
		/// <param name="world"></param>
		void DestroyWorld(IWorld world);
	}
}
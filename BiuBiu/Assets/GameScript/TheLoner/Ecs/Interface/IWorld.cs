//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public interface IWorld
	{
		ReferencePool ReferencePool
		{
			get;
		}
		
		IEntityManager EntityManager
		{
			get;
		}

		ISystemManager SystemManager
		{
			get;
		}

		IUtilityManager UtilityManager
		{
			get;
		}

		object InitData
		{
			get;
		}

		void Start();
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public interface IUtilityManager
	{
		void AddUtility<T>() where T : AbstractUtility;
		
		T GetUtility<T>() where T : AbstractUtility;

		void RemoveAllUtility();
	}
}
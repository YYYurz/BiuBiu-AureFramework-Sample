//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace BiuBiu
{
	public static partial class LuaHelper
	{
		/// <summary>
		/// 强制切换切场景流程
		/// </summary>
		/// <param name="sceneName"></param>
		public static void ChangeScene(string sceneName)
		{
			GameMain.Procedure.ChangeProcedure<ProcedureChangeScene>(sceneName);
		}
	}
}
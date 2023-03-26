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
		/// <param name="sceneType"> 场景类型 </param>
		/// <param name="sceneId"> 场景Id </param>
		public static void ChangeScene(int sceneType, uint sceneId)
		{
			GameMain.Procedure.ChangeProcedure<ProcedureChangeScene>((SceneType) sceneType, sceneId);
		}
	}
}
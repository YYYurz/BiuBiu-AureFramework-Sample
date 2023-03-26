//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;

namespace DrunkFish
{
	public class ProcedureCheckVersion : ProcedureBase
	{
		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);

			// var uiRoot = GameMain.UI.transform;
			// var startWindow = StartWindow.CreateStartWindow(uiRoot);
			// startWindowScript = startWindow.GetComponent<StartWindow>();
		}

		public override void OnUpdate(float elapseTime, float realElapseTime)
		{
			base.OnUpdate(elapseTime, realElapseTime);

			ChangeState<ProcedurePreload>();
		}
	}
}
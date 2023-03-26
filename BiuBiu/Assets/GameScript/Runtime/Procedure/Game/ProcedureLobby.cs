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
	public class ProcedureLobby : ProcedureBase
	{
		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);

			var a = 0.112234;
			var s = a * 100f % 1 / 100f;

		}
		
		public override void OnUpdate(float elapseTime, float realElapseTime)
		{
			base.OnUpdate(elapseTime, realElapseTime);

		
		}

	}
}
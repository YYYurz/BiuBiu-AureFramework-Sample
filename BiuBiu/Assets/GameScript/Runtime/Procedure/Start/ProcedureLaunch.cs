//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using UnityEngine;

namespace BiuBiu
{
	public class ProcedureLaunch : ProcedureBase
	{
		public override void OnEnter(params object[] args)
		{
			Screen.orientation = ScreenOrientation.AutoRotation;
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			Screen.autorotateToLandscapeLeft = true;
			Screen.autorotateToLandscapeRight = true;
		}

		public override void OnUpdate(float elapseTime, float realElapseTime)
		{
			base.OnUpdate(elapseTime, realElapseTime);
			
			ChangeState<ProcedureCheckVersion>();
		}
	}
}
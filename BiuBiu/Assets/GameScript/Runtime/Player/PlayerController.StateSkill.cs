//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Fsm;
using UnityEngine;

namespace BiuBiu
{
	public sealed partial class PlayerController : MonoBehaviour
	{
		/// <summary>
		/// 攻击2状态
		/// </summary>
		private sealed class StateSkill : PlayerControllerStateBase
		{
			private PlayerController playerController;
			private bool canChange;
			
			public override bool CanChange
			{
				get
				{
					return canChange;
				}
			}
			
			public override void OnInit(IFsm fsm, params object[] userData)
			{
				base.OnInit(fsm, userData);

				playerController = (PlayerController) userData[0];
			}

			public override void OnEnter(params object[] args)
			{
				base.OnEnter(args);
				
				playerController.animator.Play("AnimSkill");
				playerController.animator.Update(0f);
			}
			
			public override void OnUpdate(float elapseTime, float realElapseTime)
			{
				base.OnUpdate(elapseTime, realElapseTime);
				
				var stateInfo = playerController.animator.GetCurrentAnimatorStateInfo(0);
				if (stateInfo.normalizedTime > 1f)
				{
					canChange = true;
					ChangeState<StateIdle>();
				}
			}
		}
	}
}
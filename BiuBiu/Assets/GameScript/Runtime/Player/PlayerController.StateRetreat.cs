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
		/// 闪避状态
		/// </summary>
		private sealed class StateRetreat : PlayerControllerStateBase
		{
			private PlayerController playerController;
			private Vector3 retreatDirection;
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

				canChange = false;
				retreatDirection = playerController.transform.forward;
				playerController.animator.Play("AnimRetreat");
			}
			
			public override void OnUpdate(float elapseTime, float realElapseTime)
			{
				base.OnUpdate(elapseTime, realElapseTime);
				
				var trans = playerController.transform;
				var pos = trans.position;
				var offset = retreatDirection * (realElapseTime * 7f);
				var nextPos = new Vector3(pos.x + offset.x, pos.y, pos.z + offset.z); 
				trans.position = nextPos;
				
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
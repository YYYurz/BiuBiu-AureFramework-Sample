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
			
			public override void Pause()
			{
				playerController.animator.speed = 0f;
			}

			public override void Resume()
			{
				playerController.animator.speed = 1f;
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
				if (playerController.curDirection.Equals(Vector2.zero))
				{
					retreatDirection = playerController.transform.forward;
				}
				else
				{
					retreatDirection = playerController.curDirection;
				}
				
				var position = playerController.transform.position;
				var directionPos = new Vector3(position.x + retreatDirection.x, position.y, position.z + retreatDirection.y); 
				playerController.transform.LookAt(directionPos);
				playerController.animator.Play("AnimRetreat");
			}
			
			public override void OnUpdate(float elapseTime, float realElapseTime)
			{
				base.OnUpdate(elapseTime, realElapseTime);
				
				var trans = playerController.transform;
				var pos = trans.position;
				var offset = retreatDirection * (realElapseTime * playerController.retreatSpeed);
				var nextPos = new Vector3(pos.x + offset.x, pos.y, pos.z + offset.y); 
				trans.position = nextPos;
				
				var stateInfo = playerController.animator.GetCurrentAnimatorStateInfo(0);
				if (stateInfo.normalizedTime > 0.9f)
				{
					canChange = true;
					ChangeState<StateIdle>();
				}
			}
		}
	}
}
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
		/// 攻击1状态
		/// </summary>
		private sealed class StateAttack1 : PlayerControllerStateBase
		{
			private string[] attackAnimStageArray = {
				"AnimAttack1",
				"AnimAttack2",
				"AnimAttack3",
			};
			private PlayerController playerController;
			private Vector3 attackDirection;

			private const float AttackInterval = 0.2f;
			private int attackStage = 1;
			private float curAttackEndTime;

			private bool curAttackComplete;
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

			private AnimatorStateInfo stateInfo;
			public override void OnEnter(params object[] args)
			{
				base.OnEnter(args);

				RefreshAttackStage();
				canChange = false;
				curAttackComplete = false;
				attackDirection = playerController.transform.forward;
				playerController.animator.Play(attackAnimStageArray[attackStage]);
				playerController.animator.Update(0f);
			}

			public override void OnUpdate(float elapseTime, float realElapseTime)
			{
				base.OnUpdate(elapseTime, realElapseTime);
				
				var trans = playerController.transform;
				var pos = trans.position;
				var offset = attackDirection * (realElapseTime * 2f);
				var nextPos = new Vector3(pos.x + offset.x, pos.y, pos.z + offset.z); 
				trans.position = nextPos;
				
				stateInfo = playerController.animator.GetCurrentAnimatorStateInfo(0);
				if (stateInfo.normalizedTime > 1f)
				{
					canChange = true;
					curAttackComplete = true;
					ChangeState<StateIdle>();
				}
			}

			public override void OnExit()
			{
				base.OnExit();
				
				curAttackEndTime = Time.realtimeSinceStartup;
			}

			private void RefreshAttackStage()
			{
				if (curAttackComplete && Time.realtimeSinceStartup - curAttackEndTime <= AttackInterval)
				{
					attackStage = attackStage + 1 > 2 ? 0 : attackStage + 1;
					return;
				}

				attackStage = 0;
			}
		}
	}
}
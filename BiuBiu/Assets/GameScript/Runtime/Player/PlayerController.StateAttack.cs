//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Fsm;
using Unity.Mathematics;
using UnityEngine;

namespace BiuBiu
{
	public sealed partial class PlayerController : MonoBehaviour
	{
		/// <summary>
		/// 攻击1状态
		/// </summary>
		private sealed class StateAttack : PlayerControllerStateBase
		{
			private readonly string[] attackAnimStageArray = {
				"AnimAttack1",
				"AnimAttack2",
				"AnimAttack3",
				"AnimAttack4",
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

			public override void OnEnter(params object[] args)
			{
				base.OnEnter(args);

				RefreshAttackStage();
				FlashToAreaNearestTarget();
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
				var offset = attackDirection * realElapseTime;
				var nextPos = new Vector3(pos.x + offset.x, pos.y, pos.z + offset.z); 
				trans.position = nextPos;
				
				var stateInfo = playerController.animator.GetCurrentAnimatorStateInfo(0);
				if (stateInfo.normalizedTime > 0.8f)
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

			/// <summary>
			/// 如果有范围内瞄准的目标，直接闪现到跟前
			/// </summary>
			private void FlashToAreaNearestTarget()
			{
				var playerAreaTargetChecker = playerController.playerAreaTargetChecker;
				var transform = playerController.transform;
				var position = transform.position;
				if (playerAreaTargetChecker.TryGetAreaNearestTarget(out var targetPosition))
				{
					var direction = (targetPosition - position).normalized;
					var angle = Vector3.Angle(transform.forward, direction);
					// 就在近处或者没有朝向目标就不闪了
					if (math.distance(targetPosition, position) <= 2f || angle > 90)
					{
						return;
					}
					
					transform.LookAt(targetPosition);
					var intervalDistance = -direction;
					transform.position = targetPosition + intervalDistance;
				}
			}

			/// <summary>
			/// 当前攻击到第几段
			/// </summary>
			private void RefreshAttackStage()
			{
				if (curAttackComplete && Time.realtimeSinceStartup - curAttackEndTime <= AttackInterval)
				{
					attackStage = attackStage + 1 > 3 ? 0 : attackStage + 1;
					return;
				}

				attackStage = 0;
			}
		}
	}
}
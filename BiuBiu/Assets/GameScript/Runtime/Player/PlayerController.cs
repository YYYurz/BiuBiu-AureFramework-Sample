//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.Event;
using AureFramework.Fsm;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 主角控制
	/// </summary>
	public sealed partial class PlayerController : MonoBehaviour
	{
		private CameraController cameraController;
		private GameObject aimingTips;
		private IFsm playerFsm;
		private Vector2 curDirection;
		
		[SerializeField] private PlayerAreaTargetChecker playerAreaTargetChecker;
		[SerializeField] private Animator animator;
		[SerializeField] private float damage;
		[SerializeField] private float skillDamage;
		[SerializeField] private float health;
		[SerializeField] private float moveSpeed;
		[SerializeField] private float retreatSpeed;

		private void Awake()
		{
			if (Camera.main != null)
			{
				cameraController = GameObject.Find("MainCameraController").GetComponent<CameraController>();
				cameraController.SetFollowTarget(transform);
			}

			var playerStateTypeList = new List<Type>
			{
				typeof(StateAttack),
				typeof(StateSkill),
				typeof(StateDead),
				typeof(StateIdle),
				typeof(StateMove),
				typeof(StateRetreat),
			};
			playerFsm = GameMain.Fsm.CreateFsm(this, playerStateTypeList, this);

			aimingTips = GameMain.Resource.InstantiateSync("AimingTips");
			aimingTips.SetActive(false);
			GameMain.Event.Subscribe<InputEventArgs>(OnInput);
			
			playerFsm.ChangeState<StateIdle>();
		}

		private void Update()
		{
			if (health > 0f)
			{
				RefreshAimingTips();
			}
			else
			{
				Dead();
			}
		}

		private void OnDestroy()
		{
			GameMain.Fsm.DestroyFsm(this);
			GameMain.Resource.ReleaseAsset(aimingTips);
			GameMain.Event.Unsubscribe<InputEventArgs>(OnInput);
		}

		/// <summary>
		/// 暂停
		/// </summary>
		public void Pause()
		{
			playerFsm.Pause();
		}

		/// <summary>
		/// 恢复
		/// </summary>
		public void Resume()
		{
			playerFsm.Resume();
		}

		/// <summary>
		/// 待机
		/// </summary>
		private void Idle()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (playerState.CanChange && !(playerState is StateIdle))
			{
				playerFsm.ChangeState<StateIdle>();
			}
		}
		
		/// <summary>
		/// 移动
		/// </summary>
		private void Move()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (playerState.CanChange && !(playerState is StateMove))
			{
				playerFsm.ChangeState<StateMove>();
			}
		}

		/// <summary>
		/// 攻击1
		/// </summary>
		private void Attack1()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (playerState.CanChange)
			{
				playerFsm.ChangeState<StateAttack>();
			}
		}

		/// <summary>
		/// 攻击2
		/// </summary>
		private void Attack2()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (playerState.CanChange)
			{
				playerFsm.ChangeState<StateSkill>();
			}
		}
		
		/// <summary>
		/// 闪避
		/// </summary>
		private void Retreat()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (!(playerState is StateRetreat))
			{
				playerFsm.ChangeState<StateRetreat>();
			}
		}

		/// <summary>
		/// 死亡
		/// </summary>
		private void Dead()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (!(playerState is StateDead))
			{
				playerFsm.ChangeState<StateDead>();
			}
		}

		/// <summary>
		/// 刷新范围内瞄准敌人提示
		/// </summary>
		private void RefreshAimingTips()
		{
			if (playerAreaTargetChecker.TryGetAreaNearestTarget(out var targetPosition))
			{
				if (!aimingTips.activeSelf)
				{
					aimingTips.SetActive(true);
				}

				aimingTips.transform.position = targetPosition + new Vector3(0f, 1.5f, 0f);
			}
			else
			{
				if (aimingTips.activeSelf)
				{
					aimingTips.SetActive(false);
				}
			}
		}
		
		private void OnInput(object sender, AureEventArgs e)
		{
			if (e == null || !GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause || health <= 0f)
			{
				return;
			}
			
			var args = (InputEventArgs) e;
			switch (args.InputType)
			{
				case ECSConstant.InputType.None:
				{
					Idle();
					break;
				}
				case ECSConstant.InputType.Direction:
				{
					Move();
					break;
				}
				case ECSConstant.InputType.Attack:
				{
					Attack1();
					break;
				}
				case ECSConstant.InputType.Skill:
				{
					Attack2();
					break;
				}
				case ECSConstant.InputType.Retreat:
				{
					Retreat();
					break;
				}
			}

			curDirection = args.Direction;
		}
	}
}
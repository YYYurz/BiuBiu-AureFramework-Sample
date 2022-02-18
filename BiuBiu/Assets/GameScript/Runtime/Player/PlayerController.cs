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
		private IFsm playerFsm;
		private Vector2 curDirection;
		
		[SerializeField] private Animator animator;
		[SerializeField] private float moveSpeed;
		[SerializeField] private float health;

		private void Awake()
		{
			if (Camera.main != null)
			{
				cameraController = Camera.main.GetComponent<CameraController>();
				cameraController.SetFollowTarget(transform);
			}

			var playerStateTypeList = new List<Type>
			{
				typeof(StateAttack1),
				typeof(StateAttack2),
				typeof(StateDead),
				typeof(StateIdle),
				typeof(StateMove),
				typeof(StateRetreat),
			};
			playerFsm = GameMain.Fsm.CreateFsm(this, playerStateTypeList, this);			
			
			GameMain.Event.Subscribe<InputEventArgs>(OnInput);
		}

		private void Start()
		{
			playerFsm.ChangeState<StateIdle>();
		}

		private void OnDestroy()
		{
			GameMain.Fsm.DestroyFsm(this);

			GameMain.Event.Unsubscribe<InputEventArgs>(OnInput);
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
				playerFsm.ChangeState<StateAttack1>();
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
				playerFsm.ChangeState<StateAttack2>();
			}
		}
		
		/// <summary>
		/// 闪避
		/// </summary>
		private void Retreat()
		{
			playerFsm.ChangeState<StateRetreat>();
		}

		/// <summary>
		/// 死亡
		/// </summary>
		private void Dead()
		{
			playerFsm.ChangeState<StateDead>();
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
				case ECSConstant.InputType.Attack1:
				{
					Attack1();
					break;
				}
				case ECSConstant.InputType.Attack2:
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
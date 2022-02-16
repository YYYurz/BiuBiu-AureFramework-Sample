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
		private IFsm playerFsm;
		private Vector2 curDirection;
		
		[SerializeField] private Animator animator;
		[SerializeField] private float moveSpeed;
		[SerializeField] private float health;

		private void Awake()
		{
			var playerStateTypeList = new List<Type>
			{
				typeof(StateAttack1),
				typeof(StateAttack2),
				typeof(StateDead),
				typeof(StateIdle),
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
			GameMain.Fsm.DestroyFsm(playerFsm);

			GameMain.Event.Unsubscribe<InputEventArgs>(OnInput);
		}

		private void Idle()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (playerState.CanChange)
			{
				playerFsm.ChangeState<StateIdle>();
			}
		}
		
		private void Move()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (playerState.CanChange)
			{
				playerFsm.ChangeState<StateMove>();
			}
		}

		private void Attack1()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (playerState.CanChange)
			{
				playerFsm.ChangeState<StateAttack1>();
			}
		}

		private void Attack2()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (playerState.CanChange)
			{
				playerFsm.ChangeState<StateAttack2>();
			}
		}

		private void Retreat()
		{
			var playerState = (PlayerControllerStateBase) playerFsm.CurrentState;
			if (playerState.CanChange)
			{
				playerFsm.ChangeState<StateRetreat>();
			}
		}

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
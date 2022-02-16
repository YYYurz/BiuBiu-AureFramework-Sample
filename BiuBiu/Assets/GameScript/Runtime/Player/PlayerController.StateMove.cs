﻿//------------------------------------------------------------
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
		/// 移动状态
		/// </summary>
		private sealed class StateMove : PlayerControllerStateBase
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
				
				playerController.animator.CrossFade("normal_walking", 0.1f);
			}

			public override void OnUpdate()
			{
				base.OnUpdate();

				if (playerController.curDirection.Equals(Vector2.zero))
				{
					return;
				}
				
				
			}
		}
	}
}
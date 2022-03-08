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

				canChange = true;
				playerController.animator.CrossFade("AnimRun", 0.1f);
			}

			public override void OnUpdate(float elapseTime, float realElapseTime)
			{
				base.OnUpdate(elapseTime, realElapseTime);

				if (playerController.curDirection.Equals(Vector2.zero))
				{
					return;
				}

				var trans = playerController.transform;
				var pos = trans.position;
				var offset = playerController.curDirection * (realElapseTime * playerController.moveSpeed);
				var nextPos = new Vector3(pos.x + offset.x, pos.y, pos.z + offset.y); 
				trans.LookAt(nextPos);
				trans.position = nextPos;
			}
		}
	}
}
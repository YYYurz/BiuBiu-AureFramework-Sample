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
		/// 待机状态
		/// </summary>
		private sealed class StateIdle : PlayerControllerStateBase
		{
			private PlayerController playerController;
			private float timer;
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

				timer = 0f;
				canChange = true;
				playerController.animator.CrossFade("AnimIdle1", 0.2f);
			}

			public override void OnUpdate(float elapseTime, float realElapseTime)
			{
				base.OnUpdate(elapseTime, realElapseTime);

				var stateInfo = playerController.animator.GetCurrentAnimatorStateInfo(0);
				timer += GameMain.GamePlay.IsPause || !GameMain.GamePlay.IsStart ? 0f : realElapseTime;
				if (timer >= 10f)
				{
					timer = 0f;
					playerController.animator.CrossFade("AnimIdle2", 0.1f);
					Debug.LogError(1);
				}
				else if (stateInfo.IsName("AnimIdle2") && stateInfo.normalizedTime >= 1f)
				{
					timer = 0f;
					playerController.animator.CrossFade("AnimIdle1", 0.2f);
					Debug.LogError(2);
				}
			}
		}
	}
}
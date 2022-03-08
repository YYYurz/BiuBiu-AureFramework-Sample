//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BiuBiu
{
	public sealed class BattleWindow : UIFormBase
	{
		[SerializeField] private UIJoystick joyStick; 
		[SerializeField] private Button quitGameBtn; 
		[SerializeField] private Button attackBtn1; 
		[SerializeField] private Button attackBtn2; 
		[SerializeField] private Button retreatBtn; 
		
		public override void OnInit(object userData)
		{
			base.OnInit(userData);

			quitGameBtn.onClick.AddListener(OnClickQuitGame);
			attackBtn1.onClick.AddListener(OnClickAttack1);
			attackBtn2.onClick.AddListener(OnClickAttack2);
			retreatBtn.onClick.AddListener(OnClickRetreat);
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			
			quitGameBtn.onClick.RemoveAllListeners();
			attackBtn1.onClick.RemoveAllListeners();
			attackBtn2.onClick.RemoveAllListeners();
			retreatBtn.onClick.RemoveAllListeners();
		}

		private void OnClickAttack1()
		{
			GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Attack, joyStick.Direction));
		}
		
		private void OnClickAttack2()
		{
			GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Skill, joyStick.Direction));
		}
		
		private void OnClickRetreat()
		{
			GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Retreat, joyStick.Direction));
		}

		private static void OnClickQuitGame()
		{
			// GameMain.Procedure.ChangeProcedure<ProcedureChangeScene>(SceneType.Normal, Constant.SceneId.MainLobby);
			if (!GameMain.GamePlay.IsPause)
			{
				GameMain.GamePlay.PauseGame();
			}
			else
			{
				GameMain.GamePlay.ResumeGame();
			}
		}
	}
}
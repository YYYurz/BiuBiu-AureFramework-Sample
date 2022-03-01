//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.Procedure;
using AureFramework.Scene;
using AureFramework.UI;
using UnityEngine;

namespace BiuBiu
{
	public enum SceneType
	{
		None = 0,
		Normal = 1,
		Battle = 2,
	}	
	
	public class ProcedureChangeScene : ProcedureBase
	{
		private SceneType curSceneType = SceneType.None;
		private string lastSceneAssetName;
		private string curSceneAssetName;
		private uint curGameId;
		private uint curSceneId;
		private uint curSceneWindowId;
		private bool isChangeSceneComplete;

		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);

			GameMain.Event.Subscribe<OpenUISuccessEventArgs>(OnOpenUISuccess);
			GameMain.Event.Subscribe<LoadSceneUpdateEventArgs>(OnLoadSceneUpdate);
			GameMain.Event.Subscribe<LoadSceneSuccessEventArgs>(OnLoadSceneSuccess);
			GameMain.Event.Subscribe<UnloadSceneSuccessEventArgs>(OnUnloadSceneSuccess);

			if (curSceneType == SceneType.Battle)
			{
				GameMain.GamePlay.QuitCurrentGame();
			}
			
			isChangeSceneComplete = false;
			curSceneType = (SceneType) args[0];
			switch (curSceneType)
			{
				case SceneType.None:
				{
					break;
				}
				case SceneType.Normal:
				{
					curGameId = 0;
					curSceneId = (uint) args[1];
					break;
				}
				case SceneType.Battle:
				{
					curGameId = (uint) args[1];
					curSceneId = GameMain.DataTable.GetDataTableReader<GamePlayTableReader>().GetInfo(curGameId).SceneId;
					break;
				}
			}
			
			// 获取新场景信息
			var sceneData = GameMain.DataTable.GetDataTableReader<SceneTableReader>().GetInfo(curSceneId);
			lastSceneAssetName = curSceneAssetName;
			curSceneWindowId = sceneData.SceneWindowId;
			curSceneAssetName = sceneData.AssetName;

			// 1.关闭所有UI，打开Loading界面
			GameMain.UI.CloseAllUI();
			GameMain.UI.OpenUI(Constant.UIFormId.LoadingWindow);
		}

		public override void OnUpdate(float elapseTime, float realElapseTime)
		{
			base.OnUpdate(elapseTime, realElapseTime);

			if (!isChangeSceneComplete)
			{
				return;
			}
			
			switch (curSceneId)
			{
				case Constant.SceneId.MainLobby:
				{
					ChangeState<ProcedureLobby>();
					break;
				}
				case Constant.SceneId.BattleField:
				{
					ChangeState<ProcedureBattle>();
					break;
				}
			}
		}

		public override void OnExit()
		{
			base.OnExit();

			GameMain.UI.CloseUI(Constant.UIFormId.LoadingWindow);
			GameMain.Event.Unsubscribe<OpenUISuccessEventArgs>(OnOpenUISuccess);
			GameMain.Event.Unsubscribe<LoadSceneUpdateEventArgs>(OnLoadSceneUpdate);
			GameMain.Event.Unsubscribe<LoadSceneSuccessEventArgs>(OnLoadSceneSuccess);
			GameMain.Event.Unsubscribe<UnloadSceneSuccessEventArgs>(OnUnloadSceneSuccess);
		}

		private static void OnLoadSceneUpdate(object sender, AureEventArgs e)
		{
			var args = (LoadSceneUpdateEventArgs) e;

			Debug.Log($"Loading Scene, SceneName :{args.SceneName}, Progress :{args.Progress}");
		}

		private void OnLoadSceneSuccess(object sender, AureEventArgs e)
		{
			// 3.新场景加载完成，开始卸载旧场景
			if (!string.IsNullOrEmpty(lastSceneAssetName))
			{
				GameMain.Scene.UnloadScene(lastSceneAssetName);
			}
			else
			{
				OnUnloadSceneSuccess(this, null);
			}
		}

		private void OnUnloadSceneSuccess(object sender, AureEventArgs e)
		{
			// 4.旧场景卸载完成，打开新场景主界面
			GameMain.UI.OpenUI(curSceneWindowId);
		}

		private void OnOpenUISuccess(object sender, AureEventArgs e)
		{
			if (GameMain.UI.IsUIOpen(curSceneWindowId))
			{
				// 5.新场景主界面已经打开，切换场景完成
				if (curSceneType == SceneType.Battle)
				{
					// 进入的是游戏场景，需要创建游戏
					GameMain.GamePlay.CreateGame(curGameId, () =>
					{
						isChangeSceneComplete = true;
						GameMain.GamePlay.StartGame();
					});
				}
				else
				{
					isChangeSceneComplete = true;
				}
			}
			else if (GameMain.UI.IsUIOpen(Constant.UIFormId.LoadingWindow))
			{
				// 2.Loading界面已经打开，开始加载新场景
				GameMain.Scene.LoadScene(curSceneAssetName);
			}
		}
	}
}
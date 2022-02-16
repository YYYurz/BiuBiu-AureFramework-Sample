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
	public class ProcedureChangeScene : ProcedureBase
	{
		private string lastSceneAssetName;
		private string curSceneAssetName;
		private uint curSceneId;
		private uint curSceneWindowId;
		private bool isChangeSceneComplete;

		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);

			GameMain.Event.Subscribe<OpenUISuccessEventArgs>(OnOpenUISuccessCallback);
			GameMain.Event.Subscribe<LoadSceneUpdateEventArgs>(OnLoadSceneUpdateCallback);
			GameMain.Event.Subscribe<LoadSceneSuccessEventArgs>(OnLoadSceneSuccessCallback);
			GameMain.Event.Subscribe<UnloadSceneSuccessEventArgs>(OnUnloadSceneSuccessCallback);

			isChangeSceneComplete = false;
			curSceneId = (uint) args[0];
			
			// 获取新场景信息
			var sceneData = GameMain.DataTable.GetDataTableReader<SceneTableReader>().GetInfo(curSceneId);
			lastSceneAssetName = curSceneAssetName;
			curSceneWindowId = sceneData.SceneWindowId;
			curSceneAssetName = sceneData.AssetName;

			// 1.关闭所有UI，打开Loading界面
			GameMain.UI.CloseAllUIExcept(Constant.UIFormId.LoadingWindow);
			GameMain.UI.OpenUI(Constant.UIFormId.LoadingWindow);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (!isChangeSceneComplete)
			{
				return;
			}
			
			switch (curSceneId)
			{
				case 1:
				{
					ChangeState<ProcedureLobby>();
					break;
				}
				case 2:
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
			GameMain.Event.Unsubscribe<OpenUISuccessEventArgs>(OnOpenUISuccessCallback);
			GameMain.Event.Unsubscribe<LoadSceneUpdateEventArgs>(OnLoadSceneUpdateCallback);
			GameMain.Event.Unsubscribe<LoadSceneSuccessEventArgs>(OnLoadSceneSuccessCallback);
			GameMain.Event.Unsubscribe<UnloadSceneSuccessEventArgs>(OnUnloadSceneSuccessCallback);
		}

		private static void OnLoadSceneUpdateCallback(object sender, AureEventArgs e)
		{
			var args = (LoadSceneUpdateEventArgs) e;

			Debug.Log($"Loading Scene, SceneName :{args.SceneName}, Progress :{args.Progress}");
		}

		private void OnLoadSceneSuccessCallback(object sender, AureEventArgs e)
		{
			// 3.新场景加载完成，开始卸载旧场景
			if (!string.IsNullOrEmpty(lastSceneAssetName))
			{
				GameMain.Scene.UnloadScene(lastSceneAssetName);
			}
			else
			{
				OnUnloadSceneSuccessCallback(this, null);
			}
		}

		private void OnUnloadSceneSuccessCallback(object sender, AureEventArgs e)
		{
			// 4.旧场景卸载完成，打开新场景主界面
			GameMain.UI.OpenUI(curSceneWindowId);
		}

		private void OnOpenUISuccessCallback(object sender, AureEventArgs e)
		{
			if (GameMain.UI.IsUIOpen(curSceneWindowId))
			{
				// 5.新场景主界面已经打开，切换场景完成
				isChangeSceneComplete = true;
			}
			else if (GameMain.UI.IsUIOpen(Constant.UIFormId.LoadingWindow))
			{
				// 2.Loading界面已经打开，开始加载新场景
				GameMain.Scene.LoadScene(curSceneAssetName);
			}
		}
	}
}
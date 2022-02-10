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
		private string curSceneAssetName;
		private uint curSceneId;
		private uint curSceneWindowId;
		private bool isUnloadSceneComplete;
		private bool isLoadSceneComplete;

		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);

			GameMain.Event.Subscribe<LoadSceneUpdateEventArgs>(OnLoadSceneUpdateCallback);
			GameMain.Event.Subscribe<LoadSceneSuccessEventArgs>(OnLoadSceneSuccessCallback);
			GameMain.Event.Subscribe<UnloadSceneSuccessEventArgs>(OnUnloadSceneSuccessCallback);

			// 卸载上一个场景
			if (!string.IsNullOrEmpty(curSceneAssetName))
			{
				GameMain.Scene.UnloadScene(curSceneAssetName);
				isUnloadSceneComplete = false;
			}
			else
			{
				isUnloadSceneComplete = true;
			}
			
			isLoadSceneComplete = false;
			curSceneId = (uint) args[0];
			
			// 加载新场景
			var sceneData = GameMain.DataTable.GetDataTableReader<SceneTableReader>().GetInfo(curSceneId);
			curSceneWindowId = sceneData.SceneWindowId;
			curSceneAssetName = sceneData.AssetName;
			
			GameMain.UI.CloseAllUI();
			GameMain.UI.OpenUI(Constant.UIFormId.LoadingWindow);
			GameMain.Scene.LoadScene(curSceneAssetName);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (!isLoadSceneComplete || !isUnloadSceneComplete || !GameMain.UI.IsUIOpen(curSceneWindowId))
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
			var args = (LoadSceneSuccessEventArgs) e;

			if (args.SceneName.Equals(curSceneAssetName))
			{
				isLoadSceneComplete = true;
				GameMain.UI.OpenUI(curSceneWindowId);
			}
		}

		private void OnUnloadSceneSuccessCallback(object sender, AureEventArgs e)
		{
			isUnloadSceneComplete = true;
		}
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using AureFramework.Resource;
using AureFramework.Sound;
using UnityEngine;
using UnityEngine.Video;

namespace BiuBiu
{
	public class ProcedureLobby : ProcedureBase
	{
		private byte[] bytes;
		private float timeRecorder = 0f;

		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);

			// GameMain.UI.OpenUI(Constant.UIFormID.SoundWindow);
			
			var loadAssetCallBacks = new LoadAssetCallbacks(OnLoadAssetBegin, OnLoadAssetSuccess, null, OnLoadAssetFailed);
			// GameMain.Resource.LoadAssetAsync<VideoClip>("VideoTest", loadAssetCallBacks, null);
			// GameMain.Resource.LoadAssetAsync<VideoClip>("VideoTest2", loadAssetCallBacks, null);
			// GameMain.Resource.LoadAssetAsync<VideoClip>("VideoTest3", loadAssetCallBacks, null);
			// GameMain.Resource.LoadAssetAsync<AudioClip>("TestSound1", loadAssetCallBacks, null);
			// GameMain.Resource.LoadAssetAsync<AudioClip>("TestSound1", loadAssetCallBacks, null);
			// GameMain.Resource.LoadAssetAsync<GameObject>("House", loadAssetCallBacks, null);
			// GameMain.Resource.LoadAssetAsync<GameObject>("House", loadAssetCallBacks, null);
			// var shop1 = GameMain.Resource.InstantiateSync("Shop");
			// var shop2 = GameMain.Resource.InstantiateSync("Shop");
			// var shop2 = GameMain.Resource.LoadAssetSync<GameObject>("Shop");
			// var shop3 = Object.Instantiate(shop2);
		}

		private float i;
		private bool b = false;
		public override void OnUpdate()
		{
			base.OnUpdate();

			i += Time.deltaTime;
			if (i > 3 && !b)
			{
				b = true;
				GameMain.UI.OpenUI(Constant.UIFormID.SoundWindow);
			}
		}

		private void OnLoadAssetBegin(string assetName, int taskId)
		{
			Debug.LogError("Begin  " + taskId);
			// GameMain.Resource.ReleaseTask(taskId);
		}
		
		private void OnLoadAssetSuccess(string assetName, int taskId, Object asset, object userData)
		{
			Debug.LogError("OnLoadAssetSuccess");
		}

		private void OnLoadAssetFailed(string assetName, int taskId, string errorMessage, object userData)
		{
			Debug.LogError(errorMessage);
		}
	}
}
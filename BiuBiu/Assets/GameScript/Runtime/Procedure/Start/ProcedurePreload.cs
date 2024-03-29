﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using AureFramework.Procedure;
using AureFramework.Resource;

namespace BiuBiu
{
	public class ProcedurePreload : ProcedureBase
	{
		private static readonly LoadAssetCallbacks LoadAssetCallBacks = new LoadAssetCallbacks(null, OnLoadAssetSuccess, null, OnLoadAssetFailed);
		private static readonly List<int> LoadingAssetList = new List<int>();
		private static bool allAssetLoadedComplete;

		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);

			LoadingAssetList.Clear();
			allAssetLoadedComplete = false;

			StartPreload(args);
		}

		public override void OnUpdate(float elapseTime, float realElapseTime)
		{
			base.OnUpdate(elapseTime, realElapseTime);

			// if (!allAssetLoadedComplete)
			// {
			//     return;
			// }

			ChangeState<ProcedureChangeScene>(SceneType.Normal, Constant.SceneId.MainLobby);
		}

		public override void OnExit()
		{
			base.OnExit();
			
			GameMain.Lua.DoString("require('GameMain')");
		}

		/// <summary>
		/// 开始预加载资源
		/// </summary>
		private void StartPreload(object[] args)
		{
		}

		private static void OnLoadAssetBegin(string assetName, int taskId)
		{
			LoadingAssetList.Add(taskId);
		}

		private static void OnLoadAssetSuccess(string assetName, int taskId, Object asset, object userData)
		{
			LoadingAssetList.Remove(taskId);
			if (LoadingAssetList.Count == 0)
			{
				allAssetLoadedComplete = true;
			}
		}

		private static void OnLoadAssetFailed(string assetName, int taskId, string errorMessage, object userData)
		{
			Debug.LogError($"ProcedurePreload : Preload asset failed, asset name :{assetName}");
			LoadingAssetList.Remove(taskId);
		}
	}
}
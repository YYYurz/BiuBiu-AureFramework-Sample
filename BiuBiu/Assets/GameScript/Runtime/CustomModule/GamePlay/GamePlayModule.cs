//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework;
using AureFramework.Resource;
using AureFramework.Utility;
using GameConfig;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BiuBiu
{
	/// <summary>
	/// 游戏玩法模块 
	/// </summary>
	public sealed class GamePlayModule : AureFrameworkModule, IGamePlayModule
	{
		private readonly List<Object> cacheAssetList = new List<Object>();
		private PlayerController playerController;
		private MapConfig curMapConfig;
		private Camera mainCamera;

		private LoadAssetCallbacks loadAssetCallbacks;
		private Action createGameCompleteCallBack;

		private int preloadingAssetCount;
		private bool isStart;
		private bool isPause;
		
		/// <summary>
		/// 当前游戏寻路网格配置
		/// </summary>
		public MapConfig CurMapConfig
		{
			get
			{
				return curMapConfig;
			}
		}

		public PlayerController PlayerController
		{
			get
			{
				return playerController;
			}
		}

		public Camera MainCamera
		{
			get
			{
				return mainCamera;
			}
		}

		/// <summary>
		/// 游戏是否开始
		/// </summary>
		public bool IsStart
		{
			get
			{
				return isStart;
			}
		}

		/// <summary>
		/// 游戏是否暂停
		/// </summary>
		public bool IsPause
		{
			get
			{
				return isPause;
			}
		}

		public override void Init()
		{
			loadAssetCallbacks = new LoadAssetCallbacks(null, OnLoadAssetSuccess, null, OnLoadAssetFailed);
		}

		public override void Tick(float elapseTime, float realElapseTime)
		{
			
		}

		public override void Clear()
		{
			
		}

		public void StartGame()
		{
			isStart = true;
			isPause = false;
		}

		public void PauseGame()
		{
			if (!isStart)
			{
				return;
			}
			
			isPause = true;
		}

		public void ResumeGame()
		{
			if (!isStart)
			{
				return;
			}
			
			isPause = false;
		}
		
		public void CreateGame(uint gameId, Action callback)
		{
			createGameCompleteCallBack = callback;
			
			var gamePlayData = GameMain.DataTable.GetDataTableReader<GamePlayTableReader>().GetInfo(gameId);
			CreatePlayer(gamePlayData);
			CreateMapConfig(gamePlayData);
			PreloadAssets(gamePlayData);
		}

		public void QuitCurrentGame()
		{
			isStart = false;
			GameMain.Entity.DestroyAllCacheEntity();
			
			GameMain.Resource.ReleaseAsset(playerController.gameObject);
			playerController = null;
			foreach (var asset in cacheAssetList)
			{
				GameMain.Resource.ReleaseAsset(asset);
			}
			cacheAssetList.Clear();
			
			GameMain.Effect.ClearAllEffect();
		}

		private void PreloadAssets(GamePlay gamePlayData)
		{
			if (string.IsNullOrEmpty(gamePlayData.PreloadAssets))
			{
				createGameCompleteCallBack?.Invoke();
				return;
			}

			var preloadAssetArray = gamePlayData.PreloadAssets.Split('|');
			preloadingAssetCount = 0;
			foreach (var preloadAsset in preloadAssetArray)
			{
				GameMain.Resource.LoadAssetAsync<Object>(preloadAsset, loadAssetCallbacks, null);
				preloadingAssetCount++;
			}
		}

		private void CreatePlayer(GamePlay gamePlayData)
		{
			var playerGameObj = GameMain.Resource.InstantiateSync(gamePlayData.PlayerAsset);
			playerController = playerGameObj.GetComponent<PlayerController>();
			playerController.transform.position = Vector3.up;
		}

		private void CreateMapConfig(GamePlay gamePlayData)
		{
			curMapConfig = GameMain.Resource.LoadAssetSync<MapConfig>(gamePlayData.MapConfig);
		}

		private void OnLoadAssetSuccess(string assetName, int taskId, Object asset, object userData)
		{
			cacheAssetList.Add(asset);
			if (--preloadingAssetCount <= 0)
			{
				createGameCompleteCallBack?.Invoke();
			}
		}

		private void OnLoadAssetFailed(string assetName, int taskId, string errorMessage, object userData)
		{
			Debug.LogError($"GamePlayModule : Preload asset failed, error message : {errorMessage}");
			if (--preloadingAssetCount <= 0)
			{
				createGameCompleteCallBack?.Invoke();
			}
		}
	}
}
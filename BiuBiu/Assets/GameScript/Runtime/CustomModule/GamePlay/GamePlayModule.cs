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
using GameConfig;
using Unity.Entities;
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
			QuitCurrentGame();
		}

		/// <summary>
		/// 开始游戏
		/// </summary>
		public void StartGame()
		{
			isStart = true;
			isPause = false;
		}

		/// <summary>
		/// 暂停游戏
		/// </summary>
		public void PauseGame()
		{
			if (!isStart)
			{
				return;
			}
			
			isPause = true;
		}

		/// <summary>
		/// 恢复游戏
		/// </summary>
		public void ResumeGame()
		{
			if (!isStart)
			{
				return;
			}
			
			isPause = false;
		}
		
		/// <summary>
		/// 创建游戏
		/// </summary>
		/// <param name="gameId"> 游戏配置Id </param>
		/// <param name="callback"> 创建完成回调 </param>
		public void CreateGame(uint gameId, Action callback)
		{
			createGameCompleteCallBack = callback;
			
			var gamePlayData = GameMain.DataTable.GetDataTableReader<GamePlayTableReader>().GetInfo(gameId);
			LoadPlayer(gamePlayData);
			LoadMapConfig(gamePlayData);
			PreloadEntity(gamePlayData);
		}

		/// <summary>
		/// 推出当前游戏，清除数据
		/// </summary>
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
			World.DefaultGameObjectInjectionWorld.GetExistingSystem<CreateEntityFromAddressableSystem>().ClearAllEntity();
		}

		/// <summary>
		/// 预加载实体
		/// </summary>
		/// <param name="gamePlayData"></param>
		private void PreloadEntity(GamePlay gamePlayData)
		{
			if (gamePlayData.PreloadEntitiesLength == 0)
			{
				createGameCompleteCallBack?.Invoke();
				return;
			}

			var createEntityFromAddressableSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<CreateEntityFromAddressableSystem>();
			var preloadEntityList = gamePlayData.GetPreloadEntitiesArray();
			createEntityFromAddressableSystem.PreConvertEntityFromAddressable(preloadEntityList, createGameCompleteCallBack);
		}

		/// <summary>
		/// 加载主角
		/// </summary>
		/// <param name="gamePlayData"></param>
		private void LoadPlayer(GamePlay gamePlayData)
		{
			var playerGameObj = GameMain.Resource.InstantiateSync(gamePlayData.PlayerAsset);
			playerController = playerGameObj.GetComponent<PlayerController>();
			playerController.transform.position = Vector3.up;
		}

		/// <summary>
		/// 加载地图寻路网格配置
		/// </summary>
		/// <param name="gamePlayData"></param>
		private void LoadMapConfig(GamePlay gamePlayData)
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
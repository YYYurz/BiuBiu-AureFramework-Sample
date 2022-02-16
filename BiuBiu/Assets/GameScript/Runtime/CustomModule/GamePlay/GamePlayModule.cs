//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework;
using Unity.Entities;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 游戏玩法模块 
	/// </summary>
	public sealed class GamePlayModule : AureFrameworkModule, IGamePlayModule
	{
		private World entityWorld;
		private MapConfig curMapConfig;
		private Camera mainCamera;
		private uint sceneId;
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
			entityWorld = World.DefaultGameObjectInjectionWorld;
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
		
		public void CreateGame(uint gameId)
		{
			var gameData = GameMain.DataTable.GetDataTableReader<GamePlayTableReader>().GetInfo(gameId);
			sceneId = gameData.SceneId;
			
		}

		public void QuitCurrentGame()
		{
			isStart = false;
			GameMain.Entity.DestroyAllCacheEntity();
		}
	}
}
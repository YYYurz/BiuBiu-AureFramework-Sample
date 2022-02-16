﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;

namespace BiuBiu
{
	/// <summary>
	/// 游戏模块接口
	/// </summary>
	public interface IGamePlayModule
	{
		/// <summary>
		/// 当前游戏寻路网格配置
		/// </summary>
		MapConfig CurMapConfig
		{
			get;
		}

		bool IsStart
		{
			get;
		}

		bool IsPause
		{
			get;
		}
		
		
		/// <summary>
		/// 创建游戏
		/// </summary>
		/// <param name="gameId"> 游戏Id </param>
		/// <param name="preloadOverCallback"> 预加载资源完成回调 </param>
		// void CreateGame(int gameId, Action preloadOverCallback);
		//
		// void QuitCurrentGame();
	}
}
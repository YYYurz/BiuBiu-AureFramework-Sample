﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace BiuBiu {
	/// <summary>
	/// Lua调用C#静态类
	/// </summary>
	public static class LuaCallStatic {
		/// <summary>
		/// 强制切换切场景流程
		/// </summary>
		/// <param name="sceneName"></param>
		public static void ChangeScene(string sceneName) {
			GameMain.Procedure.ChangeProcedure<ProcedureChangeScene>(sceneName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="assetName"></param>
		/// <returns></returns>
		public static byte[] GetBytesFile(string assetName) {
			if (string.IsNullOrEmpty(assetName)) {
				Debug.LogError("LuaCallStatic : Asset name is null.");
				return null;
			}

			var textAsset = GameMain.Resource.LoadAssetSync<TextAsset>(assetName);
			return textAsset.bytes;
		}
	}
}
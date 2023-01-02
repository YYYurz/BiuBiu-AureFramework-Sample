//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.Resource;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BiuBiu
{
	public static partial class LuaHelper
	{
		/// <summary>
		/// 加载二进制资源
		/// </summary>
		/// <param name="filePath"> 资源路径 </param>
		/// <returns></returns>
		public static byte[] GetBytesFile(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				Debug.LogError("LuaHelper : File path is null.");
				return null;
			}

			return AssetUtils.LoadBytes(filePath);
		}
		
		public static void LoadAsset(string assetPath, Action<Object> assetAction)
		{
			var mAtlasCallBack = new LoadAssetCallbacks(null, LuaLoadAssetCallback.LoadSuccessCallback, null, LuaLoadAssetCallback.LoadFailureCallback);
			var userData = GameMain.ReferencePool.Acquire<LuaLoadAssetCallback>();
			userData.Fill(assetAction);
			GameMain.Resource.LoadAssetAsync<Object>(assetPath, mAtlasCallBack, userData);
		}

		public static void LoadDialogue(string filePath, Action<Object> callBack)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				Debug.LogError("LuaHelper : File path is null.");
				return;
			}

			LoadAsset(filePath, callBack);
		}
	}
}
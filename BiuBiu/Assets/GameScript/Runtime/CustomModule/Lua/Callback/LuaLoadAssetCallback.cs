//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.ReferencePool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DrunkFish
{
	public class LuaLoadAssetCallback : IReference
	{
		private Action<Object> action;
		
		public void Fill(Action<Object> luaCallback)
		{
			action = luaCallback;
		}

		private void Invoke(Object obj)
		{
			if (action != null)
			{
				action(obj);
				action = null;
			}
			else
			{
				Debug.LogError("Action is null, load Failure!");
			}
		}
		
		public void Clear()
		{
			action = null;
		}

		public static void LoadSuccessCallback(string assetName, int taskId, Object asset, object userData)
		{
			Debug.Log($"Asset LoadSuccess! assetName:{assetName}");
			if (userData is LuaLoadAssetCallback callBack)
			{
				callBack.Invoke(asset);
				GameMain.ReferencePool.Release(callBack);
			}
		}
		public static void LoadFailureCallback(string assetName, int taskId, string errorMessage, object userData)
		{
			Debug.Log($"Asset LoadFailure! assetName:{assetName}");
		}
	}
}
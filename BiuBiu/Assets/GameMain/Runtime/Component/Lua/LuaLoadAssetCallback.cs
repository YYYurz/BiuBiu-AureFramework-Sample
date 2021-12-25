using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using GameFramework;
using GameFramework.Resource;
using UnityEngine.U2D;
using System;

namespace Hr
{
    public class LuaLoadAssetCallback : IReference
    {
        private Action<Object> action;
        public void Fill(Action<Object> luaCallback)
        {
            action = luaCallback;
        }
        public void Invoke(Object obj)
        {
            if (action != null)
            {
                action(obj);
                action = null;
            }
            else
            {
                Log.Error("action is null,Load Failure!");
            }
        }
        public void Clear()
        {
            action = null;
        }

        public static void LoadSuccessCallback(string assetName, object asset, float duration, object userData)
        {
            Log.Info("Asset LoadSuccess! assetName:{0}", assetName);
            LuaLoadAssetCallback callBack = userData as LuaLoadAssetCallback;
            callBack.Invoke(asset);
            ReferencePool.Release(callBack);
        }
        public static void LoadFailureCallback(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Error("Asset LoadFailure! assetName:{0}", assetName);
        }
    }
}
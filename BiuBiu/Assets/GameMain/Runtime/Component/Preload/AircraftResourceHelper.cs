//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using AureFramework.Resource;
using System.Collections;
using UnityEngine;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;

namespace BiuBiu
{
    /// <summary>
    /// 默认资源辅助器。
    /// </summary>
    public class AircraftResourceHelper : DefaultResourceHelper
    {
        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="objectToRelease">要释放的资源。</param>
        public override void Release(object objectToRelease)
        {
            AssetBundle assetBundle = objectToRelease as AssetBundle;
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
                return;
            }

            //Unity 当前 Resources.UnloadAsset 在 iOS 设备上会导致一些诡异问题，先不用这部分
            //SceneAsset sceneAsset = objectToRelease as SceneAsset;
            //if (sceneAsset != null)
            //{
            //    return;
            //}

            Object unityObject = objectToRelease as Object;
            if (unityObject == null)
            {
                Log.Warning("Asset is invalid. type:{0}", objectToRelease.GetType().Name);
                return;
            }

            if (unityObject is GameObject || unityObject is MonoBehaviour)
            {
                // UnloadAsset may only be used on individual assets and can not be used on GameObject's / Components or AssetBundles.
                return;
            }

            Resources.UnloadAsset(unityObject);
      
        }
    }
}

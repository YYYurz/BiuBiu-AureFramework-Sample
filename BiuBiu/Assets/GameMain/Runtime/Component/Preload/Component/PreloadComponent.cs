using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BB
{
    [DisallowMultipleComponent]
    public class PreloadComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 资源加载完成的标志位
        /// </summary>
        private readonly Dictionary<string, PreloadAssetInfo> mDicLoadingAssetInfo = new Dictionary<string, PreloadAssetInfo>();

        private int loadedAssetCount;
        private int allNeedLoadAssetCount;
        
        /// <summary>
        /// 是否开启了释放资源
        /// </summary>
        private bool mStartUnloadResource;

        /// <summary>
        /// 释放资源进行到第几步了
        /// </summary>
        private int mUnloadResourceSteps;

        /// <summary>
        /// 缓存AssetExpireTime
        /// </summary>
        private float mCacheAssetExpireTime;

        /// <summary>
        /// 缓存ResourceExpireTime
        /// </summary>
        private float mCacheResourceExpireTime;

        private void Update()
        {
            if (mStartUnloadResource)
            {
                if (mUnloadResourceSteps == 0)
                {
                    mCacheAssetExpireTime = GameEntry.Resource.AssetExpireTime;
                    GameEntry.Resource.AssetExpireTime = 0.0001f;
                    GameEntry.Resource.AssetExpireTime = mCacheAssetExpireTime;
                }
                else if (mUnloadResourceSteps == 1)
                {
                    mCacheResourceExpireTime = GameEntry.Resource.ResourceExpireTime;
                    GameEntry.Resource.ResourceExpireTime = 0.0001f;
                    GameEntry.Resource.ResourceExpireTime = mCacheResourceExpireTime;
                    mStartUnloadResource = false;

                    Log.Info("UnloadResource Finished! AssetCount:{0} ResourceCount:{1}", GameEntry.Resource.AssetCount, GameEntry.Resource.ResourceCount);
                    GameEntry.Event.Fire(this, UnloadResourceFinishEventArgs.Create());
                }
                ++mUnloadResourceSteps;
            }
        }

        public void AddAssetPreloadList(PreloadAssetList preloadList)
        {
            var lisAssetInfo = preloadList.GetAssetInfoList();
            foreach (var iteAssetInfo in lisAssetInfo)
            {
                mDicLoadingAssetInfo[iteAssetInfo.AssetPath] = iteAssetInfo;
            }
        }

        public int GetTotalPreloadAssetCount()
        {
            return mDicLoadingAssetInfo.Count;
        }

        private void ResetAssetPreloadInfo()
        {
            mDicLoadingAssetInfo.Clear();
            loadedAssetCount = 0;
            allNeedLoadAssetCount = 0;
        }

        public void StartPreloadAsset()
        {
            AddEvent();
            allNeedLoadAssetCount = mDicLoadingAssetInfo.Count;
            GameEntry.Event.Fire(this, PreloadProgressLoadingEventArgs.Create(0, allNeedLoadAssetCount));

            foreach (var iteAssetInfo in mDicLoadingAssetInfo)
            {
                switch (iteAssetInfo.Value.AssetPreloadType)
                {
                    case GameEnum.GAME_ASSET_TYPE.LuaFile:
                        GameEntry.Lua.LoadLuaFile((string)iteAssetInfo.Value.UserData, iteAssetInfo.Value.AssetPath);
                        break;
                    case GameEnum.GAME_ASSET_TYPE.Scriptable:
                    case GameEnum.GAME_ASSET_TYPE.DataTable:
                        GameEntry.TableData.LoadCustomData(iteAssetInfo.Value.AssetPath, iteAssetInfo.Value.UserData);
                        break;
                    // case GameEnum.GAMEASSET_TYPE.PAT_CONFIGTXT:
                    //     {
                    //         Tuple<string, LoadType> tupUserData = iteAssetInfo.Value.UserData as Tuple<string, LoadType>;
                    //         GameEntry.Config.LoadConfig(tupUserData.Item1, iteAssetInfo.Value.AssetPath, tupUserData.Item2);
                    //     }
                    //     break;
                    // case GameEnum.GAMEASSET_TYPE.PAT_LOCALIZATION:
                    //     {
                    //         Tuple<string, LoadType> tupUserData = iteAssetInfo.Value.UserData as Tuple<string, LoadType>;
                    //         GameEntry.Localization.LoadDictionary(tupUserData.Item1, iteAssetInfo.Value.AssetPath, tupUserData.Item2);
                    //     }
                    //     break;
                    // case GameEnum.GAMEASSET_TYPE.PAT_PREFAB:
                    //     {
                    //         LoadAssetCallbacks callbacks = new LoadAssetCallbacks(OnPreLoadPrefabSuccess, OnPreLoadPrefabFailed);
                    //         GameEntry.Resource.LoadAsset(iteAssetInfo.Value.AssetPath, callbacks);
                    //     }
                        // break;
                }
            }
        }

        private void AddEvent()
        {
            GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Subscribe(LoadLuaSuccessEventArgs.EventId, OnLoadLuaSuccess);
            GameEntry.Event.Subscribe(LoadCustomDataSuccessEventArgs.EventId, OnLoadCustomDataSuccess);

        }

        private void RemoveEvent()
        {
            GameEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Unsubscribe(LoadLuaSuccessEventArgs.EventId, OnLoadLuaSuccess);
            GameEntry.Event.Unsubscribe(LoadCustomDataSuccessEventArgs.EventId, OnLoadCustomDataSuccess);
        }

        private void OneAssetLoadSuccess(string strAssetName)
        {
            ++loadedAssetCount;
            mDicLoadingAssetInfo.Remove(strAssetName);
            OnLoadAssetProgress();

            if (CheckAllAssetsLoaded())
            {
                OnLoadAssetComplete();
            }
        }

        private bool CheckNormalAssetLoaded(string strAssetName)
        {
            if (mDicLoadingAssetInfo.TryGetValue(strAssetName, out var preAssetInfo))
            {
                preAssetInfo.AssetPreloadStatus = GameEnum.PRELOAD_ASSET_STATUS.Loaded;
            }
            else
            {
                Log.Error("CheckNormalAssetLoaded error! can't find the asset:{0}", strAssetName);
                return false;
            }

            return true;
        }

        private void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {
            var args = (LoadConfigSuccessEventArgs)e;
            //Log.Debug("PreloadComponent Load config '{0}' OK.", args.ConfigName);
            if (CheckNormalAssetLoaded(args.ConfigAssetName))
            {
                OneAssetLoadSuccess(args.ConfigAssetName);
            }
        }

        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            var args = (LoadDictionarySuccessEventArgs)e;
            //Log.Debug("PreloadComponent Load Dictionary '{0}' OK.", args.ConfigName);
            if (CheckNormalAssetLoaded(args.DictionaryAssetName))
            {
                OneAssetLoadSuccess(args.DictionaryAssetName);
            }
        }

        private void OnLoadLuaSuccess(object sender, GameEventArgs e)
        {
            var args = (LoadLuaSuccessEventArgs)e;
            if (CheckNormalAssetLoaded(args.AssetName))
            {
                OneAssetLoadSuccess(args.AssetName);
            }
        }

        private void OnLoadCustomDataSuccess(object sender, GameEventArgs e)
        {
            if (!(e is LoadCustomDataSuccessEventArgs args))
            {
                return;
            }
            if (CheckNormalAssetLoaded(args.DataName))
            {
                OneAssetLoadSuccess(args.DataName);
            }
        }
        
        private void OnLoadAssetComplete()
        {
            Log.Debug("PreloadComponent LoadAssetProgress load asset complete!");
            GameEntry.Event.Fire(this, PreloadProgressCompleteEventArgs.Create());
            GameEntry.Event.Fire(this, PreloadProgressLoadingEventArgs.Create(mDicLoadingAssetInfo.Count, allNeedLoadAssetCount));
            RemoveEvent();
            ResetAssetPreloadInfo();
        }

        private void OnLoadAssetProgress()
        {
            GameEntry.Event.Fire(this, PreloadProgressLoadingEventArgs.Create(loadedAssetCount, allNeedLoadAssetCount));
        }

        private bool CheckAllAssetsLoaded()
        {
            IEnumerator<PreloadAssetInfo> iter = mDicLoadingAssetInfo.Values.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current != null && iter.Current.AssetPreloadStatus != GameEnum.PRELOAD_ASSET_STATUS.Loaded)
                {
                    return false;
                }
            }
            iter.Dispose();
            return true;
        }
    }
}
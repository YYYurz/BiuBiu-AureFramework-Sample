using FlatBuffers;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BB
{
    [DisallowMultipleComponent]
    public class TableDataComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 数据表
        /// </summary>
        public DataTableAssets DataTableInfo { get; private set; } = new DataTableAssets();

        private LoadAssetCallbacks loadAssetCallbacks;

        private void Start()
        {
            loadAssetCallbacks = new LoadAssetCallbacks(OnLoadDataFileSuccess, OnLoadDataFileFailure);
        }
        
        #region 数据表资源加载

        public void LoadCustomData(string strAssetPath, object userData)
        {
            GameEntry.Resource.LoadAsset(strAssetPath, loadAssetCallbacks, userData);
        }
        
        private void OnLoadDataFileFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Error("GameDataComponent Load data file failed! name:{0} status:{1}", assetName, status);
        }

        private void OnLoadDataFileSuccess(string assetName, object asset, float duration, object userData)
        {
            if (!(userData is ParseConfigDataInfo parseConfigInfo))
            {
                Log.Error("GameDataComponent Load data file failed! name:{0} userData is invalid", assetName);
                return;
            }
            switch (parseConfigInfo.DataType)
            {
                case GameEnum.GAME_ASSET_TYPE.DataTable:
                    DataTableInfo.ParseDataTable(asset, parseConfigInfo);
                    break;
                default:
                    Log.Error("TableDataComponent : Unknown GameData datatype!");
                    break;
            }
            // 发送预加载成功事件
            GameEntry.Event.Fire(this, LoadCustomDataSuccessEventArgs.Create(assetName, duration, userData));

            ///卸载
            GameEntry.Resource.UnloadAsset(asset);
        }

        #endregion
    }
}
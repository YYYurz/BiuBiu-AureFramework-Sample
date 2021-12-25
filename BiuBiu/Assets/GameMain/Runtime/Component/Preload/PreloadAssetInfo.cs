using GameFramework;
using System;

namespace BB
{
    public class PreloadAssetInfo
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetPath { get; set; }

        public int SerialID { get; set; }

        public int UIFormID { get; set; }

        /// <summary>
        /// 资源加载类型
        /// </summary>
        public GameEnum.GAME_ASSET_TYPE AssetPreloadType { get; set; } = GameEnum.GAME_ASSET_TYPE.Normal;

        /// <summary>
        /// 当前资源状态
        /// </summary>
        public GameEnum.PRELOAD_ASSET_STATUS AssetPreloadStatus { get; set; } = GameEnum.PRELOAD_ASSET_STATUS.UnStart;

        /// <summary>
        /// 资源类型
        /// </summary>
        public Type AssetType { get; set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public object UserData { get; set; } = null;

        public PreloadAssetInfo(string strAssetPath, int nSerialID, GameEnum.GAME_ASSET_TYPE preloadType, Type assetType, object userData)
        {
            AssetPath = strAssetPath;
            SerialID = nSerialID;
            AssetPreloadType = preloadType;
            AssetPreloadStatus = GameEnum.PRELOAD_ASSET_STATUS.UnStart;
            AssetType = assetType;
            UserData = userData;
        }
    }
}
namespace BiuBiu
{
    public static partial class Constant
    {
        public static class AssetPath
        {
            #region Update

            /// <summary>
            /// Apk的资源目录
            /// </summary>
            public const string ApkAssetsPath = "ApkAssets";
            /// <summary>
            /// Apk切片文件说明文件
            /// </summary>
            public const string ApkAssetsFile = "apk_assets_info.json";

            public const string ApkSliceFileExtension = ".apktemp";

            #endregion

            #region SCENE
            public const string DebugPrestartScene = "DevAssets/Scenes/d_PreStart";
            public const string GameAppScene = "GameAssets/Scenes/0_GameApp";
            public const string CheckVersionScene = "GameAssets/Scenes/1_GameCheckVersion";

            public const string UIFormFolder = "Assets/Art/Art2D/UI/{0}.prefab";
            public static string SoundFolder = "Assets/GameAssets/ModelResources/{0}.mp3";

            /// <summary>
            /// 用框架的加载scene逻辑的话，必须加上Assets，后缀加上unity
            /// </summary>
            public static string LoginScene = "Assets/GameAssets/Scenes/2_GameLogin.unity";
            public static string LobbyMainScene = "Assets/GameAssets/Scenes/3_Lobby.unity";
            public static string BattleFieldScene = "Assets/GameAssets/Scenes/4_BattleField.unity";
            public static string MoneyBattleFieldScene = "Assets/GameAssets/Scenes/5_MoneyBattleField.unity";
            #endregion

            #region LAUNCH
            /// <summary>
            /// 启动时需要加载的资源 脱离了框架的加载 AssetBundle
            /// </summary>
            public const string LaunchAsset = "launchasset.dat";
            public const string LaunchConfig = "Assets/GameAssets/Configs/AB/LaunchConfig.txt";
            #endregion

            /// <summary>
            /// 框架中配置文件
            /// </summary>
            public const string ServerConfigFile = "ServerConfig";
            public const string BuildConfigFile = "BuildConfig";
            public const string GameConfigFile = "GameConfig";
            public const string PlatformConfigFile = "PlatformConfig";
            public const string DefaultLocalizationFile = "Default";
        }
    }
}
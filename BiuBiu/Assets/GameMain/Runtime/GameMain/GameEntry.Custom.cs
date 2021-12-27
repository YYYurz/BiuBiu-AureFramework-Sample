using AureFramework;
using UnityEngine;

namespace BiuBiu
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameMain : MonoBehaviour
    {
        // public static TableDataComponent TableData
        // {
        //     get;
        //     private set;
        // }
        //
        // public static GameDataComponent GameData
        // {
        //     get;
        //     private set;
        // }
        //
        public static ILuaModule Lua
        {
            get;
            private set;
        }
        //
        // public static PreloadComponent AssetPreload
        // {
        //     get;
        //     private set;
        // }
        //
        // public static InputComponent InputComponent
        // {
        //     get;
        //     private set;
        // }

        private static void InitCustomModules()
        {
            // TableData = UnityGameFramework.Runtime.GameEntry.GetComponent<TableDataComponent>();
            // GameData = UnityGameFramework.Runtime.GameEntry.GetComponent<GameDataComponent>();
            Lua = Aure.GetModule<ILuaModule>();
        }
    }
}

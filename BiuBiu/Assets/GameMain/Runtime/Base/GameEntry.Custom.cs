using UnityEngine;

namespace BB
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        /// <summary>
        /// 数据表
        /// </summary>
        public static TableDataComponent TableData
        {
            get;
            private set;
        }
        
        public static GameDataComponent GameData
        {
            get;
            private set;
        }

        /// <summary>
        /// Lua组件
        /// </summary>
        public static LuaComponent Lua
        {
            get;
            private set;
        }

        public static PreloadComponent AssetPreload
        {
            get;
            private set;
        }

        public static InputComponent InputComponent
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 定时器
        /// </summary>
        // public static TimerComponent Timer
        // {
        //     get;
        //     private set;
        // }
        
        /// <summary>
        /// 游戏公共逻辑
        /// </summary>
        // public static GameLogicComponent GameLogic
        // {
        //     get;
        //     private set;
        // }    

        private static void InitCustomComponents()
        {
            TableData = UnityGameFramework.Runtime.GameEntry.GetComponent<TableDataComponent>();
            GameData = UnityGameFramework.Runtime.GameEntry.GetComponent<GameDataComponent>();
            Lua = UnityGameFramework.Runtime.GameEntry.GetComponent<LuaComponent>();
            AssetPreload = UnityGameFramework.Runtime.GameEntry.GetComponent<PreloadComponent>();
            InputComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<InputComponent>();
            // Timer = UnityGameFramework.Runtime.GameEntry.GetComponent<TimerComponent>();
            // GameLogic = UnityGameFramework.Runtime.GameEntry.GetComponent<GameLogicComponent>();
        }
    }
}

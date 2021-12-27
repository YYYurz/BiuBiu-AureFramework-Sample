namespace BiuBiu
{
    public class GameEnum
    {
        /// <summary>
        /// DataComponent数据类型
        /// </summary>
        public enum GAME_ASSET_TYPE
        {
            Normal,
            LuaFile,
            Scriptable,
            DataTable,
            Localization,
            UIForm,
            Prefab
        }    
        
        public enum PRELOAD_ASSET_STATUS
        {
            UnStart,
            Loading,
            Loaded,
        }

        public enum SCENE_TYPE
        {
            MainLobby = 1,
            HomeLand = 2,
        }

        public enum INPUT_TYPE
        {
            Move,
            Attack_1,
            Attack_2,
            Skill_1,
            SKill_2,
            Dance,
        }
    }
}


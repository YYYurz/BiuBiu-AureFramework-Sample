using System;

namespace BB
{
    public class ParseConfigDataInfo
    {
        public string ConfigName { get; private set; }

        public Type ConfigClassType { get; }

        public object UserData { get; private set; }

        public GameEnum.GAME_ASSET_TYPE DataType { get; private set; }

        public ParseConfigDataInfo(string configName, Type type, object userData, GameEnum.GAME_ASSET_TYPE dataType)
        {
            ConfigName = configName;
            ConfigClassType = type;
            UserData = userData;
            DataType = dataType;
        }

        public void Clear()
        {
            ConfigName = null;
            UserData = null;
            // DataType = GameEnum.GAMEASSET_TYPE.PAT_NORMAL;
        }
    }
}

using UnityGameFramework.Runtime;

namespace BiuBiu
{
    public class GameDataComponent : GameFrameworkComponent
    {
        public PlayerDataModel PlayerData { get; } = new PlayerDataModel();

        public GameDataModel GameData { get; }  = new GameDataModel();
    }    
}


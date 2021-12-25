 

using GameFramework.Event;

/// <summary>
/// 加载Lua失败事件
/// </summary>
public sealed class LoadLuaFailureEventArgs:GameEventArgs
{
    public static int EventId = typeof(LoadLuaFailureEventArgs).GetHashCode();

    public override int Id
    {
        get { return EventId; }
    }

    public string AssetName
    {
        private set;
        get;
    }

    public string LuaName
    {
        private set;
        get;
    }

    public string ErrorMessage
    {
        private set;
        get;
    }

    public override void Clear()
    {

    }

    public LoadLuaFailureEventArgs Fill(string assetName,string luaName,string errorMessage)
    {
        AssetName = assetName;
        LuaName = luaName;
        ErrorMessage = errorMessage;

        return this;
    }
}
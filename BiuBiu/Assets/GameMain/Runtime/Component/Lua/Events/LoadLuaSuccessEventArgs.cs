 

using AureFramework.Event;

/// <summary>
/// 加载Lua成功事件
/// </summary>
public sealed class LoadLuaSuccessEventArgs : GameEventArgs
{
    public static int EventId = typeof(LoadLuaSuccessEventArgs).GetHashCode();

    public override int Id
    {
        get
        {
            return EventId;
        }
    }

    /// <summary>
    /// 资源名
    /// </summary>
    public string AssetName
    {
        private set;
        get;
    }

    /// <summary>
    /// Lua名
    /// </summary>
    public string LuaName
    {
        private set;
        get;
    }

    /// <summary>
    /// 解析出来的Lua字符串
    /// </summary>
    public string LuaString
    {
        private set;
        get;
    }

    public float Duration
    {
        private set;
        get;
    }

    /// <summary>
    /// 清理加载Lua成功事件。
    /// </summary>
    public override void Clear()
    {
        AssetName = default(string);
        LuaString = default(string);
        LuaName = default(string);
        Duration = 0;
    }

    public LoadLuaSuccessEventArgs Fill(string assetName,string luaName,string luaString, float fDuration)
    {
        AssetName = assetName;
        LuaName = luaName;
        LuaString = luaString;
        Duration = fDuration;

        return this;
    }
}
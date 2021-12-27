using AureFramework.Event;

/// <summary>
/// 加载lua文件列表成功事件
/// </summary>
public sealed class LoadLuaFilesConfigSuccessEventArgs : GameEventArgs
{
    public static int EventId = typeof(LoadLuaFilesConfigSuccessEventArgs).GetHashCode();

    public override int Id
    {
        get { return EventId; }
    }

    public string AssetName
    {
        private set;
        get;
    }

    public string Content
    {
        private set;
        get;
    }

    public override void Clear()
    {
    }

    /// <summary>
    /// 填充事件参数
    /// </summary>
    public LoadLuaFilesConfigSuccessEventArgs Fill(string assetName, string content)
    {
        this.AssetName = assetName;
        this.Content = content;

        return this;
    }
}

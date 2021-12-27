 

using AureFramework.Event;
using System;
using XLua;
using System.Collections.Generic;

/// <summary>
/// Lua发送的过来的事件（Lua通用）
/// </summary>
public class LuaCSharpEventArgs : GameEventArgs
{
    public override int Id
    {
        get { return EventId; }
    }

    public int EventId
    {
        private set;
        get;
    }

    public string Sender
    {
        private set;
        get;
    }

    public int Param1 { get; set; } = 0;
    public int Param2 { get; set; } = 0;
    public int Param3 { get; set; } = 0;

    public object[] Param
    {
        private set;
        get;
    }

    public override void Clear()
    {
        EventId = default(int);
    }

    public LuaCSharpEventArgs Fill(int eventId, string sender, int nParam1 = 0, int nParam2 = 0, int nParam3 = 0)
    {
        this.Sender = sender;
        this.EventId = eventId;
        this.Param1 = nParam1;
        this.Param2 = nParam2;
        this.Param3 = nParam3;

        return this;
    }

    /// <summary>
    /// 填充事件参数
    /// </summary>
    /// <param name="eventId">事件id</param>
    /// <param name="param">参数数组</param>
    public LuaCSharpEventArgs Fill(int eventId, string sender, object[] param)
    {
        this.Sender = sender;
        this.EventId = eventId;
        this.Param = param;

        return this;
    }
}

#if UNITY_EDITOR
public static class LuaCSharpEventArgsExporter
{
    [LuaCallCSharp]
    public static List<Type> LuaCSharpEventArgs = new List<Type>()
        {
            typeof(LuaCSharpEventArgs),
        };
}
#endif

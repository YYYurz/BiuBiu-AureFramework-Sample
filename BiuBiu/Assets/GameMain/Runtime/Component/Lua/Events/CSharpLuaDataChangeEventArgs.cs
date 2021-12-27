// using GameFramework;
// using AureFramework.Event;
// using Hr;
// using System;
// using System.Collections.Generic;
// using XLua;
//
// public class CSharpLuaDataChangeEventArgs : GameEventArgs
// {
//     /// <summary>
//     /// 事件编号。
//     /// </summary>
//     public static readonly int EventId = typeof(CSharpLuaDataChangeEventArgs).GetHashCode();
//
//     /// <summary>
//     /// 事件编号。
//     /// </summary>
//     public override int Id
//     {
//         get
//         {
//             return EventId;
//         }
//     }
//
//     public int DataType { get; set; } = (int)GameDataType.DATA_TYPE.DT_DEFAULT;
//
//     public int ValueType { get; set; } = (int)GameDataType.VALUE_TYPE.VT_INT;
//
//     public long SubDataType { get; set; } = 0;
//
//     public long IntValue { get; set; } = 0;
//
//     public float FloatValue { get; set; } = 0.0f;
//
//     public string StringValue { get; set; } = null;
//
//     public static CSharpLuaDataChangeEventArgs Create(GameDataChangeEventArgs e)
//     {
//         CSharpLuaDataChangeEventArgs dataChangeEventArgs = ReferencePool.Acquire<CSharpLuaDataChangeEventArgs>();
//         dataChangeEventArgs.DataType = (int)e.DataType;
//         dataChangeEventArgs.ValueType = (int)e.ValueType;
//         dataChangeEventArgs.SubDataType = e.SubDataType;
//         dataChangeEventArgs.IntValue = e.IntValue;
//         dataChangeEventArgs.FloatValue = e.FloatValue;
//         dataChangeEventArgs.StringValue = e.StringValue;
//
//         return dataChangeEventArgs;
//
//     }
//
//     public static CSharpLuaDataChangeEventArgs Create(GameDataType.DATA_TYPE dataType
//         , GameDataType.VALUE_TYPE valueType
//         , long intValue = 0
//         , float floatValue = 0.0f
//         , string strValue = null)
//     {
//         CSharpLuaDataChangeEventArgs dataChangeEventArgs = ReferencePool.Acquire<CSharpLuaDataChangeEventArgs>();
//         dataChangeEventArgs.DataType = (int)dataType;
//         dataChangeEventArgs.ValueType = (int)valueType;
//         dataChangeEventArgs.IntValue = intValue;
//         dataChangeEventArgs.FloatValue = floatValue;
//         dataChangeEventArgs.StringValue = strValue;
//
//         return dataChangeEventArgs;
//     }
//
//     /// <summary>
//     /// 清理加载配置成功事件。
//     /// </summary>
//     public override void Clear()
//     {
//         DataType = (int)GameDataType.DATA_TYPE.DT_DEFAULT;
//         SubDataType = 0;
//         IntValue = 0;
//         FloatValue = 0.0f;
//         StringValue = null;
//     }
// }
//
// #if UNITY_EDITOR
// public static class CSharpLuaDataChangeEventArgsExporter
// {
//     [LuaCallCSharp]
//     public static List<Type> LuaCallCSharp = new List<Type>()
//         {
//             typeof(CSharpLuaDataChangeEventArgs),
//         };
// }
// #endif
//
//
//

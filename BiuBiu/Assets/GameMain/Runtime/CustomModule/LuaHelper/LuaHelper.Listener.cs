//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Utility;
using UnityEngine;
using XLua;

namespace BiuBiu
{
	public static partial class LuaHelper
	{
		public static void AddClickListener(GameObject gameObj, LuaFunction callBack, LuaTable luaScript)
		{
			var eventTrigger = gameObj.GetOrAddComponent<UIEventTrigger>();
			eventTrigger.AddClickEvent(callBack, luaScript);
		}
		
		public static void AddClickListener(GameObject gameObj, LuaFunction callBack, LuaTable luaScript, bool boolParam)
		{
			var eventTrigger = gameObj.GetOrAddComponent<UIEventTrigger>();
			eventTrigger.AddClickEvent(callBack, luaScript, boolParam);
		}
		
		public static void AddClickListener(GameObject gameObj, LuaFunction callBack, LuaTable luaScript, int intParam)
		{
			var eventTrigger = gameObj.GetOrAddComponent<UIEventTrigger>();
			eventTrigger.AddClickEvent(callBack, luaScript, intParam);
		}
		
		public static void AddClickListener(GameObject gameObj, LuaFunction callBack, LuaTable luaScript, float floatParam)
		{
			var eventTrigger = gameObj.GetOrAddComponent<UIEventTrigger>();
			eventTrigger.AddClickEvent(callBack, luaScript, floatParam);
		}
		
		public static void AddClickListener(GameObject gameObj, LuaFunction callBack, LuaTable luaScript, LuaTable luaTableParam)
		{
			var eventTrigger = gameObj.GetOrAddComponent<UIEventTrigger>();
			eventTrigger.AddClickEvent(callBack, luaScript, luaTableParam);
		}
	}
}
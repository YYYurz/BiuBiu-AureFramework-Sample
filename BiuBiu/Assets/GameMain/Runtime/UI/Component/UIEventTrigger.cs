//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace BiuBiu
{
	[DisallowMultipleComponent]
	public class UIEventTrigger : MonoBehaviour, IPointerClickHandler
	{
		private enum ParamTypes
		{
			None,
			Bool,
			Int,
			Float,
			LuaTable,
		};
		
		private LuaFunction onClick;
		private LuaTable luaScript;

		private bool boolParam;
		private int intParam;
		private float floatParam;
		private LuaTable luaTableParam;
		private ParamTypes curParamType;

		public UIEventTrigger AddClickEvent(LuaFunction callBack, LuaTable luaSelf)
		{
			if (callBack == null)
			{
				Debug.LogError("UIEventTrigger : Add click event failed, callBack is null.");
				return null;
			}

			RemoveAllEvent();
			luaScript = luaSelf;
			onClick = callBack;
			curParamType = ParamTypes.None;
			return this;
		}
		
		public UIEventTrigger AddClickEvent(LuaFunction callBack, LuaTable luaSelf, bool param)
		{
			if (callBack == null)
			{
				Debug.LogError("UIEventTrigger : Add click event failed, callBack is null.");
				return null;
			}
			
			RemoveAllEvent();
			onClick = callBack;
			luaScript = luaSelf;
			boolParam = param;
			curParamType = ParamTypes.Bool;
			return this;
		}
		
		public UIEventTrigger AddClickEvent(LuaFunction callBack, LuaTable luaSelf, int param)
		{
			if (callBack == null)
			{
				Debug.LogError("UIEventTrigger : Add click event failed, callBack is null.");
				return null;
			}
			
			RemoveAllEvent();
			onClick = callBack;
			luaScript = luaSelf;
			intParam = param;
			curParamType = ParamTypes.Int;
			return this;
		}
		
		public UIEventTrigger AddClickEvent(LuaFunction callBack, LuaTable luaSelf, float param)
		{
			if (callBack == null)
			{
				Debug.LogError("UIEventTrigger : Add click event failed, callBack is null.");
				return null;
			}
			
			RemoveAllEvent();
			onClick = callBack;
			luaScript = luaSelf;
			floatParam = param;
			curParamType = ParamTypes.Float;
			return this;
		}
		
		public UIEventTrigger AddClickEvent(LuaFunction callBack, LuaTable luaSelf, LuaTable param)
		{
			if (callBack == null)
			{
				Debug.LogError("UIEventTrigger : Add click event failed, callBack is null.");
				return null;
			}
			
			RemoveAllEvent();
			onClick = callBack;
			luaScript = luaSelf;
			luaTableParam = param;
			curParamType = ParamTypes.LuaTable;
			return this;
		}


		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left || onClick == null)
			{
				return;
			}

			switch (curParamType)
			{
				case ParamTypes.None:
					onClick.Call(luaScript);
					break;
				case ParamTypes.Bool:
					onClick.Call(luaScript, boolParam);
					break;
				case ParamTypes.Int:
					onClick.Call(luaScript, intParam);
					break;
				case ParamTypes.Float:
					onClick.Call(luaScript, floatParam);
					break;
				case ParamTypes.LuaTable:
					onClick.Call(luaScript, luaTableParam);
					break;
			}
		}

		public void RemoveAllEvent()
		{
			// Debug.Log("Remove All Event");
			onClick?.Dispose();
			onClick = null;
			curParamType = ParamTypes.None;
		}

		private void OnDestroy()
		{
			RemoveAllEvent();
		}
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.UI;
using UnityEngine;
using XLua;

namespace BiuBiu
{
	/// <summary>
	/// Lua界面
	/// </summary>
	public sealed class LuaForm : UIFormBase
	{
		private LuaTable luaScriptTable;
		private UIFormOpenInfo uiFormOpenInfo;

		public override void OnInit(object userData)
		{
			base.OnInit(userData);
			uiFormOpenInfo = userData as UIFormOpenInfo;
			if (uiFormOpenInfo == null)
			{
			    Debug.LogError("LuaForm : UIFormOpenInfo is null.");
			    return;
			}

			var luaScript = (LuaTable) GameMain.Lua.DoString($"return require('{uiFormOpenInfo.LuaFile}')")[0];
			luaScriptTable = (LuaTable) GameMain.Lua.CallLuaFunction(luaScript, "New", new[] {typeof(LuaTable)}, luaScript)[0];
			
			if (luaScriptTable != null)
			{
				luaScriptTable.Set("uiFormId", uiFormOpenInfo.UIFormId);
				luaScriptTable.Set("transform", transform);
				luaScriptTable.Set("gameObject", gameObject);
				GameMain.Lua.CallLuaFunction(luaScriptTable, "OnInit", null, luaScriptTable);
			}
		}

		public override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			if (luaScriptTable != null)
			{
				GameMain.Lua.CallLuaFunction(luaScriptTable, "OnOpen", null, luaScriptTable, uiFormOpenInfo.UserData);
			}
		}

		public override void OnClose()
		{
			if (luaScriptTable != null)
			{
				GameMain.Lua.CallLuaFunction(luaScriptTable, "OnClose", null, luaScriptTable);
			}

			base.OnClose();
		}

		public override void OnDestroy()
		{
			if (luaScriptTable == null)
			{
				return;
			}
			
			GameMain.Lua.CallLuaFunction(luaScriptTable, "OnDestroy", null, luaScriptTable);
			luaScriptTable.Dispose();
			luaScriptTable = null;
			
			GameMain.ReferencePool.Release(uiFormOpenInfo);
			uiFormOpenInfo = null;
		}
	}
}
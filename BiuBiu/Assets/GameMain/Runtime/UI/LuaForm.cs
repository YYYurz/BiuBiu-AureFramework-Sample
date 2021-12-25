using UnityGameFramework.Runtime;
using XLua;

/// <summary>
/// Lua界面
/// </summary>
namespace BB
{
    public class LuaForm : UGuiForm
    {
        private string uiName;
        private LuaTable luaScriptTable;
        private UIFormOpenDataInfo formDataInfo { get; set; }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            formDataInfo = userData as UIFormOpenDataInfo;
            if (formDataInfo == null)
            {
                Log.Error("LuaForm Open Error! invalid userData!");
                return;
            }
            
            Name = Name.Replace("(Clone)", "");

            var luaScript = GameEntry.Lua.DoStringCustom(formDataInfo.LuaFile);
            luaScriptTable = GameEntry.Lua.CallLuaFunctionCustom(luaScript, "New", luaScript);
            
            // ReSharper disable once InvertIf
            if (luaScriptTable != null)
            {
                luaScriptTable.Set("uiFormID", formDataInfo.FormID);
                luaScriptTable.Set("transform", transform);
                luaScriptTable.Set("gameObject", gameObject);
                GameEntry.Lua.CallLuaFunction(luaScriptTable, "OnCreate", luaScriptTable);
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            formDataInfo = userData as UIFormOpenDataInfo;
            if (formDataInfo == null)
            {
                Log.Error("LuaForm Open Error! invalid userData!");
                return;
            }
            if (formDataInfo.UserData as string == "Preload")
            {
                return;
            }

            if (luaScriptTable != null)
            {
                GameEntry.Lua.CallLuaFunction(luaScriptTable, "OnOpen", luaScriptTable, formDataInfo.UserData);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            if (luaScriptTable != null)
            {
                GameEntry.Lua.CallLuaFunction(luaScriptTable, "OnClose", luaScriptTable);
            }

            base.OnClose(isShutdown, userData);
        }

        //protected override void OnOpenComplete()
        //{
        //    base.OnOpenComplete();

        //    if (m_FormManagerLuaTable != null)
        //    {
        //        GameManager.Lua.CallLuaFunction(m_FormManagerLuaTable, "OnOpenComplete", m_FormName);
        //    }
        //}

        public void OnDestroy()
        {
            if (luaScriptTable == null) return;
            GameEntry.Lua.CallLuaFunction(luaScriptTable, "OnDestroy", luaScriptTable);
            luaScriptTable.Dispose();
            luaScriptTable = null;
        }


    }
}
#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    
    public class TutorialTestEnumWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Tutorial.TestEnum), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Tutorial.TestEnum), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Tutorial.TestEnum), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E1", Tutorial.TestEnum.E1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E2", Tutorial.TestEnum.E2);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Tutorial.TestEnum), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushTutorialTestEnum(L, (Tutorial.TestEnum)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "E1"))
                {
                    translator.PushTutorialTestEnum(L, Tutorial.TestEnum.E1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "E2"))
                {
                    translator.PushTutorialTestEnum(L, Tutorial.TestEnum.E2);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Tutorial.TestEnum!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Tutorial.TestEnum! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class TutorialDerivedClassTestEnumInnerWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Tutorial.DerivedClass.TestEnumInner), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Tutorial.DerivedClass.TestEnumInner), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Tutorial.DerivedClass.TestEnumInner), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E3", Tutorial.DerivedClass.TestEnumInner.E3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E4", Tutorial.DerivedClass.TestEnumInner.E4);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Tutorial.DerivedClass.TestEnumInner), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushTutorialDerivedClassTestEnumInner(L, (Tutorial.DerivedClass.TestEnumInner)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "E3"))
                {
                    translator.PushTutorialDerivedClassTestEnumInner(L, Tutorial.DerivedClass.TestEnumInner.E3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "E4"))
                {
                    translator.PushTutorialDerivedClassTestEnumInner(L, Tutorial.DerivedClass.TestEnumInner.E4);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Tutorial.DerivedClass.TestEnumInner!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Tutorial.DerivedClass.TestEnumInner! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineEventSystemsEventTriggerTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.EventSystems.EventTriggerType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.EventSystems.EventTriggerType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.EventSystems.EventTriggerType), L, null, 18, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PointerEnter", UnityEngine.EventSystems.EventTriggerType.PointerEnter);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PointerExit", UnityEngine.EventSystems.EventTriggerType.PointerExit);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PointerDown", UnityEngine.EventSystems.EventTriggerType.PointerDown);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PointerUp", UnityEngine.EventSystems.EventTriggerType.PointerUp);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PointerClick", UnityEngine.EventSystems.EventTriggerType.PointerClick);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Drag", UnityEngine.EventSystems.EventTriggerType.Drag);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Drop", UnityEngine.EventSystems.EventTriggerType.Drop);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Scroll", UnityEngine.EventSystems.EventTriggerType.Scroll);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpdateSelected", UnityEngine.EventSystems.EventTriggerType.UpdateSelected);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Select", UnityEngine.EventSystems.EventTriggerType.Select);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Deselect", UnityEngine.EventSystems.EventTriggerType.Deselect);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Move", UnityEngine.EventSystems.EventTriggerType.Move);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InitializePotentialDrag", UnityEngine.EventSystems.EventTriggerType.InitializePotentialDrag);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BeginDrag", UnityEngine.EventSystems.EventTriggerType.BeginDrag);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EndDrag", UnityEngine.EventSystems.EventTriggerType.EndDrag);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Submit", UnityEngine.EventSystems.EventTriggerType.Submit);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Cancel", UnityEngine.EventSystems.EventTriggerType.Cancel);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.EventSystems.EventTriggerType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineEventSystemsEventTriggerType(L, (UnityEngine.EventSystems.EventTriggerType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "PointerEnter"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.PointerEnter);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PointerExit"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.PointerExit);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PointerDown"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.PointerDown);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PointerUp"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.PointerUp);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PointerClick"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.PointerClick);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Drag"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Drag);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Drop"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Drop);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Scroll"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Scroll);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UpdateSelected"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.UpdateSelected);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Select"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Select);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Deselect"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Deselect);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Move"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Move);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "InitializePotentialDrag"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.InitializePotentialDrag);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BeginDrag"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.BeginDrag);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "EndDrag"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.EndDrag);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Submit"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Submit);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Cancel"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Cancel);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.EventSystems.EventTriggerType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.EventSystems.EventTriggerType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}
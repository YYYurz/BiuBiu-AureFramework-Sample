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
    public class BiuBiuUIEventTriggerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(BiuBiu.UIEventTrigger);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddClickEvent", _m_AddClickEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPointerClick", _m_OnPointerClick);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new BiuBiu.UIEventTrigger();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to BiuBiu.UIEventTrigger constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddClickEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BiuBiu.UIEventTrigger gen_to_be_invoked = (BiuBiu.UIEventTrigger)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaFunction _callBack = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    XLua.LuaTable _luaSelf = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.AddClickEvent( _callBack, _luaSelf );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    XLua.LuaFunction _callBack = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    XLua.LuaTable _luaSelf = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    bool _param = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.AddClickEvent( _callBack, _luaSelf, _param );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    XLua.LuaFunction _callBack = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    XLua.LuaTable _luaSelf = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    int _param = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.AddClickEvent( _callBack, _luaSelf, _param );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    XLua.LuaFunction _callBack = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    XLua.LuaTable _luaSelf = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    float _param = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.AddClickEvent( _callBack, _luaSelf, _param );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaFunction _callBack = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    XLua.LuaTable _luaSelf = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.AddClickEvent( _callBack, _luaSelf, _param );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BiuBiu.UIEventTrigger.AddClickEvent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPointerClick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BiuBiu.UIEventTrigger gen_to_be_invoked = (BiuBiu.UIEventTrigger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnPointerClick( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

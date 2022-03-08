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
    public class BiuBiuLuaHelperWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(BiuBiu.LuaHelper);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 21, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "StartGame", _m_StartGame_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddClickListener", _m_AddClickListener_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesFile", _m_GetBytesFile_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeScene", _m_ChangeScene_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PlaySound", _m_PlaySound_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StopSound", _m_StopSound_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PauseSound", _m_PauseSound_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ResumeSound", _m_ResumeSound_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsEditor", _m_IsEditor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsUIOpen", _m_IsUIOpen_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OpenUI", _m_OpenUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CloseUI", _m_CloseUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetChild", _m_GetChild_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetText", _m_GetText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetImage", _m_GetImage_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRawImage", _m_GetRawImage_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetButton", _m_GetButton_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetSlider", _m_GetSlider_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetContentList", _m_GetContentList_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetSpriteLoader", _m_GetSpriteLoader_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "BiuBiu.LuaHelper does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartGame_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    uint _gameId = LuaAPI.xlua_touint(L, 1);
                    
                    BiuBiu.LuaHelper.StartGame( _gameId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddClickListener_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    UnityEngine.GameObject _gameObj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    XLua.LuaFunction _callBack = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    XLua.LuaTable _luaScript = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    BiuBiu.LuaHelper.AddClickListener( _gameObj, _callBack, _luaScript );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.GameObject _gameObj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    XLua.LuaFunction _callBack = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    XLua.LuaTable _luaScript = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    bool _boolParam = LuaAPI.lua_toboolean(L, 4);
                    
                    BiuBiu.LuaHelper.AddClickListener( _gameObj, _callBack, _luaScript, _boolParam );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.GameObject _gameObj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    XLua.LuaFunction _callBack = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    XLua.LuaTable _luaScript = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    int _intParam = LuaAPI.xlua_tointeger(L, 4);
                    
                    BiuBiu.LuaHelper.AddClickListener( _gameObj, _callBack, _luaScript, _intParam );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.GameObject _gameObj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    XLua.LuaFunction _callBack = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    XLua.LuaTable _luaScript = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    float _floatParam = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    BiuBiu.LuaHelper.AddClickListener( _gameObj, _callBack, _luaScript, _floatParam );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TTABLE)) 
                {
                    UnityEngine.GameObject _gameObj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    XLua.LuaFunction _callBack = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    XLua.LuaTable _luaScript = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    XLua.LuaTable _luaTableParam = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    BiuBiu.LuaHelper.AddClickListener( _gameObj, _callBack, _luaScript, _luaTableParam );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BiuBiu.LuaHelper.AddClickListener!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesFile_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _filePath = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = BiuBiu.LuaHelper.GetBytesFile( _filePath );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeScene_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _sceneType = LuaAPI.xlua_tointeger(L, 1);
                    uint _sceneId = LuaAPI.xlua_touint(L, 2);
                    
                    BiuBiu.LuaHelper.ChangeScene( _sceneType, _sceneId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaySound_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.GameObject>(L, 3)) 
                {
                    uint _soundId = LuaAPI.xlua_touint(L, 1);
                    float _fadeInSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.GameObject _bindingObject = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                        var gen_ret = BiuBiu.LuaHelper.PlaySound( _soundId, _fadeInSeconds, _bindingObject );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    uint _soundId = LuaAPI.xlua_touint(L, 1);
                    float _fadeInSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        var gen_ret = BiuBiu.LuaHelper.PlaySound( _soundId, _fadeInSeconds );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    uint _soundId = LuaAPI.xlua_touint(L, 1);
                    
                        var gen_ret = BiuBiu.LuaHelper.PlaySound( _soundId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BiuBiu.LuaHelper.PlaySound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopSound_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _soundId = LuaAPI.xlua_tointeger(L, 1);
                    float _fadeOutSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    BiuBiu.LuaHelper.StopSound( _soundId, _fadeOutSeconds );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    int _soundId = LuaAPI.xlua_tointeger(L, 1);
                    
                    BiuBiu.LuaHelper.StopSound( _soundId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BiuBiu.LuaHelper.StopSound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PauseSound_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _soundId = LuaAPI.xlua_tointeger(L, 1);
                    float _fadeOutSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    BiuBiu.LuaHelper.PauseSound( _soundId, _fadeOutSeconds );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    int _soundId = LuaAPI.xlua_tointeger(L, 1);
                    
                    BiuBiu.LuaHelper.PauseSound( _soundId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BiuBiu.LuaHelper.PauseSound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResumeSound_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _soundId = LuaAPI.xlua_tointeger(L, 1);
                    float _fadeInSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    BiuBiu.LuaHelper.ResumeSound( _soundId, _fadeInSeconds );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    int _soundId = LuaAPI.xlua_tointeger(L, 1);
                    
                    BiuBiu.LuaHelper.ResumeSound( _soundId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BiuBiu.LuaHelper.ResumeSound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsEditor_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        var gen_ret = BiuBiu.LuaHelper.IsEditor(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsUIOpen_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    uint _uiFormId = LuaAPI.xlua_touint(L, 1);
                    
                        var gen_ret = BiuBiu.LuaHelper.IsUIOpen( _uiFormId );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _uiName = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = BiuBiu.LuaHelper.IsUIOpen( _uiName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BiuBiu.LuaHelper.IsUIOpen!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    uint _uiFormId = LuaAPI.xlua_touint(L, 1);
                    object _userData = translator.GetObject(L, 2, typeof(object));
                    
                    BiuBiu.LuaHelper.OpenUI( _uiFormId, _userData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    uint _uiFormId = LuaAPI.xlua_touint(L, 1);
                    
                    BiuBiu.LuaHelper.CloseUI( _uiFormId );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _uiName = LuaAPI.lua_tostring(L, 1);
                    
                    BiuBiu.LuaHelper.CloseUI( _uiName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BiuBiu.LuaHelper.CloseUI!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetChild_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _selfObj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = BiuBiu.LuaHelper.GetChild( _selfObj, _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetText_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = BiuBiu.LuaHelper.GetText( _obj, _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetImage_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = BiuBiu.LuaHelper.GetImage( _obj, _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRawImage_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = BiuBiu.LuaHelper.GetRawImage( _obj, _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetButton_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = BiuBiu.LuaHelper.GetButton( _obj, _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSlider_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = BiuBiu.LuaHelper.GetSlider( _obj, _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetContentList_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = BiuBiu.LuaHelper.GetContentList( _obj, _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSpriteLoader_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = BiuBiu.LuaHelper.GetSpriteLoader( _obj, _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

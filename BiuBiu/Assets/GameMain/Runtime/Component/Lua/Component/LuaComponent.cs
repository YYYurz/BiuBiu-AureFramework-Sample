 

using GameFramework;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLua;

/// <summary>
/// Lua组件game
/// </summary>
namespace BB
{
    public class LuaComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 全局唯一Lua虚拟环境
        /// </summary>
        private static LuaEnv _luaEnv;
        private ResourceComponent _resourceComponent;
        private EventComponent _eventComponent;
        // private LuaUpdater _luaUpdater = null;

        // public CSharpLuaDataHelper LuaDataHelper { get; private set; } = new CSharpLuaDataHelper();

        /// <summary>
        /// Lua文件列表信息
        /// </summary>
        public List<LuaFileInfo> LuaFileInfos { get; private set; }

        private Dictionary<string, string> CacheLuaDict { get; set; }
        
        private void Start()
        {
            _resourceComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<ResourceComponent>();
            _eventComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<EventComponent>();
            if (_resourceComponent == null)
            {
                Log.Error(" m_ResourceComponent is null.");
                
                return;
            }

            CacheLuaDict = new Dictionary<string, string>();
            _luaEnv = new LuaEnv();
            // LuaArrAccessAPI.RegisterPinFunc(_luaEnv.L);
            LuaFileInfos = new List<LuaFileInfo>();
        }

        private void Update()
        {
            if (_luaEnv == null)
            {
                return;
            }
            _luaEnv.Tick();
        }

        private void OnDestroy()
        {
            Log.Debug("LuaComponent Dispose");

            CacheLuaDict.Clear();
            // if (_luaUpdater != null)
            // {
            //     _luaUpdater.OnDispose();
            // }

            if (_luaEnv == null)
            {
                return;
            }

            Log.Debug("LuaComponent Dispose");
            _luaEnv.Dispose();
            _luaEnv = null;
        }

        /// <summary>
        /// 加载lua文件列表
        /// </summary>
        public void LoadLuaFilesConfig()
        {
            var callBacks = new LoadAssetCallbacks(OnLoadLuaFilesConfigSuccess, OnLoadLuaFilesConfigFailure);
            var assetName = AssetUtils.GetLuaFileConfig();
            _resourceComponent.LoadAsset(assetName, callBacks);
        }

        /// <summary>
        /// 解析Lua文件配置列表
        /// </summary>
        /// <param name="content">配置列表内容</param>
        private void ParseLuaFilesConfig(string content)
        {
            if (LuaFileInfos == null)
            {
                Log.Error("ParseLuaFilesConfig Error!!!!!! LuaFileInfos is null!");
                return;
            }
            LuaFileInfos.Clear();
            var contentLines = content.Split('\n');
            var len = contentLines.Length;
            for (var i = 0; i < len; i++)
            {
                if (string.IsNullOrEmpty(contentLines[i]))
                {
                    return;
                }

                LuaFileInfo info = null;
                // var info = GameUtils.DeserializeObject<LuaFileInfo>(contentLines[i]);
                if (info == null)
                {
                    Log.Error("invalid parse result!");
                }
                else
                {
                    LuaFileInfos.Add(info);
                }
            }
        }

        /// <summary>
        /// 初始化 Lua 环境第三方库接口
        /// </summary>
        public void InitLuaEnvExternalInterface()
        {
            if (_luaEnv != null)
            {
                _luaEnv.AddLoader(CustomLoader);
                // _luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
                // _luaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
                // _luaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadPb);
                // _luaEnv.AddBuildin("protobuf.c", XLua.LuaDLL.Lua.LoadProtobufC);
            }
            else
            {
                Log.Error("InitLuaEnv error! invalid luaenv");
            }
        }
        
        public void InitLuaCommonScript()
        {
            if (_luaEnv == null)
            {
                return;
            }
            // RequireLuaFile(Constant.Lua.LuaCommonMainScript);
            // _luaUpdater = gameObject.GetComponent<LuaUpdater>();
            // if (_luaUpdater == null)
            // {
            //     _luaUpdater = gameObject.AddComponent<LuaUpdater>();
            // }
            // _luaUpdater.OnInit(_luaEnv);
        }
        
        public void StartRunLuaLogic()
        {
            if (_luaEnv == null)
            {
                return;
            }
            RequireLuaFile(Constant.Lua.LuaGameMainScript);
        }

        /// <summary>
        /// 加载lua文件
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="luaName"></param>
        public void LoadLuaFile(string luaName, string assetName)
        {
            var callBacks = new LoadAssetCallbacks(OnLoadLuaAssetSuccess, OnLoadLuaAssetFailure);
            
            if (CacheLuaDict.TryGetValue(luaName, out var strLuaValue))
            {
                _eventComponent.Fire(this, ReferencePool.Acquire<LoadLuaSuccessEventArgs>().Fill(assetName, luaName, strLuaValue, 0));
                return;
            }
            
            _resourceComponent.LoadAsset(assetName, callBacks, luaName);
        }

        /// <summary>
        /// 执行lua文件
        /// </summary>
        /// <param name="luaName"></param>
        public void DoLuaFile(string luaName)
        {
            if (CacheLuaDict.TryGetValue(luaName, out var strValue))
            {
                SafeDoString(strValue);
            }
            else
            {
                Log.Error("Lua file '{0}' is not load,please execute LoadLuaFile() first.", luaName);
            }
        }

        public void RequireLuaFile(string scriptName)
        {
            SafeDoString($"require('{scriptName}')");
        }

        public void SafeDoString(string scriptContent)
        {
            // ReSharper disable once InvertIf
            if (_luaEnv != null)
            {
                try
                {
                    _luaEnv.DoString(scriptContent);
                }
                catch (Exception ex)
                {
                    Log.Error($"xLua exception : {ex.Message}\n {ex.StackTrace} scriptContent:{scriptContent}");
                }
            }
        }

        public LuaTable DoStringCustom(string scriptName)
        {
            return _luaEnv.DoStringCustom($"return require('{scriptName}')");
        }

        /// <summary>
        /// 获取全局类的LuaTable
        /// </summary>
        public LuaTable GetClassLuaTable(string className)
        {
            var classLuaTable = _luaEnv.Global.Get<LuaTable>(className);
            return classLuaTable;
        }

        /// <summary>
        /// 获取LuaTable
        /// </summary>
        public LuaTable GetLuaTable(string luaName, string className, string tableName)
        {
            if (CacheLuaDict.ContainsKey(luaName))
            {
                var classLuaTable = _luaEnv.Global.Get<LuaTable>(className);
                var luaTable = classLuaTable.Get<LuaTable>(tableName);
                classLuaTable.Dispose();
                return luaTable;
            }
            Log.Error("Lua file '{0}' is not load,please execute LoadLuaFile() first.", luaName); 
            return null;
        }

        public void CallLuaFunction(LuaTable luaTable, string funcName, params object[] param)
        {
            if (luaTable != null)
            {
                try
                {
                    var luaFunction = luaTable.Get<LuaFunction>(funcName);
                    luaFunction.Call(param, null);
                    luaFunction.Dispose();
                }
                catch (Exception exception)
                {
                    Log.Error(exception.Message);
                }
            }
            else
            {
                Log.Error("LuaTable is invalid.");
            }
        }

        public LuaTable CallLuaFunctionCustom(LuaTable luaTable, string funcName, params object[] param)
        {
            if (luaTable != null)
            {
                try
                {
                    var luaFunction = luaTable.Get<LuaFunction>(funcName);
                    var types = new[] {typeof(LuaTable)};
                    var result = luaFunction.Call(param, types);
                    luaFunction.Dispose();
                    return (LuaTable) result[0];
                }
                catch (Exception exception)
                {
                    Log.Error(exception.Message);
                }
            }
            else
            {
                Log.Error("LuaTable is invalid.");
            }
            
            return null;
        }

        public void CallLuaFunction(string luaName, string className, string funcName, params object[] parms)
        {
            if (CacheLuaDict.ContainsKey(luaName))
            {
                try
                {
                    var classLuaTable = _luaEnv.Global.Get<LuaTable>(className);
                    var luaFunc = classLuaTable.Get<LuaFunction>(funcName);
                    luaFunc.Call(parms);
                    classLuaTable.Dispose();
                    luaFunc.Dispose();
                }
                catch (Exception exception)
                {
                    Log.Error(exception.Message);
                }
            }
            else
            {
                Log.Error("Lua file '{0}' is not load,please execute LoadLuaFile() first.", luaName);
            }
        }

        private void OnLoadLuaFilesConfigFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Info("Load LuaFilesConfig: '{0}' failure.", assetName);
            // _eventComponent.Fire(this, ReferencePool.Acquire<LoadLuaFilesConfigFailureEventArgs>().Fill(assetName, errorMessage));
        }

        private void OnLoadLuaFilesConfigSuccess(string assetName, object asset, float duration, object userData)
        {
            var textAsset = (TextAsset)asset;
            Log.Info("Load LuaFilesConfig: '{0}' success.", assetName);

            var content = textAsset.text;
            //开始解析Lua配置文件列表
            ParseLuaFilesConfig(content);
            _eventComponent.Fire(this, ReferencePool.Acquire<LoadLuaFilesConfigSuccessEventArgs>().Fill(assetName, content));

            _resourceComponent.UnloadAsset(asset);
        }

        private void OnLoadLuaAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            var luaName = (string)userData;
            if (CacheLuaDict.ContainsKey(luaName))
            {
                Log.Warning("CacheLuaDict has exist lua file '{0}'.", luaName);
                return;
            }
            var textAsset = (TextAsset)asset;
            CacheLuaDict.Add(luaName, textAsset.text);
            _eventComponent.Fire(this, ReferencePool.Acquire<LoadLuaSuccessEventArgs>().Fill(assetName, luaName, textAsset.text, duration));
        }

        private void OnLoadLuaAssetFailure(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            var luaName = (string)userData;
            var errorMessage = string.Format("Load lua file failed. The file is {0}. ", assetName);
            _eventComponent.Fire(this, ReferencePool.Acquire<LoadLuaFailureEventArgs>().Fill(assetName, luaName, errorMessage));
        }

        private static byte[] CustomLoader(ref string filepath)
        {
            var strLuaName = filepath.Replace(".", "/");
             if (strLuaName == "emmy_core")
             {
                 return null;
             }
             if (GameEntry.Lua.CacheLuaDict.TryGetValue(strLuaName, out var strLuaContent))
             {
                 return Utility.Converter.GetBytes(strLuaContent);
             }
             return null;
        }
    }
}

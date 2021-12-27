using UnityEngine;
using AureFramework.Event;
using AureFramework.Procedure;

namespace BiuBiu
{
    public class ProcedurePreload : ProcedureBase
    {
        private StartWindow startWindowScript; 
            
        private bool allAssetLoadedComplete;
        
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);

            var uiRoot = GameMain.UI.transform;
            var startWindow = StartWindow.CreateStartWindow(uiRoot);
            startWindowScript = startWindow.GetComponent<StartWindow>();
            
            GameMain.Event.Subscribe(LoadLuaFilesConfigSuccessEventArgs.EventId, OnLoadLuaFilesConfigSuccess);
            GameMain.Event.Subscribe(PreloadProgressCompleteEventArgs.EventId, OnAllAssetsLoadedComplete);
            GameMain.Event.Subscribe(PreloadProgressLoadingEventArgs.EventId, OnPreloadProgress);
            
            allAssetLoadedComplete = false;
            GameMain.Lua.LoadLuaFilesConfig();
        }

        public override void OnExit()
        {
            base.OnExit();
            GameMain.Event.Unsubscribe(LoadLuaFilesConfigSuccessEventArgs.EventId, OnLoadLuaFilesConfigSuccess);
            GameMain.Event.Unsubscribe(PreloadProgressCompleteEventArgs.EventId, OnAllAssetsLoadedComplete);
            GameMain.Event.Unsubscribe(PreloadProgressLoadingEventArgs.EventId, OnPreloadProgress);

            startWindowScript.DestroySelf();
            startWindowScript = null;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (!allAssetLoadedComplete)
            {
                return;
            }

            ChangeState<ProcedureChangeScene>();
        }

        private void OnLoadLuaFilesConfigSuccess(object sender, AureEventArgs e)
        {
            var assetLuaFileInfo = new PreloadLuaFileList();
            assetLuaFileInfo.SetLuaFileInfo(GameMain.Lua.LuaFileInfos);
            GameMain.AssetPreload.AddAssetPreloadList(assetLuaFileInfo);
            
            var assetDataTableInfo = new PreloadDataTableList();
            assetDataTableInfo.AddOneAssetInfo(typeof(DTGameConfigTableReader));
            assetDataTableInfo.AddOneAssetInfo(typeof(DTUIWindowTableReader));
            assetDataTableInfo.AddOneAssetInfo(typeof(DTSoundTableReader));
            assetDataTableInfo.AddOneAssetInfo(typeof(DTSceneTableReader));
            assetDataTableInfo.AddOneAssetInfo(typeof(DTEntityTableReader));
            assetDataTableInfo.AddOneAssetInfo(typeof(DTVocationTableReader));
            GameMain.AssetPreload.AddAssetPreloadList(assetDataTableInfo);

            GameMain.AssetPreload.StartPreloadAsset();
        }
        
        private void OnAllAssetsLoadedComplete(object sender, AureEventArgs e)
        {
            GameMain.Lua.InitLuaEnvExternalInterface();
            GameMain.Lua.InitLuaCommonScript();
            GameMain.Lua.StartRunLuaLogic();
            allAssetLoadedComplete = true;
        }

        private void OnPreloadProgress(object sender, AureEventArgs e)
        {
            if (!(e is PreloadProgressLoadingEventArgs args))
            {
                Debug.LogError("ProcedurePreload : PreloadProgressLoadingEventArgs is null");
                return;
            }
            startWindowScript.SetSliderProgress((float)args.LoadedAssetsCount / args.TotalAssetsCount);
        }
    }
}

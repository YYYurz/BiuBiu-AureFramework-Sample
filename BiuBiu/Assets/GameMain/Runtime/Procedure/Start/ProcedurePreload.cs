using GameConfig;
using GameFramework.Event;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace BB
{
    public class ProcedurePreload : ProcedureBase
    {
        private StartWindow startWindowScript; 
            
        private bool allAssetLoadedComplete;
        
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("ProcedurePreload OnEnter");

            var uiRoot = GameEntry.UI.transform;
            var startWindow = StartWindow.CreateStartWindow(uiRoot);
            startWindowScript = startWindow.GetComponent<StartWindow>();
            
            GameEntry.Event.Subscribe(LoadLuaFilesConfigSuccessEventArgs.EventId, OnLoadLuaFilesConfigSuccess);
            GameEntry.Event.Subscribe(PreloadProgressCompleteEventArgs.EventId, OnAllAssetsLoadedComplete);
            GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            GameEntry.Event.Subscribe(PreloadProgressLoadingEventArgs.EventId, OnPreloadProgress);
            
            allAssetLoadedComplete = false;
            GameEntry.Lua.LoadLuaFilesConfig();
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(LoadLuaFilesConfigSuccessEventArgs.EventId, OnLoadLuaFilesConfigSuccess);
            GameEntry.Event.Unsubscribe(PreloadProgressCompleteEventArgs.EventId, OnAllAssetsLoadedComplete);
            GameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            GameEntry.Event.Unsubscribe(PreloadProgressLoadingEventArgs.EventId, OnPreloadProgress);

            startWindowScript.DestroySelf();
            startWindowScript = null;
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            if (!allAssetLoadedComplete)
            {
                return;
            }

            // procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, (int)GameEnum.SCENE_TYPE.MainLobby);
            procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, (int)GameEnum.SCENE_TYPE.HomeLand);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        private void OnLoadLuaFilesConfigSuccess(object sender, GameEventArgs e)
        {
            var assetLuaFileInfo = new PreloadLuaFileList();
            assetLuaFileInfo.SetLuaFileInfo(GameEntry.Lua.LuaFileInfos);
            GameEntry.AssetPreload.AddAssetPreloadList(assetLuaFileInfo);
            
            var assetDataTableInfo = new PreloadDataTableList();
            assetDataTableInfo.AddOneAssetInfo(typeof(DTGameConfigTableReader));
            assetDataTableInfo.AddOneAssetInfo(typeof(DTUIWindowTableReader));
            assetDataTableInfo.AddOneAssetInfo(typeof(DTSoundTableReader));
            assetDataTableInfo.AddOneAssetInfo(typeof(DTSceneTableReader));
            assetDataTableInfo.AddOneAssetInfo(typeof(DTEntityTableReader));
            assetDataTableInfo.AddOneAssetInfo(typeof(DTVocationTableReader));
            GameEntry.AssetPreload.AddAssetPreloadList(assetDataTableInfo);

            GameEntry.AssetPreload.StartPreloadAsset();
        }
        
        private void OnAllAssetsLoadedComplete(object sender, GameEventArgs e)
        {
            Log.Debug("ProcedurePreload : All Assets Preload Over");
            GameEntry.Lua.InitLuaEnvExternalInterface();
            GameEntry.Lua.InitLuaCommonScript();
            GameEntry.Lua.StartRunLuaLogic();
            allAssetLoadedComplete = true;
        }

        private void OnPreloadProgress(object sender, GameEventArgs e)
        {
            if (!(e is PreloadProgressLoadingEventArgs args))
            {
                Log.Error("ProcedurePreload : PreloadProgressLoadingEventArgs is null");
                return;
            }
            startWindowScript.SetSliderProgress((float)args.LoadedAssetsCount / args.TotalAssetsCount);
        }

        private void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {
            var args = (OpenUIFormFailureEventArgs) e;
            Debug.Log("Open Failed" + args.ErrorMessage);
        }
    }
}

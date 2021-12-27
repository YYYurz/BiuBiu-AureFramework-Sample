using AureFramework.UI;

namespace BiuBiu
{
    public static class UIExtension
    {
        public static void OpenUI(this IUIModule uiModule, int uiFormId, object userData = null)
        {
            // DTUIWindow? uiFormDataTable = GameMain.TableData.DataTableInfo.GetDataTableReader<DTUIWindowTableReader>().GetInfo((uint)uiFormId);
            // var strAssetPath = AssetUtils.GetUIFormAsset(uiFormDataTable.Value.AssetPath);
            // if (uiFormDataTable.Value.AllowMultiInstance == 0)
            // {
            //     if (uiModule.IsLoadingUIForm(strAssetPath))
            //     {
            //         return null;
            //     }
            //
            //     if (uiModule.HasUIForm(strAssetPath))
            //     {
            //         return null;
            //     }
            // }
            // var uiFormOpenDataInfo = UIFormOpenDataInfo.Create(uiFormId, uiFormDataTable.Value.LuaFile, userData);
            // return uiModule.OpenUIForm(strAssetPath, uiFormDataTable.Value.UIGroupName, Constant.AssetPriority.UIFormAsset, (uiFormDataTable.Value.PauseCoveredUIForm == 1), uiFormOpenDataInfo);
        }
    }
}

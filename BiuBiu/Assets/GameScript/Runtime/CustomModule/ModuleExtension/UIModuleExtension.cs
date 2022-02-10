//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.UI;

namespace BiuBiu
{
    /// <summary>
    /// UI模块扩展类
    /// </summary>
    public static class UIModuleExtension
    {
        public static bool IsUIOpen(this IUIModule uiModule, uint uiFormId)
        {
            var uiData = GameMain.DataTable.GetDataTableReader<UIWindowTableReader>().GetInfo(uiFormId);
            var uiName = uiData.UIName;
            
            return uiModule.IsUIOpen(uiName);
        }
        
        public static void OpenUI(this IUIModule uiModule, uint uiFormId, object userData = null)
        {
            var uiData = GameMain.DataTable.GetDataTableReader<UIWindowTableReader>().GetInfo(uiFormId);
            var uiName = uiData.UIName;
            var uiAssetName = uiData.AssetName;
            var uiGroupName = uiData.UIGroupName;
            var uiFormOpenDataInfo = UIFormOpenInfo.Create(uiFormId, uiData.LuaFile, userData);
            
            uiModule.OpenUI(uiName, uiAssetName, uiGroupName, uiFormOpenDataInfo);
        }

        public static void CloseUI(this IUIModule uiModule, uint uiFormId)
        {
            var uiInfo = GameMain.DataTable.GetDataTableReader<UIWindowTableReader>().GetInfo(uiFormId);
            var uiName = uiInfo.UIName;
            
            uiModule.CloseUI(uiName);
        }
    }
}

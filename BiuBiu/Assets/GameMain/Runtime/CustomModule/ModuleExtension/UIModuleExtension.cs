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
        public static bool IsUIOpen(this IUIModule uiModule, int uiFormId)
        {
            var uiInfo = GameMain.UIDataTable.GetDataTableReader<UIWindowTableReader>().GetInfo((uint)uiFormId);
            var uiName = uiInfo.UIName;
            
            return uiModule.IsUIOpen(uiName);
        }
        
        public static void OpenUI(this IUIModule uiModule, int uiFormId, object userData = null)
        {
            var uiInfo = GameMain.UIDataTable.GetDataTableReader<UIWindowTableReader>().GetInfo((uint)uiFormId);
            var uiName = uiInfo.UIName;
            var uiAssetName = uiInfo.AssetName;
            var uiGroupName = uiInfo.UIGroupName;
            var uiFormOpenDataInfo = UIFormOpenInfo.Create(uiFormId, uiInfo.LuaFile, userData);
            
            uiModule.OpenUI(uiName, uiAssetName, uiGroupName, uiFormOpenDataInfo);
        }

        public static void CloseUI(this IUIModule uiModule, int uiFormId)
        {
            var uiInfo = GameMain.UIDataTable.GetDataTableReader<UIWindowTableReader>().GetInfo((uint)uiFormId);
            var uiName = uiInfo.UIName;
            
            uiModule.CloseUI(uiName);
        }
    }
}

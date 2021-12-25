using System;
using GameConfig;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XLua;

namespace BB
{
    public static class UIExtension
    {
        public static bool IsUIOpen(this UIComponent uiComponent, int uiFormId)
        {
            var assetName = GameEntry.TableData.DataTableInfo.GetDataTableReader<DTUIWindowTableReader>().GetInfo((uint)uiFormId).AssetPath;
            return uiComponent.HasUIForm(AssetUtils.GetUIFormAsset(assetName));
        }

        public static void CloseUI(this UIComponent uiComponent, int uiFormId)
        {
            var assetName = GameEntry.TableData.DataTableInfo.GetDataTableReader<DTUIWindowTableReader>().GetInfo((uint)uiFormId).AssetPath;
            var uiForm = uiComponent.GetUIForm(AssetUtils.GetUIFormAsset(assetName));
            var uis = uiComponent.GetAllLoadedUIForms();
            if (uiForm == null)
            {
                Log.Error("UIExtension : Try to close UI but failed UI name : " + assetName);
                return;
            }
            uiComponent.CloseUIForm(uiForm);
        }

        public static int? OpenUI(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            DTUIWindow? uiFormDataTable = GameEntry.TableData.DataTableInfo.GetDataTableReader<DTUIWindowTableReader>().GetInfo((uint)uiFormId);
            var strAssetPath = AssetUtils.GetUIFormAsset(uiFormDataTable.Value.AssetPath);
            if (uiFormDataTable.Value.AllowMultiInstance == 0)
            {
                if (uiComponent.IsLoadingUIForm(strAssetPath))
                {
                    return null;
                }
            
                if (uiComponent.HasUIForm(strAssetPath))
                {
                    return null;
                }
            }
            var uiFormOpenDataInfo = UIFormOpenDataInfo.Create(uiFormId, uiFormDataTable.Value.LuaFile, userData);
            return uiComponent.OpenUIForm(strAssetPath, uiFormDataTable.Value.UIGroupName, Constant.AssetPriority.UIFormAsset, (uiFormDataTable.Value.PauseCoveredUIForm == 1), uiFormOpenDataInfo);
        }

        public static void AddButtonClickListener(Button button, Action<LuaTable> callBack, LuaTable self) {
            
        }

        public static void CloseAllUIForms(this UIComponent uiComponent)
        {
            uiComponent.CloseAllLoadedUIForms();
            uiComponent.CloseAllLoadingUIForms();
        }
    }
}

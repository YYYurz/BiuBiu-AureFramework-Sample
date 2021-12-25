using System;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XLua;

namespace BB
{
    /// <summary>
    /// Lua调用C# UI相关接口
    /// </summary>
    // ReSharper disable once UnusedType.Global
    // ReSharper disable once InconsistentNaming
    public static class UIUtils
    {
        public static bool IsEditor() => Application.isEditor;
        
        public static bool IsUIOpen(int uiFormID) => GameEntry.UI.IsUIOpen(uiFormID);
        
        public static void OpenWindow(int uiFormID) => GameEntry.UI.OpenUI(uiFormID);

        public static void CloseWindow(int uiFormID) => GameEntry.UI.CloseUI(uiFormID);

        public static void AddButtonClickListener(Button button, Action<LuaTable> callBack, LuaTable tableSelf)
        {
            if (callBack == null)
            {
                Log.Debug($"LuaUIHelper : Button Listener Callback = null");
                return;
            }
            button.onClick.AddListener(() =>
            {
                callBack.Invoke(tableSelf);
            });
        }

        public static void RemoveButtonClickListener(Button button)
        {
            if (button == null)
            {
                Log.Debug($"LuaUIHelper : Remove Button Click false");
                return;
            }
            button.onClick.RemoveAllListeners();
        }
        
        // public static void AddButton
        
        private static T GetChild<T>(GameObject selfObj, string path)
        {
            if (selfObj == null)
            {
                Log.Error("LuaUIHelper : Find some child Obj fail because baseObj null");
                return default;
            }
            var childObj = selfObj.transform.Find(path);
            var targetComponent = childObj.GetComponent<T>();
            return targetComponent;  }

        public static Text GetText(GameObject obj, string path)
        {
            return GetChild<Text>(obj, path);
        }
        
        public static Image GetImage(GameObject obj, string path)
        {
            return GetChild<Image>(obj, path);
        }
        
        public static RawImage GetRawImage(GameObject obj, string path)
        {
            return GetChild<RawImage>(obj, path);
        }
        
        public static Button GetButton(GameObject obj, string path)
        {
            return GetChild<Button>(obj, path);
        }

        public static Slider GetSlider(GameObject obj, string path)
        {
            return GetChild<Slider>(obj, path);
        }

    }
}
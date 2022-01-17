using AureFramework;
using UnityEngine;
using UnityEngine.UI;

namespace BiuBiu
{
    public static partial class LuaHelper
    {
        public static bool IsEditor() => Application.isEditor;
        
        public static bool IsUIOpen(int uiFormId) => GameMain.UI.IsUIOpen(uiFormId);
        
        public static bool IsUIOpen(string uiName) => GameMain.UI.IsUIOpen(uiName);
        
        public static void OpenUI(int uiFormId, object userData) => GameMain.UI.OpenUI(uiFormId, userData);

        public static void CloseUI(int uiFormId) => GameMain.UI.CloseUI(uiFormId);
        
        public static void CloseUI(string uiName) => GameMain.UI.CloseUI(uiName);

        private static T GetChild<T>(GameObject selfObj, string path) {
            if (selfObj == null) {
                Debug.LogError("UIUtils : Find some child Obj fail because baseObj null");
                return default;
            }

            var childObj = selfObj.transform.Find(path);
            var targetComponent = childObj.GetComponent<T>();
            return targetComponent;
        }

        public static Text GetText(GameObject obj, string path) {
            return GetChild<Text>(obj, path);
        }

        public static Image GetImage(GameObject obj, string path) {
            return GetChild<Image>(obj, path);
        }

        public static RawImage GetRawImage(GameObject obj, string path) {
            return GetChild<RawImage>(obj, path);
        }

        public static Button GetButton(GameObject obj, string path) {
            return GetChild<Button>(obj, path);
        }

        public static Slider GetSlider(GameObject obj, string path) {
            return GetChild<Slider>(obj, path);
        }

        public static UIContentList GetContentList(GameObject obj, string path)
        {
            return GetChild<UIContentList>(obj, path);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace BiuBiu
{
    /// <summary>
    /// Lua调用C# UI相关接口
    /// </summary>
    public static class UIUtils
    {
        public static bool IsEditor() => Application.isEditor;
        
        public static bool IsUIOpen(string uiName) => GameMain.UI.IsUIOpen(uiName);
        
        public static void OpenWindow(string uiName) => GameMain.UI.OpenUI(uiName, "Normal", null);

        public static void CloseWindow(string uiName) => GameMain.UI.CloseUI(uiName);

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
    }
}
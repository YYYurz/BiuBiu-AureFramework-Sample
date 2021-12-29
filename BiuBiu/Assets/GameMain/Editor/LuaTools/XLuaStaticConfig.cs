using System;
using System.Collections.Generic;
using XLua;
using UnityEditor;

/// <summary>
/// XLua静态Wrap配置类
/// </summary>
namespace BiuBiu.Editor
{
    public static class XLuaStaticConfig
    {
        // lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
        [LuaCallCSharp] public static List<Type> LuaCallCSharp = new List<Type>()
        {

            typeof(Object),
            typeof(List<int>),
            typeof(Action<string>),

            #region Unity

            typeof(UnityEngine.Object),
            typeof(UnityEngine.Quaternion),
            typeof(UnityEngine.Color),
            typeof(UnityEngine.Ray),
            typeof(UnityEngine.Bounds),
            typeof(UnityEngine.Ray2D),
            typeof(UnityEngine.Time),
            typeof(UnityEngine.GameObject),
            typeof(UnityEngine.Component),
            typeof(UnityEngine.Behaviour),
            typeof(UnityEngine.Transform),
            typeof(UnityEngine.Resources),
            typeof(UnityEngine.TextAsset),
            typeof(UnityEngine.Keyframe),
            typeof(UnityEngine.AnimationCurve),
            typeof(UnityEngine.Animator),
            typeof(UnityEngine.AnimationClip),
            typeof(UnityEngine.MonoBehaviour),
            typeof(UnityEngine.ParticleSystem),
            typeof(UnityEngine.SkinnedMeshRenderer),
            typeof(UnityEngine.Renderer),
            typeof(UnityEngine.Debug),
            typeof(UnityEngine.CanvasGroup),
            typeof(UnityEngine.UI.InputField),
            typeof(UnityEngine.UI.CanvasScaler),
            typeof(UnityEngine.UI.Toggle),
            typeof(UnityEngine.UI.Text),
            typeof(UnityEngine.UI.Button),
            typeof(UnityEngine.Rect),
            typeof(UnityEngine.Events.UnityEvent<int>),
            typeof(UnityEngine.Events.UnityEvent<string>),
            typeof(UnityEngine.Events.UnityEvent<bool>),
            typeof(UnityEngine.Events.UnityEvent<UnityEngine.Vector2>),
            // typeof(UnityEngine.UI.Graphic),黑名单
            typeof(UnityEngine.RectTransform),
            typeof(UnityEngine.Events.UnityEvent),
            typeof(UnityEngine.Events.UnityEventBase),
            typeof(UnityEngine.UI.Image),
            typeof(UnityEngine.UI.Dropdown),
            typeof(UnityEngine.UI.Dropdown.OptionData),
            typeof(List<UnityEngine.UI.Dropdown.OptionData>),
            typeof(UnityEngine.UI.Dropdown.OptionData),
            typeof(UnityEngine.UI.Slider),
            typeof(UnityEngine.Animation),
            typeof(UnityEngine.ImageConversion),
            typeof(UnityEngine.Texture2D),
            typeof(UnityEngine.Camera),
            typeof(UnityEngine.EventSystems.EventTrigger),
            typeof(UnityEngine.EventSystems.EventTrigger.Entry),
            typeof(UnityEngine.EventSystems.EventTriggerType),
            typeof(UnityEngine.U2D.SpriteAtlas),
            typeof(UnityEngine.UI.ScrollRect),
            typeof(UnityEngine.UI.Selectable),
            typeof(UnityEngine.Vector2),
            typeof(UnityEngine.Events.UnityEvent),
            typeof(UnityEngine.UI.Graphic),

            #endregion

            #region Dotween

            typeof(DG.Tweening.Ease),
            typeof(DG.Tweening.LoopType),
            typeof(DG.Tweening.TweenType),
            typeof(DG.Tweening.PathType),
            typeof(DG.Tweening.UpdateType),
            typeof(DG.Tweening.DOTween),
            typeof(DG.Tweening.EaseFactory),
            typeof(DG.Tweening.Tweener),
            typeof(DG.Tweening.Tween),
            typeof(DG.Tweening.Sequence),
            typeof(DG.Tweening.TweenParams),
            typeof(DG.Tweening.TweenCallback),
            typeof(DG.Tweening.DOTweenModuleUI),
            typeof(DG.Tweening.DOTweenModuleSprite),
            typeof(DG.Tweening.TweenSettingsExtensions),
            typeof(DG.Tweening.ShortcutExtensions),
            typeof(DG.Tweening.TweenExtensions),

            #endregion

            //typeof(Vector2),
            //typeof(Vector3),
            //typeof(Vector4),
            //typeof(Light),            
            //typeof(Mathf),
            //typeof(UnityEngine.UI.Graphic),
            
            typeof(LuaCallStatic),
        };

        [CSharpCallLua] public static List<Type> CSharpCallLua = new List<Type>()
        {
            #region unity

            typeof(UnityEngine.Events.UnityAction),
            typeof(UnityEngine.Events.UnityAction<bool>),
            typeof(UnityEngine.Events.UnityAction<int>),
            typeof(UnityEngine.Events.UnityAction<float>),
            typeof(UnityEngine.Events.UnityAction<UnityEngine.Vector2>),
            typeof(List<UnityEngine.UI.Dropdown.OptionData>),
            typeof(UnityEngine.Events.UnityEvent<int>),

            #endregion

            #region system

            typeof(Action<int, byte[]>),
            typeof(Action<int>),
            typeof(Action<float, float>),

            #endregion
        };

        //黑名单
        [BlackList] public static List<List<string>> BlackList = new List<List<string>>()
        {
            new List<string>() {"System.Xml.XmlNodeList", "ItemOf"},
            new List<string>() {"UnityEngine.WWW", "movie"},
#if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
#endif
            new List<string>() {"UnityEngine.Texture2D", "alphaIsTransparency"},
            new List<string>() {"UnityEngine.Security", "GetChainOfTrustValue"},
            new List<string>() {"UnityEngine.CanvasRenderer", "onRequestRebuild"},
            new List<string>() {"UnityEngine.Light", "areaSize"},
            new List<string>() {"UnityEngine.Light", "lightmapBakeType"},
            new List<string>() {"UnityEngine.WWW", "MovieTexture"},
            new List<string>() {"UnityEngine.WWW", "GetMovieTexture"},
            new List<string>() {"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
#if !UNITY_WEBPLAYER
            new List<string>() {"UnityEngine.Application", "ExternalEval"},
            new List<string>() {"UnityEngine.UI.Graphic", "OnRebuildRequested"},
#endif
            new List<string>() {"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
            new List<string>() {"UnityEngine.Component", "networkView"}, //4.6.2 not support
            new List<string>()
                {"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
            new List<string>() {"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
            new List<string>()
                {"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
            new List<string>()
                {"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
            new List<string>()
            {
                "System.IO.DirectoryInfo", "CreateSubdirectory", "System.String",
                "System.Security.AccessControl.DirectorySecurity"
            },
            new List<string>() {"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
            new List<string>() {"UnityEngine.MonoBehaviour", "runInEditMode"},

            #region MY_BLACKLIST

            new List<string>() {"UnityEngine.UI.Text", "OnRebuildRequested"},

            #endregion
        };

        /// <summary>
        /// 生成Wrap代码后，刷新下
        /// </summary>
        [CSObjectWrapEditor.GenCodeMenu]
        private static void AfterGenWrapCode()
        {
            AssetDatabase.Refresh();
        }
    }
}
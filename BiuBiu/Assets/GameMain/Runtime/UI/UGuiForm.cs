//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;
namespace BB
{
    public abstract class UGuiForm : UIFormLogic
    {
        public const int mDepthFactor = 100;
        private const float mFadeTime = 0.3f;

        private static Font s_MainFont = null;
        private Canvas m_CachedCanvas = null;
        private CanvasGroup m_CanvasGroup = null;

        public int OriginalDepth
        {
            get;
            private set;
        }

        public int Depth
        {
            get
            {
                return m_CachedCanvas.sortingOrder;
            }
        }

        public void PlayUISound(int uiSoundId)
        {
            //GameEntry.Sound.PlaySound(uiSoundId);
        }

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            s_MainFont = mainFont;

            var go = new GameObject();
            go.AddComponent<Text>().font = mainFont;
            Destroy(go);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            m_CachedCanvas.overrideSorting = true;
            OriginalDepth = m_CachedCanvas.sortingOrder;

            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

            var trans = GetComponent<RectTransform>();
            trans.anchorMin = Vector2.zero;
            trans.anchorMax = Vector2.one;
            trans.anchoredPosition = Vector2.zero;
            trans.anchoredPosition3D = Vector3.zero;
            trans.sizeDelta = Vector2.zero;

            gameObject.GetOrAddComponent<GraphicRaycaster>();

            // if (s_MainFont != null)
            // {
            //     var texts = GetComponentsInChildren<Text>(true);
            //     foreach (var i in texts)
            //     {
            //         i.font = s_MainFont;
            //         if (!string.IsNullOrEmpty(i.text))
            //         {
            //             i.text = GameEntry.Localization.GetString(i.text);
            //         }
            //     }
            // }
        }

        // protected override void OnOpen(object userData)
        // {
        //     base.OnOpen(userData);
        //
        //     m_CanvasGroup.alpha = 0f;
        //     var tween = m_CanvasGroup.DOFade(1, mFadeTime);
        //     tween.SetUpdate(true);
        // }
        //
        // protected override void OnClose(bool isShutdown, object userData)
        // {
        //     base.OnClose(isShutdown, userData);
        // }
        //
        // protected override void OnPause()
        // {
        //     base.OnPause();
        // }
        //
        // protected override void OnResume()
        // {
        //     base.OnResume();
        // }
        //
        // protected override void OnCover()
        // {
        //     base.OnCover();
        // }
        //
        // protected override void OnReveal()
        // {
        //     base.OnReveal();
        // }
        //
        // protected override void OnRefocus(object userData)
        // {
        //     base.OnRefocus(userData);
        // }
        //
        // protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        // {
        //     base.OnUpdate(elapseSeconds, realElapseSeconds);
        // }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);

            var oldDepth = Depth;
            var deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + mDepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            var canvases = GetComponentsInChildren<Canvas>(true);
            foreach (var canvas in canvases)
            {
                canvas.sortingOrder += deltaDepth;
            }
        }
    }
}

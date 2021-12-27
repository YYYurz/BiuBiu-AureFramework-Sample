using AureFramework.Event;
using System.Collections.Generic;
using XLua;
using System;

namespace BiuBiu
{
    public sealed class PreloadProgressLoadingEventArgs : AureEventArgs
    {
        /// <summary>
        /// 资源更新改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PreloadProgressLoadingEventArgs).GetHashCode();

        /// <summary>
        /// 已经加载的资源数量
        /// </summary>
        public int LoadedAssetsCount { 
            get; 
            set;
        }

        /// <summary>
        /// 所有资源数量
        /// </summary>
        public int TotalAssetsCount
        {
            get;
            set;
        }

        public PreloadProgressLoadingEventArgs() {
        }


        public static PreloadProgressLoadingEventArgs Create(int nLoadedAssets, int nTotalAssets)
        {
            PreloadProgressLoadingEventArgs assetLoadProgressLoadingEventArgs = ReferencePool.Acquire<PreloadProgressLoadingEventArgs>();
            assetLoadProgressLoadingEventArgs.LoadedAssetsCount = nLoadedAssets;
            assetLoadProgressLoadingEventArgs.TotalAssetsCount = nTotalAssets;

            return assetLoadProgressLoadingEventArgs;
        }

        public override void Clear()
        {
            LoadedAssetsCount = 0;
            TotalAssetsCount = 0;
        }
    }

    public sealed class PreloadProgressCompleteEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源更新改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PreloadProgressCompleteEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public PreloadProgressCompleteEventArgs()
        {
        }

        public static PreloadProgressCompleteEventArgs Create()
        {
            PreloadProgressCompleteEventArgs assetLoadProgressCompleteEventArgs = ReferencePool.Acquire<PreloadProgressCompleteEventArgs>();

            return assetLoadProgressCompleteEventArgs;
        }

        public override void Clear()
        {

        }
    }

    public sealed class PreloadProgressErrorEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源更新改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PreloadProgressErrorEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }



        public PreloadProgressErrorEventArgs()
        {
        }

        public static PreloadProgressErrorEventArgs Create()
        {
            PreloadProgressErrorEventArgs assetLoadProgressErrorEventArgs = ReferencePool.Acquire<PreloadProgressErrorEventArgs>();


            return assetLoadProgressErrorEventArgs;
        }

        public override void Clear()
        {

        }
    }

}

#if UNITY_EDITOR
public static class PreloadProgressLoadingEventArgsExporter
{
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(BiuBiu.PreloadProgressLoadingEventArgs),
        };
}
#endif
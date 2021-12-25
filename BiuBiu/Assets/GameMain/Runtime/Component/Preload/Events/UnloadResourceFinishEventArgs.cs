using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB
{

    public sealed class UnloadResourceFinishEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源更新改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(UnloadResourceFinishEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static UnloadResourceFinishEventArgs Create()
        {
            UnloadResourceFinishEventArgs assetLoadProgressErrorEventArgs = ReferencePool.Acquire<UnloadResourceFinishEventArgs>();
            return assetLoadProgressErrorEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
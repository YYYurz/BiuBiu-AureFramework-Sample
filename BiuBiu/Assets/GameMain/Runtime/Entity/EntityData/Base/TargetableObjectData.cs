using System;
using UnityEngine;

namespace BiuBiu
{
    [Serializable]
    public abstract class TargetableObjectData : EntityData
    {
        protected TargetableObjectData(int entityId, int typeId)
            : base(entityId, typeId) { }

        /// <summary>
        /// 当前生命。
        /// </summary>
        public abstract float CurHealth
        {
            get;
            set;
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public abstract float MaxHealth
        {
            get;
        }

        /// <summary>
        /// 生命百分比。
        /// </summary>
        public float HpRatio => MaxHealth > 0 ? CurHealth / MaxHealth : 0f;
    }
}

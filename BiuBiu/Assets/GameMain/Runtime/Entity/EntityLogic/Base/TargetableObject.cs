using UnityEngine;
using UnityGameFramework.Runtime;

namespace BiuBiu
{
    /// <summary>
    /// 可作为目标的实体类。
    /// </summary>
    public abstract class TargetableObject : Entity
    {
        [SerializeField]
        private TargetableObjectData targetableObjectData = null;

        public bool IsDead => targetableObjectData.CurHealth <= 0;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            targetableObjectData = userData as TargetableObjectData;
            if (targetableObjectData != null) return;
            Log.Error("Targetable object data is invalid.");
        }
    }
}

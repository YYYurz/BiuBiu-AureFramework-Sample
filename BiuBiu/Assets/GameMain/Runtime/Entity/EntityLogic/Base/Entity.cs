using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BiuBiu
{
    public abstract class Entity : EntityLogic
    {
        [SerializeField]
        private EntityData entityData = null;

        public int Id => Entity.Id;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityData = userData as EntityData;
            if (entityData == null)
            {
                Log.Error("Entity data is invalid.");
                return;
            }
            CachedTransform.localPosition = entityData.Position;
            CachedTransform.localRotation = entityData.Rotation;
            CachedTransform.localScale = Vector3.one;
        }
    }
}

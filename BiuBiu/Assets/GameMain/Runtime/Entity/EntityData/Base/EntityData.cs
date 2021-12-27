using GameFramework;
using System;
using UnityEngine;

namespace BiuBiu
{
    [Serializable]
    public abstract class EntityData : IReference
    {
        [SerializeField]
        private int id;

        [SerializeField]
        private int typeId;

        [SerializeField]
        private Vector3 position = Vector3.zero;

        [SerializeField]
        private Quaternion rotation = Quaternion.identity;

        [SerializeField]
        private string mAssetPath;

        protected EntityData(int entityId, int typeId)
        {
            id = entityId;
            this.typeId = typeId;
        }

         /// <summary>
        /// 实体编号。
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// 实体类型编号。
        /// </summary>
        public int TypeId
        {
            get => typeId;
            set => typeId = value;
        }

        /// <summary>
        /// 实体位置。
        /// </summary>
        public Vector3 Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// 实体朝向。
        /// </summary>
        public Quaternion Rotation
        {
            get => rotation;
            set => rotation = value;
        }

        public string AssetPath
        {
            get => mAssetPath;
            set => mAssetPath = value;
        }

        public virtual void Clear()
        {
            id = 0;
            typeId = 0;
            position = Vector3.zero;
            rotation = Quaternion.identity;
            mAssetPath = default;
        }
    }
}

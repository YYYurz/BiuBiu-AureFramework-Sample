using System;
using UnityEngine;

namespace BiuBiu
{
    [Serializable]
    public abstract class AccessoryObjectData : EntityData
    {
        [SerializeField]
        private int m_OwnerId = 0;
        
        [SerializeField]
        // private CampType m_OwnerCamp = CampType.Unknown;

        public AccessoryObjectData(int entityId, int typeId, int ownerId)
            : base(entityId, typeId)
        {
            m_OwnerId = ownerId;
        }

        /// <summary>
        /// 拥有者编号。
        /// </summary>
        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }
    }
}

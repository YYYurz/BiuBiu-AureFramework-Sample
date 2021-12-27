using GameFramework;
using System;
using UnityEngine;

namespace BiuBiu
{
    [Serializable]
    public class CommonEntityData : EntityData
    {
        public float DelayDestroyTime { get; set; } = 3.0f;

        public CommonEntityData() : base(0, 0)
        {
        }

        public static CommonEntityData Create(int nEntityId, float fDelayTime, Vector3 pos, Quaternion oriention, string strAssetPath)
        {
            var entityData = ReferencePool.Acquire<CommonEntityData>();
            entityData.Id = nEntityId;
            entityData.DelayDestroyTime = fDelayTime;
            entityData.TypeId = 0;
            entityData.Position = pos;
            entityData.Rotation = oriention;
            entityData.AssetPath = strAssetPath;

            return entityData;
        }
    }

}

//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BiuBiu
{
    public class CommonEntityLogic : Entity
    {
        public CommonEntityData EntityData { get; set; } = null;

        public float fDelayTimeCount = 0;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            EntityData = userData as CommonEntityData;
            if (EntityData == null)
            {
                Log.Error("CommonEntityData data is invalid.");
                return;
            }
            fDelayTimeCount = EntityData.DelayDestroyTime;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (fDelayTimeCount > 0)
            {
                fDelayTimeCount -= elapseSeconds;
                if (fDelayTimeCount <= 0)
                {
                    GameMain.Entity.HideEntity(Entity.Id);
                }
            }
        }

    }
}

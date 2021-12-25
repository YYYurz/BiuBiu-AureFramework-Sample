using System;
using GameConfig;
using UnityGameFramework.Runtime;

namespace BB
{
    public static class EntityExtension
    {
        // EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int SerialId;

        // public static Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        // {
        //     var entity = entityComponent.GetEntity(entityId);
        //     if (entity == null)
        //     {
        //         return null;
        //     }
        //     return (Entity)entity.Logic;
        // }
 
        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

        public static void ShowSoldier(this EntityComponent entityComponent)
        {
            var data = new CharacterData(GenerateSerialId(), 10001);
            entityComponent.ShowEntity(typeof(SoldierLogic), "Player", Constant.AssetPriority.EntityAsset, data);
        }

        public static void ShowEffect(this EntityComponent entityComponent, EntityData data)
        {
            entityComponent.ShowEntity(data.Id, typeof(CommonEntityLogic), data.AssetPath, "Effect", Constant.AssetPriority.EffectAsset, data);
        }

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }
            DTEntity? entityInfo = GameEntry.TableData.DataTableInfo.GetDataTableReader<DTEntityTableReader>().GetInfo((uint)data.TypeId);
            entityComponent.ShowEntity(data.Id, logicType, AssetUtils.GetEntityAsset(entityInfo.Value.AssetName), entityGroup, priority, data);
        }

        private static int GenerateSerialId()
        {
            return --SerialId;
        }
    }
}


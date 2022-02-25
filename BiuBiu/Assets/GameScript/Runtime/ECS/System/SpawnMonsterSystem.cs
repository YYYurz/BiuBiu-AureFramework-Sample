//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace BiuBiu
{
	/// <summary>
	/// 创建怪物
	/// </summary>
	public class SpawnMonsterSystem : ComponentSystem
	{
		private const int MaxMonsterCount = 1000;
		private int curMonsterCount;

		protected override void OnUpdate()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}

			while (++curMonsterCount <= MaxMonsterCount)
			{
				var createEntityFromAddressableSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<CreateEntityFromAddressableSystem>();
				createEntityFromAddressableSystem.CreateEntity(Constant.EntityId.Monster, OnCreateMonsterEntitySuccess);
			}
		}

		private void OnCreateMonsterEntitySuccess(Entity monsterEntity)
		{
			var position =  new float3(Random.Range(-15f, 15f), 1f, Random.Range(-15f, 15f));
			EntityManager.SetComponentData(monsterEntity, new Translation{ Value = position });
			EntityManager.AddBuffer<PathPositionBuffer>(monsterEntity);		
		}
	}
}
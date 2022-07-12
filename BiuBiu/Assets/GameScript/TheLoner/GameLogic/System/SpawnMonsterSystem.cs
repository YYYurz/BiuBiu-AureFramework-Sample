//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace TheLoner
{
	/// <summary>
	/// 创建怪物
	/// </summary>
	public class SpawnMonsterSystem : ComponentSystem
	{
		private const int MaxMonsterCount = 500;
		private const float SpawnMonsterInterval = 1f;
		private float spawnTimer;

		private EntityQuery monsterQuery;
		private CreateEntityFromAddressableSystem createEntityFromAddressableSystem;

		protected override void OnCreate()
		{
			base.OnCreate();

			monsterQuery = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<MonsterDataComponent>());
			createEntityFromAddressableSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<CreateEntityFromAddressableSystem>();
		}

		protected override void OnUpdate()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}
			
			spawnTimer += UnityEngine.Time.deltaTime;
			if (spawnTimer > SpawnMonsterInterval && monsterQuery.CalculateEntityCount() < MaxMonsterCount)
			{
				var monsterEntity = createEntityFromAddressableSystem.InstantiateEntity(Constant.EntityId.Monster);
				var position = new float3(Random.Range(-23f, 15f), 1f, Random.Range(-15f, 15f));
				EntityManager.SetComponentData(monsterEntity, new Translation {Value = position});
				EntityManager.SetComponentData(monsterEntity, new PhysicsVelocity() {Linear = float3.zero}); // 受力归零，防止起飞
				EntityManager.AddBuffer<PathPositionBuffer>(monsterEntity);
			}
		}
	}
}
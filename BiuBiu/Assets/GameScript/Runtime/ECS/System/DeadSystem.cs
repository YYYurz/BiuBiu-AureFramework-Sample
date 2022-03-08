//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BiuBiu
{
	/// <summary>
	/// 处理死亡的怪物
	/// </summary>
	public class DeadSystem : ComponentSystem
	{
		private CreateEntityFromAddressableSystem createEntityFromAddressableSystem;

		protected override void OnCreate()
		{
			base.OnCreate();

			createEntityFromAddressableSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<CreateEntityFromAddressableSystem>();
		}

		protected override void OnUpdate()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}
			
			var deadEntityList = new NativeList<Entity>(Allocator.Temp);
			var translationList = new NativeList<Translation>(Allocator.Temp);
			Entities.ForEach((Entity monsterEntity, ref MonsterDataComponent monsterDataComponent, ref Translation translation) =>
			{
				if (monsterDataComponent.Health <= 0)
				{
					deadEntityList.Add(monsterEntity);
					translationList.Add(translation);
				}
			});

			if (deadEntityList.Length > 0)
			{
				for (var i = 0; i < deadEntityList.Length; i++)
				{
					var entity = deadEntityList[i];
					var translation = translationList[i];
					GameMain.Effect.PlayEffect("Explosion", translation.Value, quaternion.identity);
					createEntityFromAddressableSystem.DestroyEntity(entity);
				}
				
				GameMain.Sound.PlaySound(1007u, 0f);
			}
			
			deadEntityList.Dispose();
			translationList.Dispose();
		}
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Collections;
using Unity.Entities;

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
			
			Entities.ForEach((Entity monsterEntity, ref MonsterDataComponent monsterDataComponent) =>
			{
				if (monsterDataComponent.Health <= 0)
				{
					deadEntityList.Add(monsterEntity);
				}
			});

			foreach (var entity in deadEntityList)
			{
				createEntityFromAddressableSystem.DestroyEntity(entity);
			}

			deadEntityList.Dispose();
		}
	}
}
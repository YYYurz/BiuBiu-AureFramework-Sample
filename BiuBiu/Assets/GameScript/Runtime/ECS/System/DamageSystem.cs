//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BiuBiu
{
	/// <summary>
	/// 伤害计算系统
	/// </summary>
	public class DamageSystem : ComponentSystem
	{
		[BurstCompile]
		private struct DamageTask
		{
			public float3 Position;
			public float Radius;
			public float Damage;
		}
		
		private NativeList<DamageTask> damageTaskList = new NativeList<DamageTask>(Allocator.Persistent);
		
		protected override void OnUpdate()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}

			var hitMonsterCount = 0;
			Entities.ForEach((Entity monsterEntity, ref MonsterDataComponent monsterDataComponent, ref Translation translation) =>
			{
				foreach (var damageTask in damageTaskList)
				{
					if (math.distance(translation.Value, damageTask.Position) <= damageTask.Radius)
					{
						var health = monsterDataComponent.Health - damageTask.Damage;
						monsterDataComponent.Health = health < 0 ? 0 : health;
						hitMonsterCount++;
					}		
				}
			});

			if (hitMonsterCount > 0)
			{
				GameMain.Event.Fire(this, HitMonsterEventArgs.Create(hitMonsterCount));
			}
			
			damageTaskList.Clear();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (damageTaskList.IsCreated)
			{
				damageTaskList.Dispose();
			}
		}

		/// <summary>
		/// 创建一次伤害
		/// </summary>
		/// <param name="position"> 位置 </param>
		/// <param name="radius"> 半径 </param>
		/// <param name="damage"> 伤害 </param>
		public void CreateDamageTask(float3 position, float radius, float damage)
		{
			var damageTask = new DamageTask
			{
				Position = position,
				Radius = radius,
				Damage = damage,
			};
			
			damageTaskList.Add(damageTask);
		}
	}
}
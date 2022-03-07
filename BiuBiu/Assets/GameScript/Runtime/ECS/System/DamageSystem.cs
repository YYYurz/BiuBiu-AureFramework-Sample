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
using UnityEngine;

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

			var hitMonsterTranslationList = new NativeList<float3>(Allocator.Temp);
			Entities.ForEach((Entity monsterEntity, ref MonsterDataComponent monsterDataComponent, ref Translation translation) =>
			{
				foreach (var damageTask in damageTaskList)
				{
					if (math.distance(translation.Value, damageTask.Position) <= damageTask.Radius)
					{
						var health = monsterDataComponent.Health - damageTask.Damage;
						monsterDataComponent.Health = health < 0 ? 0 : health;
						hitMonsterTranslationList.Add(translation.Value);
					}		
				}
			});

			if (hitMonsterTranslationList.Length > 0)
			{
				var playerPos = GameMain.GamePlay.PlayerController.transform.position;
				foreach (var hitPosition in hitMonsterTranslationList)
				{
					var direction = (Vector3) hitPosition - playerPos;
					var rotation = Quaternion.LookRotation(direction);
					GameMain.Effect.PlayEffect("Hit", hitPosition + new float3(0f, 1.5f, 0f), rotation);
					GameMain.Sound.PlaySound(1013u, 0.3f);
				}
				
				GameMain.Event.Fire(this, HitMonsterEventArgs.Create(false));
			}

			hitMonsterTranslationList.Dispose();
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
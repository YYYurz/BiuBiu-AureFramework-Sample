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
		/// <summary>
		/// 圆形打击盒
		/// </summary>
		[BurstCompile]
		private struct CircleDamage
		{
			public float3 Position;
			public float Radius;
			public float Damage;
		}
		
		/// <summary>
		/// 四边形打击盒
		/// </summary>
		[BurstCompatible]
		private struct QuadrilateralDamage
		{
			public float2 Vertex1;
			public float2 Vertex2;
			public float2 Vertex3;
			public float2 Vertex4;
			public float Damage;
		}
		
		private NativeList<CircleDamage> circleDamageList = new NativeList<CircleDamage>(Allocator.Persistent);
		private NativeList<QuadrilateralDamage> quadrilateralList = new NativeList<QuadrilateralDamage>(Allocator.Persistent);
		
		protected override void OnUpdate()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}

			var hitMonsterTranslationList = new NativeList<float3>(Allocator.Temp);
			Entities.ForEach((Entity monsterEntity, ref MonsterDataComponent monsterDataComponent, ref Translation translation) =>
			{
				foreach (var circleDamage in circleDamageList)
				{
					if (GameUtils.PointInCircle(translation.Value, circleDamage.Position, circleDamage.Radius))
					{
						var health = monsterDataComponent.Health - circleDamage.Damage;
						monsterDataComponent.Health = health < 0 ? 0 : health;
						hitMonsterTranslationList.Add(translation.Value);
					}
				}

				foreach (var quadrilateralDamage in quadrilateralList)
				{
					var polygonVertexList = new NativeList<float2>(Allocator.Temp)
					{
						quadrilateralDamage.Vertex1,
						quadrilateralDamage.Vertex2,
						quadrilateralDamage.Vertex3,
						quadrilateralDamage.Vertex4,
					};
					var position = new float2(translation.Value.x, translation.Value.z);
					
					if (GameUtils.PointInPolygon(position, polygonVertexList))
					{
						var health = monsterDataComponent.Health - quadrilateralDamage.Damage;
						monsterDataComponent.Health = health < 0 ? 0 : health;
						hitMonsterTranslationList.Add(translation.Value);
					}

					polygonVertexList.Dispose();
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
				}
				
				GameMain.Sound.PlaySound(1013u, 0.3f);
				GameMain.Event.Fire(this, HitMonsterEventArgs.Create(false));
			}

			hitMonsterTranslationList.Dispose();
			circleDamageList.Clear();
			quadrilateralList.Clear();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (circleDamageList.IsCreated)
			{
				circleDamageList.Dispose();
			}
			
			if (quadrilateralList.IsCreated)
			{
				quadrilateralList.Dispose();
			}
		}

		/// <summary>
		/// 创建一次圆形打击
		/// </summary>
		/// <param name="position"> 位置 </param>
		/// <param name="radius"> 半径 </param>
		/// <param name="damage"> 伤害 </param>
		public void CreateCircleDamage(float3 position, float radius, float damage)
		{
			var circleDamage = new CircleDamage
			{
				Position = position,
				Radius = radius,
				Damage = damage,
			};
			
			circleDamageList.Add(circleDamage);
		}

		/// <summary>
		/// 创建一次四边形打击
		/// </summary>
		/// <param name="v1"> 顶点1 </param>
		/// <param name="v2"> 顶点2 </param>
		/// <param name="v3"> 顶点3 </param>
		/// <param name="v4"> 顶点4 </param>
		/// <param name="damage"> 伤害 </param>
		public void CreateQuadrilateralDamage(float2 v1, float2 v2, float2 v3, float2 v4, float damage)
		{
			var quadrilateralDamage = new QuadrilateralDamage
			{
				Vertex1 = v1,
				Vertex2 = v2,
				Vertex3 = v3,
				Vertex4 = v4,
				Damage = damage,
			};
			
			quadrilateralList.Add(quadrilateralDamage);
		}
	}
}
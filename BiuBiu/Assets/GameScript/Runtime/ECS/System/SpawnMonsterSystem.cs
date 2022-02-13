//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BiuBiu
{
	/// <summary>
	/// 创建怪物
	/// </summary>
	public class SpawnMonsterSystem : ComponentSystemBase
	{
		private const int MaxMonsterCount = 10;
		private int curMonsterCount = 0;
		private EntityArchetype monsterArchetype;

		protected override void OnCreate()
		{
			base.OnCreate();
			
			monsterArchetype = EntityManager.CreateArchetype(
				typeof(Translation),
				typeof(LocalToWorld),
				typeof(Rotation),
				typeof(RenderMesh),
				typeof(RenderBounds),
				typeof(Animation)
			);
		}

		public override void Update()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}

			if (curMonsterCount < MaxMonsterCount)
			{
				CreateMonster();
				curMonsterCount++;
			}
		}

		private void CreateMonster()
		{
			var monsterEntity = EntityManager.CreateEntity(monsterArchetype);
			var position =  new float3(Random.Range(-15f, 15f), 1f, Random.Range(-15f, 15f));
			var rotation = new quaternion(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
			EntityManager.SetComponentData(monsterEntity, new Translation{ Value = position });
			EntityManager.SetComponentData(monsterEntity, new Rotation{ Value = rotation });
			EntityManager.SetSharedComponentData(monsterEntity, new RenderMesh
			{
				mesh = GameMain.Resource.LoadAssetSync<Mesh>("Me"),
				material = GameMain.Resource.LoadAssetSync<Material>("Mat")
			});
		}
	}
}
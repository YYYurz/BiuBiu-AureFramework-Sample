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
		private GameObject monster;
		private const int MaxMonsterCount = 10;
		private int curMonsterCount;

		public override void Update()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}

			if (monster == null)
			{
				monster = GameMain.Resource.LoadAssetSync<GameObject>("Monster");
			}

			while (curMonsterCount < MaxMonsterCount)
			{
				CreateMonster();
				curMonsterCount++;
			}
		}
		

		private void CreateMonster()
		{
			var monsterEntity = GameMain.Entity.CreateEntity(monster, "Monster");
			var position =  new float3(Random.Range(-15f, 15f), 1f, Random.Range(-15f, 15f));
			position = PathfindingUtils.GetNearestEffectivePosInMap(GameMain.GamePlay.CurMapConfig, position);
			EntityManager.SetComponentData(monsterEntity, new Translation{ Value = position });
			EntityManager.SetComponentData(monsterEntity, new PathFollowComponent{ PathIndex = -1 });
			EntityManager.SetComponentData(monsterEntity, new MonsterDataComponent
			{
				Health = 100f,
				AttackDamage = 1f,
				MoveSpeed = 3f,
			});
			EntityManager.SetSharedComponentData(monsterEntity, new RenderMesh
			{
				mesh = GameMain.Resource.LoadAssetSync<Mesh>("TestMesh"),
				material = GameMain.Resource.LoadAssetSync<Material>("TestMat")
			});
			
			EntityManager.AddBuffer<PathPositionBuffer>(monsterEntity);

			// var mo = GameMain.Resource.InstantiateSync("Monster");
			// var position =  new Vector3(Random.Range(-23f, 23f), 1f, Random.Range(-23f, 23f));
			// mo.transform.position = position;
		}
	}
}
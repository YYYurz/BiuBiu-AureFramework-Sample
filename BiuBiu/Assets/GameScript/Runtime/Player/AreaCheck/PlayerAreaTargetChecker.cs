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
using UnityEngine;

namespace BiuBiu
{
	public class PlayerAreaTargetChecker : MonoBehaviour
	{
		private EntityManager entityManager;
		private EntityQuery entityQuery;
		private Entity curTargetEntity;
		private bool isLock;
		private float lockTimer;

		[SerializeField] private float lockTime;
		[SerializeField] private float radius;

		private void Awake()
		{
			entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		private void Start()
		{
			var queryDescription = new EntityQueryDesc
			{
				All = new[]{ 
					ComponentType.ReadOnly<MonsterDataComponent>(),
					ComponentType.ReadOnly<Translation>() }
			};
			entityQuery = entityManager.CreateEntityQuery(queryDescription);
		}

		private void Update()
		{
			if (isLock)
			{
				lockTimer += Time.deltaTime;
				if (lockTimer > lockTime)
				{
					isLock = false;
				}
			}
		}

		/// <summary>
		/// 获取范围内最近的目标位置
		/// </summary>
		/// <returns></returns>
		public bool TryGetAreaNearestTarget(out Vector3 targetPosition)
		{
			while (true)
			{
				targetPosition = Vector3.zero;
				if (isLock && entityManager.Exists(curTargetEntity))
				{
					var translation = entityManager.GetComponentData<Translation>(curTargetEntity);
					if (math.distance(translation.Value, transform.position) <= radius)
					{
						targetPosition = translation.Value;
						return true;
					}

					isLock = false;
					continue;
				}

				var monsterEntityArray = entityQuery.ToEntityArray(Allocator.Temp);
				var tempDistance = float.MaxValue;
				foreach (var monsterEntity in monsterEntityArray)
				{
					var translation = entityManager.GetComponentData<Translation>(monsterEntity);
					var monsterPosition = translation.Value;
					var distance = math.distance(transform.position, monsterPosition);
					if (distance < tempDistance)
					{
						curTargetEntity = monsterEntity;
						tempDistance = distance;
					}
				}

				monsterEntityArray.Dispose();

				if (tempDistance <= radius)
				{
					var translation = entityManager.GetComponentData<Translation>(curTargetEntity);
					targetPosition = translation.Value;
					return true;
				}

				return false;
			}
		}
		
		/// <summary>
		/// 锁定当前目标（刷新锁定时长）
		/// </summary>
		public void LockCurrentTarget()
		{
			lockTimer = 0f;

			if (entityManager.Exists(curTargetEntity))
			{
				var translation = entityManager.GetComponentData<Translation>(curTargetEntity);
				if (math.distance(translation.Value, transform.position) <= radius)
				{
					isLock = true;
					return;
				}
			}

			isLock = false;
		}

		/// <summary>
		/// 解除当前锁定
		/// </summary>
		public void Unlock()
		{
			lockTimer = 0f;
			isLock = false;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position, radius);
		}
#endif
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 每帧刷新自动寻路单位的寻路路径
	/// </summary>
	public class PathfindingSystem : ComponentSystemBase
	{
		private struct PointInformation
		{
			public float WorldPosX;
			public float WorldPosZ;
			
			public float G;
			public float H;
			public float F;
			
			public int Parent;

			public bool IsWalkable;
		}
		
		private readonly Dictionary<uint, List<float3>> pathDic = new Dictionary<uint, List<float3>>();
		private int targetIndex;

		public override void Update()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}

			RefreshTargetPos();
			var pointInformationArray = GetMapPointInformationArray(); 
			var entityArray = GetMovablePositionComponentArray();
			var jobHandleList = new NativeList<JobHandle>(Allocator.TempJob);
			var curMapConfig = GameMain.GamePlay.CurMapConfig;

			var startTime = UnityEngine.Time.realtimeSinceStartup;
			foreach (var entity in entityArray)
			{
				var positionComponent = EntityManager.GetComponentData<PositionComponent>(entity);
				var startIndex = PathfindingUtils.GetIndexByWorldPosition(positionComponent.CurPosition, curMapConfig.CellSize, curMapConfig.MapSize);
				// Debug.Log(positionComponent.CurPosition);

				if (!(startIndex >= 0 && startIndex < curMapConfig.PointArray.Length)
				    || !(targetIndex >= 0 && targetIndex < curMapConfig.PointArray.Length)
				    || curMapConfig.PointArray[startIndex].index == 0
				    || curMapConfig.PointArray[targetIndex].index == 0)
				{
					continue;
				}
				
				var pathFindingJob = new PathfindingJob
				{
					PointInformationArray = pointInformationArray,
					start = startIndex,
					end = targetIndex,
					cellSize = curMapConfig.CellSize,
					mapSize = curMapConfig.MapSize,
				};
			
				jobHandleList.Add(pathFindingJob.Schedule());
			}
			JobHandle.CompleteAll(jobHandleList);
			
			Debug.Log((UnityEngine.Time.realtimeSinceStartup - startTime));
			
			// foreach (var entity in entityArray)
			// {
			// 	var positionComponent = EntityManager.GetComponentData<PositionComponent>(entity);
			// 	var a = pathResult[positionComponent.Id];
			// 	var f = GameMain.GamePlay.CurMapConfig.PointList.Find((point => point.key.Equals(a[0]))).value;
			// 	var e = GameMain.GamePlay.CurMapConfig.PointList.Find((point => point.key.Equals(a[1]))).value;
			// 	EntityManager.SetComponentData(entity, new PositionComponent
			// 	{
			// 		CurPosition = f,
			// 		NextPosition = e
			// 	});
			// }
			// foreach (var job in jobList)
			// {
			// 	Debug.Log(job.pathResultList[0] + "  " + job.pathResultList[1]);
			// }

			pointInformationArray.Dispose();
			entityArray.Dispose();
			jobHandleList.Dispose();
		}

		private void RefreshTargetPos()
		{
			var playerPos = GameMain.GamePlay.PlayerController.transform.position;
			var cellSize = GameMain.GamePlay.CurMapConfig.CellSize;
			var mapSize = GameMain.GamePlay.CurMapConfig.MapSize;
			targetIndex = PathfindingUtils.GetIndexByWorldPosition(playerPos, cellSize, mapSize);
		}

		private NativeArray<PointInformation> GetMapPointInformationArray()
		{
			var mapConfig = GameMain.GamePlay.CurMapConfig;
			var pointInformationArray = new NativeArray<PointInformation>(GameMain.GamePlay.CurMapConfig.PointArray.Length, Allocator.TempJob);
			for (var i = 0; i < pointInformationArray.Length; i++)
			{
				var point = mapConfig.PointArray[i];
				if (point.index != 0)
				{
					pointInformationArray[i] = new PointInformation
					{
						WorldPosX = point.worldPos.x,
						WorldPosZ = point.worldPos.z,
						IsWalkable = true,
					};
				}
			}

			return pointInformationArray;
		}

		private NativeArray<Entity> GetMovablePositionComponentArray()
		{
			var queryDescription = new EntityQueryDesc
			{
				All = new ComponentType[]
				{
					typeof(PositionComponent),
					typeof(Translation),
				},
				None = new ComponentType[]
				{
					typeof(ControlBuffComponent)
				},
			};
			
			var query = GetEntityQuery(queryDescription);
			return query.ToEntityArray(Allocator.Temp);
		}

		/// <summary>
		/// 寻路Job
		/// </summary>
		[BurstCompile]
		private struct PathfindingJob : IJob
		{
			[ReadOnly] public NativeArray<PointInformation> PointInformationArray;
			public float cellSize;
			public float mapSize;
			public int start;
			public int end;
			private int processingPos;

			public void Execute()
			{
				var pointInformationArray = new NativeArray<PointInformation>(PointInformationArray.Length, Allocator.Temp);
				PointInformationArray.CopyTo(pointInformationArray);
				
				var openList = new NativeList<int>(Allocator.Temp);
				var closeList = new NativeList<int>(Allocator.Temp);
				openList.Add(start);

				while (openList.Length > 0)
				{
					SelectMinCostF(pointInformationArray, openList, closeList);
					RefreshSurroundPoints(pointInformationArray, openList, closeList);
					if (openList.Contains(end))
					{
						// CreatePath();
						break;
					}
				}

				openList.Dispose();
				closeList.Dispose();
				pointInformationArray.Dispose();
			}

			/// <summary>
			/// 选择F值最小的节点作为当前处理节点
			/// </summary>
			private void SelectMinCostF(NativeArray<PointInformation> pointInformationArray, NativeList<int> openList, NativeList<int> closeList)
			{
				var key = 0;
				for (var i = 0; i < openList.Length; i++)
				{
					if (pointInformationArray[openList[i]].F < pointInformationArray[openList[key]].F)
					{
						key = i;
					}
				}

				processingPos = openList[key];
				closeList.Add(openList[key]);
				openList.RemoveAt(key);
			}

			/// <summary>
			/// 刷新当前点的周围点信息
			/// </summary>
			private void RefreshSurroundPoints(NativeArray<PointInformation> pointInformationArray, NativeList<int> openList, NativeList<int> closeList)
			{
				for (var x = -cellSize; x <= cellSize; x += cellSize)
				{
					for (var z = -cellSize; z <= cellSize; z += cellSize)
					{
						if (x.Equals(0f) && z.Equals(0f))
						{
							continue;
						}
			
						var processingPointInformation = pointInformationArray[processingPos];
						var posX = processingPointInformation.WorldPosX + x;
						var posZ = processingPointInformation.WorldPosZ + z;
						var index = PathfindingUtils.GetIndexByWorldPosition(new float3(posX, 0f, posZ), cellSize, mapSize);
						if (!(index >= 0 && index < pointInformationArray.Length) || !pointInformationArray[index].IsWalkable || closeList.Contains(index))
						{
							continue;
						}
			
						var costG = Mathf.Abs(x).Equals(Mathf.Abs(z)) ? processingPointInformation.G + 14 : processingPointInformation.G + 10;
						var costH = (Mathf.Abs(posX - pointInformationArray[end].WorldPosX) + Mathf.Abs(posZ - pointInformationArray[end].WorldPosZ)) * 10;
						var costF = costG + costH;

						var pointInformation = pointInformationArray[index];
						var newPointInformation = new PointInformation
						{
							WorldPosX = pointInformation.WorldPosX,
							WorldPosZ = pointInformation.WorldPosZ,
							
							G = costG,
							H = costH,
							F = costF,
							Parent = processingPos,
							
							IsWalkable = true,
						};

						if (openList.Contains(index))
						{
							if (pointInformationArray[index].G > costG)
							{
								pointInformationArray[index] = newPointInformation;
							}
						}
						else
						{
							pointInformationArray[index] = newPointInformation;
							openList.Add(index);
						}
					}
				}
			}

			// private void CreatePath()
			// {
			// 	var tempInformation = PointInformationArray[End];
			// 	var tempIndex = End;
			//
			// 	PathResultList.Add(End);
			// 	while (!tempIndex.Equals(Start))
			// 	{
			// 		tempIndex = tempInformation.Parent;
			// 		tempInformation = PointInformationArray[tempIndex];
			// 		PathResultList.Add(tempIndex);
			// 	}
			// }
		}
	}
}
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
using Unity.Collections.LowLevel.Unsafe;
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
			public int g;
			public int h;
			public int f;
			public int2 parent;
		}
		
		private readonly Dictionary<uint, List<float3>> pathDic = new Dictionary<uint, List<float3>>();
		private int2 targetPos;

		public override void Update()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}

			var mapConfig = GameMain.GamePlay.CurMapConfig;
			// var mapPointArray = new NativeArray<PointInformation>(mapConfig.PointArray.Count, Allocator.Temp); 

			var entityArray = GetMovablePositionComponentArray();
			var positionComponentArray = new NativeArray<PositionComponent>();
			
			var jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
			var jobList = new List<PathfindingJob>();
			var mapCellSize = GameMain.GamePlay.CurMapConfig.CellSize;
			foreach (var entity in entityArray)
			{
				var positionComponent = EntityManager.GetComponentData<PositionComponent>(entity);
				var start = PathfindingUtils.GetIndexByWorldPosition(positionComponent.CurPosition.x, positionComponent.CurPosition.z, mapCellSize);
				var pathFindingJob = new PathfindingJob(start, targetPos);
				jobList.Add(pathFindingJob);
				jobHandleList.Add(pathFindingJob.Schedule());
			}
			JobHandle.CompleteAll(jobHandleList);
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
			
			jobHandleList.Dispose();
		}

		private void RefreshTargetPos()
		{
			var playerPos = GameMain.GamePlay.PlayerController.transform.position;
			var mapCellSize = GameMain.GamePlay.CurMapConfig.CellSize;
			targetPos = PathfindingUtils.GetIndexByWorldPosition(playerPos.x, playerPos.z, mapCellSize);
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
			

			private NativeList<int2> pathResultList;
			private int2 startPos;
			private int2 endPos;
			
			private NativeHashMap<int2, float3> pointDic;
			private NativeHashMap<int2, PointInformation> openDic;
			private NativeHashMap<int2, PointInformation> closeDic;
			private int2 processingPos;

			public PathfindingJob(int2 start, int2 end)
			{
				pathResultList = new NativeList<int2>(Allocator.TempJob);
				pointDic = new NativeHashMap<int2, float3>();
				openDic = new NativeHashMap<int2, PointInformation>();
				closeDic = new NativeHashMap<int2, PointInformation>();
				startPos = start;
				endPos = end;
				processingPos = int2.zero;
			}

			public void Execute()
			{
				openDic.Add(startPos, new PointInformation
				{
					g = 0,
					h = 0,
					f = 0,
					parent = startPos,
				});
				
				while (!openDic.IsEmpty)
				{
					SelectMinCostF();
					RefreshSurroundPoints();
					if (openDic.ContainsKey(endPos))
					{
						CreatePath();
						break;
					}
				}
			}

			/// <summary>
			/// 选择F值最小的节点作为当前处理节点
			/// </summary>
			private void SelectMinCostF()
			{
				var tempPoint = new KeyValue<int2, PointInformation>();
				var tempF = int.MaxValue;
				foreach (var openPoint in openDic)
				{
					if (openPoint.Value.f < tempF)
					{
						tempPoint = openPoint;
						tempF = openPoint.Value.f;
					}
				}

				processingPos = tempPoint.Key;
				openDic.Remove(tempPoint.Key);
				closeDic.Add(tempPoint.Key, tempPoint.Value);
			}

			/// <summary>
			/// 刷新当前点的周围点信息
			/// </summary>
			private void RefreshSurroundPoints()
			{
				for (var x = -1; x <= 1; x++)
				{
					for (var y = -1; y <= 1; y++)
					{
						if (x == 0 && y == 0)
						{
							continue;
						}

						var processingPointInformation = closeDic[processingPos];
						var posX = processingPos.x + x;
						var posY = processingPos.y + y;
						var pos = new int2(posX, posY);
						if (!pointDic.ContainsKey(pos) || closeDic.ContainsKey(pos))
						{
							continue;
						}

						var costG = Mathf.Abs(x) == Mathf.Abs(y)
							? processingPointInformation.g + 14
							: processingPointInformation.g + 10;
						var costH = (Mathf.Abs(posX - endPos.x) + Mathf.Abs(posY - endPos.y)) * 10;

						var costF = costG + costH;
						var pointInformation = new PointInformation
						{
							g = costG,
							h = costH,
							f = costF,
							parent = processingPos,
						};
						AddToOpen(pos, pointInformation);
					}
				}
			}

			/// <summary>
			/// 添加点到OpenDic
			/// </summary>
			/// <param name="pos"></param>
			/// <param name="pointInformation"></param>
			private void AddToOpen(int2 pos, PointInformation pointInformation)
			{
				if (openDic.ContainsKey(pos))
				{
					if (openDic[pos].g > pointInformation.g)
					{
						openDic[pos] = pointInformation;
					}
				}
				else
				{
					openDic.Add(pos, pointInformation);
				}
			}

			private void CreatePath()
			{
				var tempInformation = openDic[endPos];
				var tempPos = endPos;

				pathResultList.Add(tempPos);
				while (!tempPos.Equals(startPos))
				{
					tempPos = tempInformation.parent;
					tempInformation = closeDic[tempPos];
					pathResultList.Add(tempPos);
				}
			}
		}
	}
}
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
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace BiuBiu
{
	/// <summary>
	/// 寻路系统
	/// </summary>
	public class PathfindingSystem : ComponentSystem
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

		private int targetIndex;

		protected override void OnUpdate()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}
			RefreshTargetPos();
			var pointInformationArray = GetMapPointInformationArray();
			var curMapConfig = GameMain.GamePlay.CurMapConfig;

			Entities
				.WithAll<PathFollowComponent, Translation>()
				.ForEach((Entity entity, DynamicBuffer<PathPositionBuffer> positionBuffer,ref ControlBuffComponent controlBuffComponent, ref Translation translation) =>
				{
					var startIndex = PathfindingUtils.GetIndexByWorldPosition(translation.Value, curMapConfig.CellSize, curMapConfig.MapSize);
					if ((startIndex >= 0 && startIndex < curMapConfig.PointArray.Length)
					    && (targetIndex >= 0 && targetIndex < curMapConfig.PointArray.Length)
					    && curMapConfig.PointArray[startIndex].index != 0
					    && curMapConfig.PointArray[targetIndex].index != 0
					    && startIndex != targetIndex
					    && controlBuffComponent.BackTime <= 0f)
					{
						var pathFindingJob = new PathfindingJob
						{
							PointInformationArray = pointInformationArray,
							ResultBuffer = positionBuffer,
							PathFollowComponentFromEntity = GetComponentDataFromEntity<PathFollowComponent>(),
							PathEntity = entity,
							Start = startIndex,
							End = targetIndex,
							CellSize = curMapConfig.CellSize,
							MapSize = curMapConfig.MapSize,
						};
						
						pathFindingJob.Run();
					}
				});

			pointInformationArray.Dispose();
		}

		/// <summary>
		/// 刷新玩家位置地图索引
		/// </summary>
		private void RefreshTargetPos()
		{
			var playerPos = GameMain.GamePlay.PlayerController.transform.position;
			var cellSize = GameMain.GamePlay.CurMapConfig.CellSize;
			var mapSize = GameMain.GamePlay.CurMapConfig.MapSize;
			targetIndex = PathfindingUtils.GetIndexByWorldPosition(playerPos, cellSize, mapSize);
		}

		/// <summary>
		/// 获取网格配置所有可行走的点
		/// </summary>
		/// <returns></returns>
		private NativeArray<PointInformation> GetMapPointInformationArray()
		{
			var pointInformationArray = new NativeArray<PointInformation>(GameMain.GamePlay.CurMapConfig.PointArray.Length, Allocator.TempJob);
			var mapConfig = GameMain.GamePlay.CurMapConfig;
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

		/// <summary>
		/// 寻路Job
		/// </summary>
		[BurstCompile]
		private struct PathfindingJob : IJob
		{
			[ReadOnly] public NativeArray<PointInformation> PointInformationArray;
			public DynamicBuffer<PathPositionBuffer> ResultBuffer;
			public ComponentDataFromEntity<PathFollowComponent> PathFollowComponentFromEntity;
			public Entity PathEntity;
			public float CellSize;
			public float MapSize;
			public int Start;
			public int End;
			private int processingIndex;

			public void Execute()
			{
				var pointInformationArray = new NativeArray<PointInformation>(PointInformationArray.Length, Allocator.Temp);
				PointInformationArray.CopyTo(pointInformationArray);
				
				var openList = new NativeList<int>(Allocator.Temp);
				var closeList = new NativeList<int>(Allocator.Temp);
				openList.Add(Start);

				while (openList.Length > 0)
				{
					SelectMinCostF(pointInformationArray, openList, closeList);
					RefreshSurroundPoints(pointInformationArray, openList, closeList);
					if (openList.Contains(End))
					{
						CreatePath(pointInformationArray);
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
			/// <param name="pointInformationArray"></param>
			/// <param name="openList"></param>
			/// <param name="closeList"></param>
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

				processingIndex = openList[key];
				closeList.Add(openList[key]);
				openList.RemoveAt(key);
			}

			/// <summary>
			/// 刷新当前点的周围点信息
			/// </summary>
			/// <param name="pointInformationArray"></param>
			/// <param name="openList"></param>
			/// <param name="closeList"></param>
			private void RefreshSurroundPoints(NativeArray<PointInformation> pointInformationArray, NativeList<int> openList, NativeList<int> closeList)
			{
				for (var x = -CellSize; x <= CellSize; x += CellSize)
				{
					for (var z = -CellSize; z <= CellSize; z += CellSize)
					{
						if (x.Equals(0f) && z.Equals(0f))
						{
							continue;
						}
			
						var processingPointInformation = pointInformationArray[processingIndex];
						var posX = processingPointInformation.WorldPosX + x;
						var posZ = processingPointInformation.WorldPosZ + z;
						var index = PathfindingUtils.GetIndexByWorldPosition(new float3(posX, 0f, posZ), CellSize, MapSize);
						if (!(index >= 0 && index < pointInformationArray.Length) || !pointInformationArray[index].IsWalkable || closeList.Contains(index))
						{
							continue;
						}
			
						var costG = math.abs(x).Equals(math.abs(z)) ? processingPointInformation.G + 14 : processingPointInformation.G + 10;
						var costH = (math.abs(posX - pointInformationArray[End].WorldPosX) + math.abs(posZ - pointInformationArray[End].WorldPosZ)) * 10;
						var costF = costG + costH;

						var pointInformation = pointInformationArray[index];
						var newPointInformation = new PointInformation
						{
							WorldPosX = pointInformation.WorldPosX,
							WorldPosZ = pointInformation.WorldPosZ,
							
							G = costG,
							H = costH,
							F = costF,
							Parent = processingIndex,
							
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

			/// <summary>
			/// 创建最短路径存入Buffer
			/// </summary>
			/// <param name="pointInformationArray"></param>
			private void CreatePath(NativeArray<PointInformation> pointInformationArray)
			{
				ResultBuffer.Clear();
				var tempIndex = End;
				do
				{
					var tempInformation = pointInformationArray[tempIndex];
					ResultBuffer.Add(new PathPositionBuffer {MapPointIndex = tempIndex});
					tempIndex = tempInformation.Parent;
				} while (!tempIndex.Equals(Start));

				var pathFollowComponent = PathFollowComponentFromEntity[PathEntity];
				pathFollowComponent.PathIndex = ResultBuffer.Length - 1;
				PathFollowComponentFromEntity[PathEntity] = pathFollowComponent;
			}
		}
	}
}
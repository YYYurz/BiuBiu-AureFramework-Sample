//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AureFramework;
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
		private float3 targetPos;

		protected override void OnCreate()
		{
			base.OnCreate();
		}

		public override void Update()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}
			
			// var queryDescription = new EntityQueryDesc
			// {
			// 	All = new ComponentType[]
			// 	{
			// 		typeof(PositionComponent),
			// 		typeof(Translation),
			// 	},
			// 	None = new ComponentType[] {typeof(ControlBuffComponent)},
			// };
			//
			// var query = GetEntityQuery(queryDescription);
			// var entityArray = query.ToEntityArray(Allocator.TempJob);
			// foreach (var entity in entityArray)
			// {
			// }
		}

		/// <summary>
		/// 寻路Job
		/// </summary>
		[BurstCompile]
		private struct PathfindingJob : IJob
		{
			private NativeHashMap<int2, float3> pointDic;
			private NativeHashMap<int2, PointInformation> openDic;
			private NativeHashMap<int2, PointInformation> closeDic;
			private NativeList<int2> pathResultList;
			private int2 processingPos;
			private readonly int2 start;
			private readonly int2 end;

			public PathfindingJob(int2 start, int2 end)
			{
				pointDic = new NativeHashMap<int2, float3>();
				openDic = new NativeHashMap<int2, PointInformation>();
				closeDic = new NativeHashMap<int2, PointInformation>();
				pathResultList = new NativeList<int2>();
				processingPos = int2.zero;
				this.start = start;
				this.end = end;
			}
			
			public void Execute()
			{
				openDic.Add(start, new PointInformation
				{
					g = 0,
					h = 0,
					f = 0,
					parent = start,
				});
				
				while (!openDic.IsEmpty)
				{
					SelectMinCostF();
					RefreshSurroundPoints();
					if (openDic.ContainsKey(end))
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
						var costH = (Mathf.Abs(posX - end.x) + Mathf.Abs(posY - end.y)) * 10;

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
				var tempInformation = openDic[end];
				var tempPos = end;

				pathResultList.Add(tempPos);
				while (!tempPos.Equals(start))
				{
					tempPos = tempInformation.parent;
					tempInformation = closeDic[tempPos];
					pathResultList.Add(tempPos);
				}
			}
		}
	}
}
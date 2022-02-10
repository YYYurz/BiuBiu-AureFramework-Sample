//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace BiuBiu
{
	public class PathfindingSystem : ComponentSystemBase
	{
		private struct PointInformation
		{
			public int g;
			public int h;
			public int f;
			public int2 parent;
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			
			// Debug.Log("PathfindingSystem  OnCreate");
		}

		public override void Update()
		{
			// if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			// {
			// 	return;
			// }
			
			
			// GetEntityQuery()
			// var lookup = GetBufferFromEntity<PathPointBufferElement>();
			// var buffer = lookup[]
			
			// Debug.Log("PathfindingSystem  Update");
		}

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
				while (openDic.Any())
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
				var tempPoint = openDic.First();
				foreach (var openPoint in openDic)
				{
					if (openPoint.Value.f < tempPoint.Value.f)
					{
						tempPoint = openPoint;
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
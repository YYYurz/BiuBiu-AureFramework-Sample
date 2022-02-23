//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 寻路工具
	/// </summary>
	public static class PathfindingUtils
	{
		/// <summary>
		/// 根据实际位置获取在网格配置中的索引
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="cellSize"></param>
		/// <param name="mapSize"></param>
		/// <returns></returns>
		public static int GetIndexByWorldPosition(float3 worldPos, float cellSize, float mapSize)
		{
			var halfMapSize = mapSize * 0.5f;
			if (Mathf.Abs(worldPos.x) > halfMapSize || Mathf.Abs(worldPos.z) > halfMapSize)
			{
				return -1;
			}
			
			var cellNumPerLine = Mathf.CeilToInt(mapSize / cellSize);
			var xValue = Mathf.CeilToInt((worldPos.x + halfMapSize) / cellSize);
			var zValue = Mathf.FloorToInt((worldPos.z + halfMapSize) / cellSize);
			var index = xValue + zValue * cellNumPerLine;

			return index;
		}

		/// <summary>
		/// 根据索引获取在网格配置中的实际位置
		/// </summary>
		/// <param name="index"></param>
		/// <param name="cellSize"></param>
		/// <param name="mapSize"></param>
		/// <param name="mapHeight"></param>
		/// <returns></returns>
		public static float3 GetWorldPositionByIndex(int index, float cellSize, float mapSize, float mapHeight)
		{
			var halfMapSize = mapSize * 0.5f;
			var halfCellSize = cellSize * 0.5f;
			var xValue = index % 10;
			var zValue = Mathf.FloorToInt(index / 10f) + 1;
			var xPos = xValue * cellSize - halfCellSize - halfMapSize;
			var zPos = zValue * cellSize - halfCellSize - halfMapSize;
			
			return new float3(xPos, mapHeight, zPos);
		}

		public static float3 GetNearestEffectivePosInMap(MapConfig mapConfig, float3 pos)
		{
			var cellSize = mapConfig.CellSize;
			var mapSize = mapConfig.MapSize;
			var mapHeight = mapConfig.MapHeight;
			var pointArray = mapConfig.PointArray;
			var curIndex = GetIndexByWorldPosition(pos, cellSize, mapSize);
			var curUnitPos = GetWorldPositionByIndex(curIndex, cellSize, mapSize, mapHeight);
			var nearestIndex = 0;
			var nearestDistance = float.MaxValue;
			for (var x = -cellSize; x <= cellSize; x += cellSize)
			{
				for (var z = -cellSize; z <= cellSize; z += cellSize)
				{
					if (x.Equals(0f) && z.Equals(0f))
					{
						continue;
					}
					
					var posX = curUnitPos.x + x;
					var posZ = curUnitPos.z + z;
					var index = GetIndexByWorldPosition(new float3(posX, 0f, posZ), cellSize, mapSize);
					if (!(index >= 0 && index < pointArray.Length) || pointArray[index].index == 0)
					{
						continue;
					}

					var distance = math.distance(pos, pointArray[index].worldPos);
					if (nearestIndex == 0 || distance < nearestDistance)
					{
						nearestIndex = index;
						nearestDistance = distance;
					}
				}
			}

			var nearestPoint = pointArray[nearestIndex];
			var xPos = pos.x;
			if (math.abs(nearestPoint.worldPos.x - pos.x) > cellSize * 0.5f)
			{
				if (nearestPoint.worldPos.x - pos.x > 0f)
				{
					xPos = nearestPoint.worldPos.x - cellSize + 0.001f;
				}
				else
				{
					xPos = nearestPoint.worldPos.x + cellSize - 0.001f;
				}
			}

			var zPos = pos.z;
			if (math.abs(nearestPoint.worldPos.z - pos.z) > cellSize * 0.5f)
			{
				if (nearestPoint.worldPos.z - pos.z > 0f)
				{
					zPos = nearestPoint.worldPos.z - cellSize * 0.5f + 0.001f;
				}
				else
				{
					zPos = nearestPoint.worldPos.z + cellSize * 0.5f - 0.001f;
				}
			}

			return new float3(xPos, pos.y, zPos);
		}
	}
}
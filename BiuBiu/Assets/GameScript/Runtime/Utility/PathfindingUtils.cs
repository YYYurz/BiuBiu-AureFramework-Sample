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
	}
}
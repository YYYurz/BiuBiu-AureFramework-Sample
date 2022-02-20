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
		/// 根据实际位置获取在网格配置中的具体格子位置
		/// </summary>
		/// <param name="actualPosition"></param>
		/// <param name="cellSize"></param>
		/// <param name="mapHeight"></param>
		/// <returns></returns>
		public static float3 GetUnitPositionInMapConfig(float3 actualPosition, float2 cellSize, float mapHeight)
		{
			var posX = Mathf.FloorToInt((actualPosition.x + cellSize.x / 2) / cellSize.x) * cellSize.x;
			var posZ = Mathf.FloorToInt((actualPosition.z + cellSize.y / 2) / cellSize.y) * cellSize.y;
			
			return new float3(posX, mapHeight, posZ);
		}

		/// <summary>
		/// 根据实际位置的xz左边获取在网格配置中的索引位置
		/// </summary>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="cellSize"></param>
		/// <returns></returns>
		public static int2 GetIndexPositionInMapConfig(float x, float z, float2 cellSize)
		{
			var x2d = x >= 0 ? Mathf.CeilToInt(x / cellSize.x) : Mathf.FloorToInt(x / cellSize.x);
			var z2d = z >= 0 ? Mathf.CeilToInt(z / cellSize.y) : Mathf.FloorToInt(z / cellSize.y);

			return new int2(x2d, z2d);
		}
	}
}
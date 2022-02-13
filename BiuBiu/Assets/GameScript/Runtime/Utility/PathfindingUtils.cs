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
		/// 根据实际位置获取在网格配置中的索引位置
		/// </summary>
		/// <param name="actualPosition"></param>
		/// <param name="cellSize"></param>
		/// <param name="mapHeight"></param>
		/// <returns></returns>
		public static float3 GetIndexPositionInMapConfig(float3 actualPosition, float2 cellSize, float mapHeight)
		{
			var posX = Mathf.FloorToInt((actualPosition.x + cellSize.x / 2) / cellSize.x) * cellSize.x;
			var posZ = Mathf.FloorToInt((actualPosition.z + cellSize.y / 2) / cellSize.y) * cellSize.y;
			
			return new float3(posX, mapHeight, posZ);
		}
	}
}
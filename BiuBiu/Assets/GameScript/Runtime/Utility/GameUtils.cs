//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui yu.
// GitHub: https://github.com/yyyurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 游戏工具
	/// </summary>
	public static class GameUtils
	{
		/// <summary>
		/// 判断点是否在圆内（三维）
		/// </summary>
		/// <returns></returns>
		public static bool PointInCircle(float3 position, float3 center, float radius)
		{
			return math.distance(position, center) <= radius;
		}
		
		/// <summary>
		/// 判断点是否在多边形内（二维）
		/// </summary>
		/// <param name="position"> 当前位置 </param>
		/// <param name="polygonVertexList"> 多边形顶点列表 </param>
		/// <returns></returns>
		public static bool PointInPolygon(float2 position, NativeList<float2> polygonVertexList)
		{
			if (polygonVertexList.Length < 3)
			{
				return false;
			}

			var crossCount = 0;
			for (var i = 0; i < polygonVertexList.Length; i++)
			{
				var v1 = polygonVertexList[i];
				var v2 = polygonVertexList[i + 1 == polygonVertexList.Length ? 0 : i + 1];
				if ((v1.y <= position.y && v2.y > position.y) || (v1.y > position.y && v2.y <= position.y)) 
				{
					if (position.x > v1.x + (position.y - v1.y) / (v2.y - v1.y) * (v2.x - v1.x))
					{
						crossCount++;
					}					
				}
			}

			return crossCount % 2 > 0;
		}
	}
}


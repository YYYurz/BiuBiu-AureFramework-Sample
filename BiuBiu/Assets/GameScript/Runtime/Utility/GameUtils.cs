//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui yu.
// GitHub: https://github.com/yyyurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 游戏工具
	/// </summary>
	public static class GameUtils
	{
		/// <summary>
		/// 判断点是否在多边形内
		/// </summary>
		/// <param name="position"> 当前位置 </param>
		/// <param name="polygonPointList"> 多边形顶点列表 </param>
		/// <returns></returns>
		public static bool PointInPolygon(Vector3 position, List<Vector3> polygonPointList)
		{
			if (polygonPointList == null)
			{
				return false;
			}

			int j = 0, intersectCount = 0;
			for (var i = 0; i < polygonPointList.Count; i++)
			{
				j = (i == polygonPointList.Count - 1) ? 0 : j + 1;
				if (!(polygonPointList[i].y.Equals(polygonPointList[j].y))
				    && (((position.y >= polygonPointList[i].y) && (position.y < polygonPointList[j].y)) 
				        || ((position.y >= polygonPointList[j].y) && (position.y < polygonPointList[i].y))) 
				    && (position.x < (polygonPointList[j].x - polygonPointList[i].x) * (position.y - polygonPointList[i].y) / (polygonPointList[j].y - polygonPointList[i].y) + polygonPointList[i].x)) intersectCount++;
			}

			return intersectCount % 2 > 0;
		}
	}
}


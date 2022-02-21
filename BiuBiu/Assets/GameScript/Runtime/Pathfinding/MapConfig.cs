//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace BiuBiu
{
	[BurstCompile]
	[Serializable]
	public struct MapPoint
	{
		/// <summary>
		/// 索引
		/// </summary>
		public int index;
		
		/// <summary>
		/// 实际世界坐标
		/// </summary>
		public float3 worldPos;

		public MapPoint(int index, float3 worldPos)
		{
			this.index = index;
			this.worldPos = worldPos;
		}
	}
	
	/// <summary>
	/// 寻路地图配置
	/// </summary>
	[Serializable]
	public class MapConfig : ScriptableObject
	{
		public const string DefaultMapConfigPath = "Assets/MapConfig.asset";

		[SerializeField] private MapPoint[] pointArray = new MapPoint[0];
		[SerializeField] private float cellSize = 1f;
		[SerializeField] private float mapHeight;
		[SerializeField] private float mapSize = 100f;


		/// <summary>
		/// 获取可行走格子坐标列表
		/// </summary>
		public MapPoint[] PointArray
		{
			get
			{
				return pointArray;
			}
		}

		/// <summary>
		/// 获取或设置格子大小
		/// </summary>
		public float CellSize
		{
			get
			{
				return cellSize;
			}
			set
			{
				cellSize = value;
				RefreshPointList();
			}
		}
		
		/// <summary>
		/// 获取或设置地图高度
		/// </summary>
		public float MapHeight
		{
			get
			{
				return mapHeight;
			}
			set
			{
				mapHeight = value;
				RefreshPointList();
			}
		}
		
		/// <summary>
		/// 获取或设置网格尺寸
		/// </summary>
		public float MapSize
		{
			get
			{
				return mapSize;
			}
			set
			{
				mapSize = value;
				RefreshPointList();
			}
		}
		
		public static MapConfig CreateDefaultConfig(string filePath)
		{
			var path = string.IsNullOrEmpty(filePath) ? DefaultMapConfigPath : filePath;
			if (AssetDatabase.LoadAssetAtPath<MapConfig>(path) != null)
			{
				Debug.LogError("MapConfig : Default map config is already exist.");
				return null;
			}
			
			var mapConfig = CreateInstance<MapConfig>();
			AssetDatabase.CreateAsset(mapConfig, path);

			return mapConfig;
		}

		private void RefreshPointList()
		{
			var cellNumPerLine = Mathf.CeilToInt(mapSize / cellSize);
			var newPointArray = new MapPoint[cellNumPerLine * cellNumPerLine + 1];
			for (var i = 0; i < pointArray.Length; i++)
			{
				if (pointArray[i].index != 0)
				{
					var worldPos = pointArray[i].worldPos;
					var newIndex = PathfindingUtils.GetIndexByWorldPosition(worldPos, cellSize, mapSize);
					if (newIndex >= 0 && newIndex < newPointArray.Length)
					{
						newPointArray[newIndex] = new MapPoint
						{
							index = newIndex,
							worldPos = worldPos,
						};
					}
				}
			}

			pointArray = newPointArray;
		}
	}
}
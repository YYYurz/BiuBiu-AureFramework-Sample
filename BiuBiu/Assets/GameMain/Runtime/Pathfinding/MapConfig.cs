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
		/// 2d坐标轴位置，按数组索引方式
		/// </summary>
		public int2 key;
		
		/// <summary>
		/// 实际世界坐标
		/// </summary>
		public float3 value;

		public MapPoint(int2 key, float3 value)
		{
			this.key = key;
			this.value = value;
		}
	}
	
	/// <summary>
	/// 寻路地图配置
	/// </summary>
	[Serializable]
	public class MapConfig : ScriptableObject
	{
		public const string DefaultMapConfigPath = "Assets/MapConfig.asset";

		[SerializeField] private List<MapPoint> pointList = new List<MapPoint>();
		[SerializeField] private Vector2 cellSize = Vector2.one;
		[SerializeField] private float mapHeight;
		[SerializeField] private float mapMaxSize = 100f;


		/// <summary>
		/// 获取可行走格子坐标列表
		/// </summary>
		public List<MapPoint> PointList
		{
			get
			{
				return pointList;
			}
		}

		/// <summary>
		/// 获取或设置格子大小
		/// </summary>
		public Vector2 CellSize
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
		public float MapMaxSize
		{
			get
			{
				return mapMaxSize;
			}
			set
			{
				mapMaxSize = value;
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
			for (var i = 0; i < pointList.Count; i++)
			{
				var position2D = pointList[i].key;
				var newX = position2D.x * cellSize.x - cellSize.x / 2f;
				var newY = mapHeight;
				var newZ = position2D.y * cellSize.y - cellSize.y / 2f;
				pointList[i] = new MapPoint
				{
					key = position2D,
					value = new float3(newX, newY, newZ)
				};
			}
		}
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace BiuBiu
{
	[Serializable]
	public struct Point
	{
		public float2 key;
		public float3 value;

		public Point(float2 key, float3 value)
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

		[SerializeField] private List<float3> pointList = new List<float3>();
		[SerializeField] private Vector2 cellSize = Vector2.one;
		[SerializeField] private float mapHeight = 0f;
		[SerializeField] private float mapMaxSize = 100f;


		/// <summary>
		/// 获取可行走格子坐标列表
		/// </summary>
		public List<float3> PointList
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
				var offsetX = value.x - cellSize.x;
				var offsetZ = value.y - cellSize.y;
				RefreshPointList(offsetX, 0f, offsetZ);

				cellSize = value;
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
				var offsetY = value - mapHeight;
				RefreshPointList(0f, offsetY, 0f);
				
				mapHeight = value;
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

		private void RefreshPointList(float offsetX, float offsetY, float offsetZ)
		{
			for (var i = 0; i < pointList.Count; i++)
			{
				var point = pointList[i];
				var newX = point.x + offsetX;
				var newY = point.y + offsetY;
				var newZ = point.z + offsetZ;
				pointList[i] = new float3(newX, newY, newZ);
			}
		}
	}
}
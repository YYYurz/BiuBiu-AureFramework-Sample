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
		public float2 value;

		public Point(float2 key, float2 value)
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

		[SerializeField] private List<Point> pointDic = new List<Point>();

		public List<Point> PointDic
		{
			get
			{
				return pointDic;
			}
		}
		
		public static MapConfig CreateDefaultConfig(string filePath)
		{
			var mapConfig = CreateInstance<MapConfig>();
			AssetDatabase.CreateAsset(mapConfig, string.IsNullOrEmpty(filePath) ? DefaultMapConfigPath : filePath);

			return mapConfig;
		}
	}
}
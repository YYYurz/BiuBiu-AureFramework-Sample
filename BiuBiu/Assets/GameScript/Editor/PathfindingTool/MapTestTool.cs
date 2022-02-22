//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace BiuBiu.Editor
{
	/// <summary>
	/// 配置寻路网格工具
	/// </summary>
	public class MapTestTool : EditorWindow
	{
		private enum PointType
		{
			Start,
			End,
		}
		
		private GameObject planeObj;
		private MapConfig curMapConfig;
		private float cellSize = 1f;
		private float mapHeight;
		private float mapSize = 100f;

		private Vector3 mouseMovePos;
		private Vector3 mouseDownPos;
		private Vector3 mouseUpPos;
		private PointType curPointType = PointType.Start;
		private bool isDragging;

		private Vector3 startPosition;
		private Vector3 endPosition;
		
		/// <summary>
		/// 获取或设置编辑的地图配置
		/// </summary>
		private MapConfig CurMapConfig
		{
			get
			{
				return curMapConfig;
			}
			set
			{
				if (value != null && !value.Equals(curMapConfig))
				{
					cellSize = value.CellSize;
					mapHeight = value.MapHeight;
					mapSize = value.MapSize;
				}
				
				curMapConfig = value;
			}
		}

		/// <summary>
		/// 设置格子大小
		/// </summary>
		private float CellSize
		{
			set
			{
				if (value.Equals(cellSize))
				{
					return;
				}
				
				cellSize = value < 0.05f ? 0.05f : value;
				if (curMapConfig != null)
				{
					curMapConfig.CellSize = cellSize;
				}
			}
		}

		/// <summary>
		/// 设置网格高度
		/// </summary>
		private float MapHeight
		{
			set
			{
				if (value.Equals(mapHeight))
				{
					return;
				}
				
				mapHeight = value < 0 ? 0 : value;
				if (planeObj != null)
				{
					planeObj.transform.position = new Vector3(0f, mapHeight, 0f);
				}

				if (curMapConfig != null)
				{
					curMapConfig.MapHeight = value;
				}
			}
		}

		/// <summary>
		/// 设置网格尺寸
		/// </summary>
		private float MapMaxSize
		{
			set
			{
				mapSize = value;
				if (planeObj != null)
				{
					planeObj.transform.localScale = new Vector3(mapSize / 10f, 1f, mapSize / 10f);
				}
				
				if (curMapConfig != null)
				{
					curMapConfig.MapSize = value;
				}
			}
		}
		
		[MenuItem("BiuBiu/寻路测试", false, 101)]
		private static void Open()
		{
			var window = GetWindow<MapTestTool>(true, "地图网格编辑工具", true);
			window.minSize = window.maxSize = new Vector2(300f, 150f);
		}
		
		private void OnEnable()
		{
			SceneView.duringSceneGui += OnSceneGUI;
			Tools.hidden = true;
			
			planeObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			planeObj.GetComponent<MeshRenderer>().enabled = false;
			planeObj.transform.position = new Vector3(0f, mapHeight, 0f);
			planeObj.transform.localScale = new Vector3(mapSize / 10f, 1f, mapSize / 10f);
			planeObj.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
		}

		private void OnGUI()
		{
			CurMapConfig = (MapConfig) EditorGUILayout.ObjectField("寻路网格配置文件", CurMapConfig, typeof(MapConfig), false);
			curPointType = (PointType) EditorGUILayout.EnumPopup("格子类型", curPointType);
			if (GUILayout.Button("测试"))
			{
				Test();
			}
		}

		private struct PointInformation
		{
			public float WorldPosX;
			public float WorldPosZ;
			
			public float G;
			public float H;
			public float F;
			
			public int Parent;

			public bool IsWalkable;
		}

		private PointInformation[] pointInformationArray;
		private readonly List<int> openList = new List<int>();
		private readonly List<int> closeList = new List<int>();
		private readonly List<int> pathResultList = new List<int>();
		private int processingPos;
		private int start;
		private int end;
		
		private void Test()
		{
			if (!Init())
			{
				return;
			}
			
			var i = 1;
			while (openList.Count > 0)
			{
				SelectMinCostF();
				RefreshSurroundPoints();
				if (openList.Contains(end))
				{
					CreatePath();
					break;
				}
				i++;

			} 
			Debug.Log(i);

		}

		private bool Init()
		{
			start = PathfindingUtils.GetIndexByWorldPosition(startPosition, cellSize, mapSize);
			end = PathfindingUtils.GetIndexByWorldPosition(endPosition, cellSize, mapSize);
			if (!(start >= 0 && start < curMapConfig.PointArray.Length)
			|| !(end >= 0 && end < curMapConfig.PointArray.Length)
			|| curMapConfig.PointArray[start].index == 0
			|| curMapConfig.PointArray[end].index == 0)
			{
				return false;
			}
			
			openList.Clear();
			closeList.Clear();
			pathResultList.Clear();
			
			var cellNumPerLine = Mathf.CeilToInt(mapSize / cellSize);
			pointInformationArray = new PointInformation[cellNumPerLine * cellNumPerLine + 1];
			for (var i = 0; i < pointInformationArray.Length; i++)
			{
				var point = curMapConfig.PointArray[i];
				if (point.index != 0)
				{
					pointInformationArray[i].WorldPosX = point.worldPos.x;
					pointInformationArray[i].WorldPosZ = point.worldPos.z;
					pointInformationArray[i].IsWalkable = true;
				}
			}
			
			openList.Add(start);
			return true;
		}
		
		/// <summary>
		/// 选择F值最小的节点作为当前处理节点
		/// </summary>
		private void SelectMinCostF()
		{
			var tempIndex = openList[0];
			foreach (var openPoint in openList)
			{
				if (pointInformationArray[openPoint].F < pointInformationArray[tempIndex].F)
				{
					tempIndex = openPoint;
				}
			}

			processingPos = tempIndex;
			openList.Remove(tempIndex);
			closeList.Add(tempIndex);
		}

		/// <summary>
		/// 刷新当前点的周围点信息
		/// </summary>
		private void RefreshSurroundPoints()
		{
			for (var x = -cellSize; x <= cellSize; x += cellSize)
			{
				for (var z = -cellSize; z <= cellSize; z += cellSize)
				{
					if (x.Equals(0f) && z.Equals(0f))
					{
						continue;
					}
			
					var processingPointInformation = pointInformationArray[processingPos];
					var posX = processingPointInformation.WorldPosX + x;
					var posZ = processingPointInformation.WorldPosZ + z;
					var index = PathfindingUtils.GetIndexByWorldPosition(new float3(posX, 0f, posZ), cellSize, mapSize);
					if (!(index >= 0 && index < pointInformationArray.Length) || !pointInformationArray[index].IsWalkable || closeList.Contains(index))
					{
						continue;
					}
			
					var costG = Mathf.Abs(x).Equals(Mathf.Abs(z)) ? processingPointInformation.G + 14 : processingPointInformation.G + 10;
					var costH = (Mathf.Abs(posX - pointInformationArray[end].WorldPosX) + Mathf.Abs(posZ - pointInformationArray[end].WorldPosZ)) * 10;
					var costF = costG + costH;

					if (openList.Contains(index))
					{
						if (pointInformationArray[index].G > costG)
						{
							pointInformationArray[index].G = costG;
							pointInformationArray[index].H = costH;
							pointInformationArray[index].F = costF;
							pointInformationArray[index].Parent = processingPos;
						}
					}
					else
					{
						pointInformationArray[index].G = costG;
						pointInformationArray[index].H = costH;
						pointInformationArray[index].F = costF;
						pointInformationArray[index].Parent = processingPos;
						openList.Add(index);
					}
				}
			}
		}
		
		private void CreatePath()
		{
			var tempInformation = pointInformationArray[end];
			var tempIndex = end;

			pathResultList.Add(end);
			while (!tempIndex.Equals(start))
			{
				tempIndex = tempInformation.Parent;
				tempInformation = pointInformationArray[tempIndex];
				pathResultList.Add(tempIndex);
			}
			
			pathResultList.Reverse();
		}
		
		
		
		
		
		
		
		
		
		

		private void OnDisable()
		{
			SceneView.duringSceneGui -= OnSceneGUI;
			Tools.hidden = false;
			if (planeObj != null)
			{
				DestroyImmediate(planeObj);
			}
		}

		private void OnSceneGUI(SceneView sceneView)
		{
			// 禁用场景中物体选中
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			
			switch (Event.current.type)
			{
				case EventType.MouseDown when Event.current.button == 0:
				{
					OnMouseSelectEvent(EventType.MouseDown);
					break;
				}
				case EventType.MouseUp when Event.current.button == 0:
				{
					OnMouseSelectEvent(EventType.MouseUp);
					break;
				}
			}
			OnMouseMoveEvent();

			GL.Flush();
			DrawConfigCell();
			DrawMapMeshLine();
			DrawMouseMoveCell();
			DrawMouseSelectCell();

			DrawOpenCell();
			DrawCloseCell();
			DrawPathCell();
			
			SceneView.RepaintAll();
		}

		private void DrawPathCell()
		{
			if (curMapConfig == null)
			{
				return;
			}
			
			var pointArray = curMapConfig.PointArray;
			foreach (var pathIndex in pathResultList)
			{
				DrawCellByPoint(pointArray[pathIndex].worldPos, Color.magenta, Color.black);
			}
		}
		
		private void DrawOpenCell()
		{
			if (curMapConfig == null)
			{
				return;
			}
			
			var pointArray = curMapConfig.PointArray;
			foreach (var openIndex in openList)
			{
				DrawCellByPoint(pointArray[openIndex].worldPos, Color.yellow, Color.black);
			}
		}
		
		private void DrawCloseCell()
		{
			if (curMapConfig == null)
			{
				return;
			}
			
			var pointArray = curMapConfig.PointArray;
			foreach (var closeIndex in closeList)
			{
				DrawCellByPoint(pointArray[closeIndex].worldPos, Color.black, Color.black);
			}
		}

		/// <summary>
		/// 绘制配置中保存的网格
		/// </summary>
		private void DrawConfigCell()
		{
			if (curMapConfig == null)
			{
				return;
			}
			
			foreach (var point in curMapConfig.PointArray)
			{
				DrawCellByPoint(point.worldPos, new Color(0f, 1f, 0f, 0.2f), Color.blue);
			}
		}

		/// <summary>
		/// 绘制跟随鼠标的格子
		/// </summary>
		private void DrawMouseMoveCell()
		{
			if (isDragging)
			{
				return;
			}

			DrawCellByPoint(mouseMovePos, new Color(0f, 1f, 0f, 0.1f), Color.blue);
		}

		/// <summary>
		/// 绘制鼠标选中格子
		/// </summary>
		private void DrawMouseSelectCell()
		{
			if (!startPosition.Equals(Vector3.zero))
			{
				DrawCellByPoint(startPosition, Color.red, Color.blue);
			}
			
			if (!endPosition.Equals(Vector3.zero))
			{
				DrawCellByPoint(endPosition, Color.black, Color.blue);
			}
		}

		/// <summary>
		/// 绘制地图网格
		/// </summary>
		private void DrawMapMeshLine()
		{
			var mapHalfSize = mapSize / 2f;
			
			var tempXPos = 0f;
			while (tempXPos <= mapHalfSize)
			{
				var leftXa = new Vector3(tempXPos, mapHeight, mapHalfSize);
				var leftXb = new Vector3(tempXPos, mapHeight, -mapHalfSize);
				Handles.DrawLine(leftXa, leftXb);
				
				var rightXa = new Vector3(-tempXPos, mapHeight, mapHalfSize);
				var rightXb = new Vector3(-tempXPos, mapHeight, -mapHalfSize);
				Handles.DrawLine(rightXa, rightXb);

				tempXPos += cellSize;
			}

			var tempYPos = 0f;
			while (tempYPos <= mapHalfSize)
			{
				var leftXa = new Vector3(mapHalfSize, mapHeight, tempYPos);
				var leftYa = new Vector3(-mapHalfSize, mapHeight, tempYPos);
				Handles.DrawLine(leftXa, leftYa);
				
				
				var rightXa = new Vector3(mapHalfSize, mapHeight, -tempYPos);
				var rightYa = new Vector3(-mapHalfSize, mapHeight, -tempYPos);
				Handles.DrawLine(rightXa, rightYa);
				
				tempYPos += cellSize;
			}
		}
		
		private void DrawCellByPoint(Vector3 point, Color faceColor, Color outlineColor)
		{
			var vertArray = new[]
			{
				new Vector3(point.x - cellSize / 2, point.y, point.z - cellSize / 2),
				new Vector3(point.x - cellSize / 2, point.y, point.z + cellSize / 2),
				new Vector3(point.x + cellSize / 2, point.y, point.z + cellSize / 2),
				new Vector3(point.x + cellSize / 2, point.y, point.z - cellSize / 2),
			};
			Handles.DrawSolidRectangleWithOutline(vertArray, faceColor, outlineColor);
		}
		
		private void OnMouseMoveEvent()
		{
			if (TryGetMousePositionInCell(out var mousePose))
			{
				mouseMovePos = mousePose;
			}
		}

		private void OnMouseSelectEvent(EventType mouseEventType)
		{
			switch (mouseEventType)
			{
				case EventType.MouseDown when Event.current.button == 0:
				{
					if (TryGetMousePositionInCell(out var mousePose))
					{
						mouseDownPos = mousePose;
					}
					else
					{
						mouseDownPos = mouseMovePos;
					}

					SelectPoint();
					break;
				}
			}
		}

		private void SelectPoint()
		{
			switch (curPointType)
			{
				case PointType.Start:
					startPosition = mouseDownPos;
					break;
				case PointType.End:
					endPosition = mouseDownPos;
					break;
			}
		}
		
		/// <summary>
		/// 获取鼠标当前所在的格子坐标
		/// </summary>
		private bool TryGetMousePositionInCell(out Vector3 mousePose)
		{
			mousePose = Vector3.zero;
			
			var mousePos = Event.current.mousePosition;
			mousePos.y = Camera.current.pixelHeight - mousePos.y;
			var ray = Camera.current.ScreenPointToRay(mousePos);
			if (Physics.Raycast(ray, out var rh, 3000f) && rh.collider.gameObject == planeObj)
			{
				var posX = Mathf.CeilToInt((rh.point.x / cellSize)) * cellSize - cellSize / 2;
				var posZ = Mathf.CeilToInt((rh.point.z / cellSize)) * cellSize - cellSize / 2;
				mousePose = new float3(posX, mapHeight, posZ);

				return true;
			}
			
			return false;
		}
	}
}
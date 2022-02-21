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
		private float mapMaxSize = 100f;

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
					mapMaxSize = value.MapSize;
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
				mapMaxSize = value;
				if (planeObj != null)
				{
					planeObj.transform.localScale = new Vector3(mapMaxSize / 10f, 1f, mapMaxSize / 10f);
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
			planeObj.transform.localScale = new Vector3(mapMaxSize / 10f, 1f, mapMaxSize / 10f);
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
			public float worldPosx;
			public float worldPosz;
			
			public int g;
			public int h;
			public int f;
			public int parent;

			public bool isWalkable;
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
			Init();
			while (openList.Count > 0)
			{
				SelectMinCostF();
				RefreshSurroundPoints();
				if (openList.Contains(end))
				{
					CreatePath();
					break;
				}
			} 
		}

		private void Init()
		{
			openList.Clear();
			closeList.Clear();
			pathResultList.Clear();
			
			var cellNumPerLine = Mathf.CeilToInt(mapMaxSize / cellSize);
			pointInformationArray = new PointInformation[cellNumPerLine * cellNumPerLine + 1];
			for (var i = 0; i < pointInformationArray.Length; i++)
			{
				var point = curMapConfig.PointArray[i];
				if (point.index != 0)
				{
					pointInformationArray[i].worldPosx = point.worldPos.x;
					pointInformationArray[i].worldPosz = point.worldPos.z;
					pointInformationArray[i].isWalkable = true;
				}
			}
			
			openList.Add(start);
		}
		
		/// <summary>
		/// 选择F值最小的节点作为当前处理节点
		/// </summary>
		private void SelectMinCostF()
		{
			var tempIndex = openList[0];
			foreach (var openPoint in openList)
			{
				if (pointInformationArray[openPoint].f < pointInformationArray[tempIndex].f)
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
					var posX = processingPointInformation.worldPosx + x;
					var posZ = processingPointInformation.worldPosz + z;
					var pos = new int2(posX, posY);
					if (!pointDic.ContainsKey(pos) || closeList.ContainsKey(pos))
					{
						continue;
					}
			
					var costG = Mathf.Abs(x) == Mathf.Abs(z) ? processingPointInformation.g + 14 : processingPointInformation.g + 10;
					var costH = (Mathf.Abs(posX - end.x) + Mathf.Abs(posY - end.y)) * 10;
					
					var costF = costG + costH;
					var pointInformation = new PointInformation
					{
						g = costG,
						h = costH,
						f = costF,
						parent = processingPos,
					};
					AddToOpen(pos, pointInformation);
				}
			}
		}
		
		/// <summary>
		/// 添加点到OpenDic
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="pointInformation"></param>
		private void AddToOpen(int2 pos, PointInformation pointInformation)
		{
			if (openList.ContainsKey(pos))
			{
				if (openList[pos].g > pointInformation.g)
				{
					openList[pos] = pointInformation;
				}
			}
			else
			{
				openList.Add(pos, pointInformation);
			}
		}

		private void CreatePath()
		{
			var tempInformation = openList[end];
			var tempPos = end;

			pathResultList.Add(tempPos);
			while (!tempPos.Equals(start))
			{
				tempPos = tempInformation.parent;
				tempInformation = closeList[tempPos];
				pathResultList.Add(tempPos);
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
			foreach (var pathPoint in pathResultList)
			{
				DrawCellByPoint(pointDic[pathPoint], Color.magenta, Color.black);
			}
		}
		
		private void DrawOpenCell()
		{
			foreach (var openPoint in openList)
			{
				DrawCellByPoint(pointDic[openPoint.Key], Color.yellow, Color.black);
			}
		}
		
		private void DrawCloseCell()
		{
			foreach (var closePoint in closeList)
			{
				DrawCellByPoint(pointDic[closePoint.Key], Color.black, Color.black);
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
				DrawCellByPoint(point.worldPos, Color.green, Color.blue);
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
		/// 绘制鼠标拖拽选中格子
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
			var mapHalfSize = mapMaxSize / 2f;
			
			var tempXPos =cellSize / 2f;
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

			var tempYPos = cellSize / 2f;
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
				var posX = Mathf.FloorToInt((rh.point.x + cellSize / 2) / cellSize) * cellSize;
				var posZ = Mathf.FloorToInt((rh.point.z + cellSize / 2) / cellSize) * cellSize;
				mousePose = new Vector3(posX, mapHeight, posZ);

				return true;
			}
			
			return false;
		}
	}
}
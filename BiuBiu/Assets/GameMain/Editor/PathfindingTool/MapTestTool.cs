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
		private Vector2 cellSize = Vector2.one;
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
					mapMaxSize = value.MapMaxSize;
					
					pointDic.Clear();
					foreach (var point in value.PointList)
					{
						if (pointDic.ContainsKey(point.key))
						{
							Debug.LogError(point.key);
							continue;
						}
						pointDic.Add(point.key, point.value);
					}
				}
				
				curMapConfig = value;
			}
		}

		/// <summary>
		/// 设置格子大小
		/// </summary>
		private Vector2 CellSize
		{
			set
			{
				if (value.x.Equals(cellSize.x) && value.y.Equals(cellSize.y))
				{
					return;
				}
				
				var x = value.x < 0.05f ? 0.05f : value.x;
				var y = value.y < 0.05f ? 0.05f : value.y;
				cellSize = new Vector2(x, y);
				
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
					curMapConfig.MapMaxSize = value;
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
			public int g;
			public int h;
			public int f;
			public int2 parent;
		}

		private readonly Dictionary<int2, float3> pointDic = new Dictionary<int2, float3>();
		private readonly Dictionary<int2, PointInformation> openDic = new Dictionary<int2, PointInformation>();
		private readonly Dictionary<int2, PointInformation> closeDic = new Dictionary<int2, PointInformation>();
		private readonly List<int2> pathResultList = new List<int2>();
		private int2 processingPos;
		private int2 start;
		private int2 end;
		
		private void Test()
		{
			openDic.Clear();
			closeDic.Clear();
			pathResultList.Clear();

			start = pointDic.FirstOrDefault(point => point.Value.Equals(startPosition)).Key;
			end = pointDic.FirstOrDefault(point => point.Value.Equals(endPosition)).Key;

			Debug.Log("---");
			Debug.Log(start);
			Debug.Log(end);
			Debug.Log("---");
			
			do
			{
				SelectMinCostF();
				if (CheckAround())
				{
					CreatePath();
					break;
				}
			} 
			while (openDic.Count > 0);
		}

		/// <summary>
		/// 选择F值最小的节点作为当前处理节点
		/// </summary>
		private void SelectMinCostF()
		{
			var pointInformation = new PointInformation();
			if (closeDic.Count == 0)
			{
				pointInformation.g = 0;
				pointInformation.h = 0;
				pointInformation.f = 0;
				pointInformation.parent = start;

				processingPos = start;
				closeDic.Add(start, pointInformation);
			}
			else
			{
				var tempPoint = openDic.First();
				foreach (var openPoint in openDic)
				{
					if (openPoint.Value.f < tempPoint.Value.f)
					{
						tempPoint = openPoint;
					}
				}				
				
				processingPos = tempPoint.Key;
				openDic.Remove(tempPoint.Key);
				closeDic.Add(tempPoint.Key, tempPoint.Value);
			}

			DrawCloseCell();
		}

		/// <summary>
		/// 检查当前点的周围坐标
		/// </summary>
		/// <returns></returns>
		private bool CheckAround()
		{
			for (var x = -1; x <= 1; x++)
			{
				for (var y = 0; y <= 1; y++)
				{
					if (x == 0 && y == 0)
					{
						continue;
					}

					var processingPointInformation = closeDic[processingPos];
					var posX = processingPos.x + x;
					var posY = processingPos.y + y;
					var pos = new int2(processingPos.x + x, processingPos.y + y);
					if (!pointDic.ContainsKey(pos) || closeDic.ContainsKey(pos))
					{
						continue;
					}

					var costG = Mathf.Abs(x) == Mathf.Abs(y) ? processingPointInformation.g + 14 : processingPointInformation.g + 10;
					var costH = Mathf.Abs(posX - end.x) + Mathf.Abs(posY - end.y);
					var costF = costG + costH;
					var pointInformation = new PointInformation
					{
						g = costG,
						h = costH,
						f = costF,
						parent = processingPos,
					};
					if (AddToOpen(pos, pointInformation))
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// 添加点到OpenDic
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="pointInformation"></param>
		private bool AddToOpen(int2 pos, PointInformation pointInformation)
		{
			if (openDic.ContainsKey(pos) && openDic[pos].g > pointInformation.g)
			{
				openDic[pos] = pointInformation;
			}
			else
			{
				openDic.Add(pos, pointInformation);
			}

			if (pos.Equals(end))
			{
				return true;
			}

			return false;
		}

		private void CreatePath()
		{
			var tempInformation = openDic[end];
			var tempPos = end;

			pathResultList.Add(tempPos);
			while (!tempPos.Equals(start))
			{
				tempPos = tempInformation.parent;
				tempInformation = closeDic[tempPos];
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

			DrawPathCell();
			DrawOpenCell();
			// DrawCloseCell();
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
			foreach (var openPoint in openDic)
			{
				DrawCellByPoint(pointDic[openPoint.Key], Color.yellow, Color.black);
			}
		}
		
		private void DrawCloseCell()
		{
			foreach (var closePoint in closeDic)
			{
				Debug.Log(closePoint.Key);
				// DrawCellByPoint(pointDic[closePoint.Key], Color.black, Color.black);
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
			
			foreach (var point in curMapConfig.PointList)
			{
				DrawCellByPoint(point.value, Color.green, Color.blue);
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
			var mapHalfSize = mapMaxSize / 2;
			
			var tempXPos = 0f;
			while (tempXPos <= mapHalfSize)
			{
				var leftXa = new Vector3(tempXPos, mapHeight, mapHalfSize);
				var leftXb = new Vector3(tempXPos, mapHeight, -mapHalfSize);
				Handles.DrawLine(leftXa, leftXb);
				
				var rightXa = new Vector3(-tempXPos, mapHeight, mapHalfSize);
				var rightXb = new Vector3(-tempXPos, mapHeight, -mapHalfSize);
				Handles.DrawLine(rightXa, rightXb);

				tempXPos += cellSize.x;
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
				
				tempYPos += cellSize.y;
			}
		}
		
		private void DrawCellByPoint(Vector3 point, Color faceColor, Color outlineColor)
		{
			var vertArray = new[]
			{
				new Vector3(point.x - cellSize.x / 2, point.y, point.z - cellSize.y / 2),
				new Vector3(point.x - cellSize.x / 2, point.y, point.z + cellSize.y / 2),
				new Vector3(point.x + cellSize.x / 2, point.y, point.z + cellSize.y / 2),
				new Vector3(point.x + cellSize.x / 2, point.y, point.z - cellSize.y / 2),
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
		/// 根据坐标获取当前配置PointList中的索引
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		private int GetCurMapConfigPointIndex(float x, float y, float z)
		{
			var configPointList = curMapConfig.PointList;
			for (var index = 0; index < configPointList.Count; index++)
			{
				var realPosition = configPointList[index].value;
				if (realPosition.x.Equals(x) && realPosition.y.Equals(y) && realPosition.z.Equals(z))
				{
					return index;
				}
			}

			return -1;
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
				var posX = Mathf.CeilToInt(rh.point.x / cellSize.x) * cellSize.x - cellSize.x / 2;
				var posZ = Mathf.CeilToInt(rh.point.z / cellSize.y) * cellSize.y - cellSize.y / 2;
				mousePose = new Vector3(posX, mapHeight, posZ);

				return true;
			}
			
			return false;
		}
	}
}
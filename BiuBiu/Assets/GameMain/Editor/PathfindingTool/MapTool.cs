//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace BiuBiu.Editor
{
	/// <summary>
	/// 配置寻路网格工具
	/// </summary>
	public class MapTool : EditorWindow
	{
		private GameObject planeObj;
		private MapConfig curMapConfig;
		private Vector2 cellSize = Vector2.one;
		private float mapHeight = 0f;
		private float mapMaxSize = 100f;

		private Vector3 mouseMovePos;
		private Vector3 mouseDownPos;
		private Vector3 mouseUpPos;
		private MapCellType curCellType = MapCellType.None;
		private bool isDragging;
		
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
				curMapConfig = value;
				if (curMapConfig != null)
				{
					cellSize = curMapConfig.CellSize;
					mapHeight = curMapConfig.MapHeight;
					mapMaxSize = curMapConfig.MapMaxSize;
				}
			}
		}

		/// <summary>
		/// 设置格子大小
		/// </summary>
		public Vector2 CellSize
		{
			set
			{
				var x = value.x < 0 ? 0 : value.x;
				var y = value.y < 0 ? 0 : value.y;
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
		
		[MenuItem("BiuBiu/地图网格编辑工具", false, 100)]
		private static void Open()
		{
			var window = GetWindow<MapTool>(true, "地图网格编辑工具", true);
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
			CurMapConfig = (MapConfig) EditorGUILayout.ObjectField("分组配置文件", CurMapConfig, typeof(MapConfig), false);
			MapHeight = EditorGUILayout.FloatField("网格高度", mapHeight);
			MapMaxSize = EditorGUILayout.FloatField("网格尺寸", mapMaxSize);
			cellSize = EditorGUILayout.Vector2Field("格子大小", cellSize);
			curCellType = (MapCellType) EditorGUILayout.EnumPopup("格子类型", curCellType);
			if (GUILayout.Button("新建地图网格配置"))
			{
				CurMapConfig = MapConfig.CreateDefaultConfig(null);
			}
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
			SceneView.RepaintAll();
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
				var vertArray = new[]
				{
					new Vector3(point.x - cellSize.x / 2, point.y, point.z - cellSize.y / 2),
					new Vector3(point.x - cellSize.x / 2, point.y, point.z + cellSize.y / 2),
					new Vector3(point.x + cellSize.x / 2, point.y, point.z + cellSize.y / 2),
					new Vector3(point.x + cellSize.x / 2, point.y, point.z - cellSize.y / 2),
				};
				Handles.DrawSolidRectangleWithOutline(vertArray, Color.green, Color.blue);
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

			DrawCellByPoint(mouseMovePos);
		}

		/// <summary>
		/// 绘制鼠标拖拽选中格子
		/// </summary>
		private void DrawMouseSelectCell()
		{
			if (!isDragging)
			{
				return;
			}
			
			var startX = Math.Min(mouseDownPos.x, mouseMovePos.x);
			var startZ = Math.Min(mouseDownPos.z, mouseMovePos.z);
			var endX = Math.Max(mouseDownPos.x, mouseMovePos.x);
			var endZ = Math.Max(mouseDownPos.z, mouseMovePos.z);;
			for (var x = startX; x <= endX; x += cellSize.x)
			{
				for (var z = startZ; z <= endZ; z += cellSize.y)
				{
					DrawCellByPoint(new Vector3(x, mapHeight, z));
				}
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
		
		private void DrawCellByPoint(Vector3 point)
		{
			var vertArray = new[]
			{
				new Vector3(point.x - cellSize.x / 2, point.y, point.z - cellSize.y / 2),
				new Vector3(point.x - cellSize.x / 2, point.y, point.z + cellSize.y / 2),
				new Vector3(point.x + cellSize.x / 2, point.y, point.z + cellSize.y / 2),
				new Vector3(point.x + cellSize.x / 2, point.y, point.z - cellSize.y / 2),
			};
			Handles.DrawSolidRectangleWithOutline(vertArray, new Color(0f, 1f, 0f, 0.1f), Color.blue);
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
					
					isDragging = true;
					break;
				}
				case EventType.MouseUp when Event.current.button == 0:
				{
					if (TryGetMousePositionInCell(out var mousePose))
					{
						mouseUpPos = mousePose;
					}
					else
					{
						mouseUpPos = mouseMovePos;
					}

					if (isDragging)
					{
						WriteSelectCellToConfig();
					}
					
					isDragging = false;
					break;
				}
			}
		}

		/// <summary>
		/// 写入上一次拖拽选择的结果到当前编辑的MapConfig
		/// </summary>
		private void WriteSelectCellToConfig()
		{
			if (curMapConfig == null)
			{
				Debug.Log("Map Tool : Please select a map config first.");
				return;
			}
			
			var startX = Math.Min(mouseDownPos.x, mouseUpPos.x);
			var startZ = Math.Min(mouseDownPos.z, mouseUpPos.z);
			var endX = Math.Max(mouseDownPos.x, mouseUpPos.x);
			var endZ = Math.Max(mouseDownPos.z, mouseUpPos.z);
			
			var configPointList = curMapConfig.PointList;
			for (var x = startX; x <= endX; x += cellSize.x)
			{
				for (var z = startZ; z <= endZ; z += cellSize.y)
				{
					var indexInPointList = GetCurMapConfigPointIndex(x, mapHeight, z);
					switch (curCellType)
					{
						case MapCellType.None:
						{
							if (indexInPointList != -1)
							{
								configPointList.RemoveAt(indexInPointList);
							}

							break;
						}
						case MapCellType.NormalFloor:
						{
							if (indexInPointList == -1)
							{
								configPointList.Add(new float3(x, mapHeight, z));
							}

							break;
						}
					}
				}
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
				var point = configPointList[index];
				if (point.x.Equals(x) && point.y.Equals(y) && point.z.Equals(z))
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
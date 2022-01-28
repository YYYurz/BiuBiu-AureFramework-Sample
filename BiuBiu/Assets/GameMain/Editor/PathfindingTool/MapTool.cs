//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace BiuBiu.Editor
{
	public class MapTool : EditorWindow
	{
		private GameObject planeObj;
		private MapConfig mapConfig;
		private Vector3[] vertArray = null;
		private Vector2 cellSize = Vector2.one;
		
		/// <summary>
		/// 当前编辑的地图配置
		/// </summary>
		private MapConfig CurrentGroupConfig
		{
			get
			{
				return mapConfig;
			}
			set
			{
				mapConfig = value;
				ReadGroupConfig();
			}
		}
		
		[MenuItem("BiuBiu/地图", false, 100)]
		private static void Open()
		{
			var window = GetWindow<MapTool>(true, "导表工具", true);
			window.minSize = window.maxSize = new Vector2(500f, 500f);
		}
		
		private void OnEnable()
		{
			SceneView.duringSceneGui += OnSceneGUI;
			Tools.hidden = true;
			
			planeObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			planeObj.GetComponent<MeshRenderer>().enabled = false;
			planeObj.transform.localScale = new Vector3(10000f, 1f, 10000f);
			planeObj.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
		}

		private void OnGUI()
		{
			// CurrentGroupConfig = (MapConfig) EditorGUILayout.ObjectField("分组配置文件", CurrentGroupConfig, typeof(MapConfig), false);
			cellSize = EditorGUILayout.Vector2Field("格子大小", cellSize);
			if (GUILayout.Button("生成"))
			{
				// var a = MapConfig.CreateDefaultConfig(null);
				ReadGroupConfig();
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
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			// switch (Event.current.type)
			// {
			// 	case EventType.MouseDown when Event.current.button == 0:
			// 	{
			// 		OnMouseEvent();
			// 		break;
			// 	}
			// 	case EventType.MouseDrag when Event.current.button == 0:
			// 	{
			// 		OnMouseEvent();
			// 		break;
			// 	}
			// 	case EventType.MouseUp when Event.current.button == 0:
			// 	{
			// 		vertArray = null;
			// 		break;
			// 	}
			// }
			OnMouseEvent();

			GL.Flush();
			if (vertArray != null)
			{
				Handles.DrawSolidRectangleWithOutline(vertArray, Color.green, Color.blue);
			}
			
			SceneView.RepaintAll();
		}

		private void ReadGroupConfig()
		{
			
		}
		
		private void OnMouseEvent()
		{
			var mousePos = Event.current.mousePosition;
			mousePos.y = Camera.current.pixelHeight - mousePos.y;
			var ray = Camera.current.ScreenPointToRay(mousePos);
			if (Physics.Raycast(ray, out var rh, 3000f))
			{
				vertArray = new[]
				{
					new Vector3(rh.point.x - cellSize.x, 0, rh.point.z - cellSize.y), 
					new Vector3(rh.point.x - cellSize.x, 0, rh.point.z + cellSize.y), 
					new Vector3(rh.point.x + cellSize.x, 0, rh.point.z + cellSize.y), 
					new Vector3(rh.point.x + cellSize.x, 0, rh.point.z - cellSize.y), 
				};
			}
		}
	}
}
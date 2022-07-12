//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using AureFramework.Editor;
using AureFramework.Procedure;
using Unity.Entities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace BiuBiu.Editor
{
	[CustomEditor(typeof(EcsModule))]
	public class EntityModuleInspector : AureFrameworkInspector
	{
		private SerializedProperty entityPresetList;
		
		private readonly List<string> allComponentDataTypeList = new List<string>();
		private readonly List<string> searchComponentDataTypeList = new List<string>();
		private string searchWord = "";
		private int index;

		private string SearchWord
		{
			set
			{
				if (searchWord.Equals(value))
				{
					return;
				}
				
				searchWord = value;
				RefreshSearchComponentDataTypeList();
			}
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			EditorGUIUtility.LookLikeInspector();
			GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
			SearchWord = GUILayout.TextField(searchWord, GUI.skin.FindStyle("ToolbarSeachTextField"));
			if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
			{
				SearchWord = "";
			}
			GUILayout.EndHorizontal();
			var selectedIndex = EditorGUILayout.Popup("ComponentData Type", index, searchComponentDataTypeList.ToArray());
			if (selectedIndex != index)
			{
				SearchWord = searchComponentDataTypeList[selectedIndex];
			}
			
			EditorGUILayout.PropertyField(entityPresetList);

			serializedObject.ApplyModifiedProperties();
			Repaint();
		}

		private void OnEnable()
		{
			entityPresetList = serializedObject.FindProperty("entityPresetList");
			RefreshComponentDataTypeList();
			RefreshSearchComponentDataTypeList();
		}

		private void RefreshComponentDataTypeList()
		{
			allComponentDataTypeList.Clear();
			
			var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in allAssemblies)
			{
				var allTypes = assembly.GetTypes();
				foreach (var type in allTypes)
				{
					if (typeof(IComponentData).IsAssignableFrom(type) || typeof(ISharedComponentData).IsAssignableFrom(type))
					{
						allComponentDataTypeList.Add(type.FullName);
					}
				}
			}
		}

		private void RefreshSearchComponentDataTypeList()
		{
			searchComponentDataTypeList.Clear();

			var lowerValue = searchWord.ToLower();
			foreach (var componentDataType in allComponentDataTypeList)
			{
				var lowerComponentDataType = componentDataType.ToLower();
				if (lowerComponentDataType.Contains(lowerValue) || searchWord.Equals(""))
				{
					searchComponentDataTypeList.Add(componentDataType);
				}
			}
		}
	}
}
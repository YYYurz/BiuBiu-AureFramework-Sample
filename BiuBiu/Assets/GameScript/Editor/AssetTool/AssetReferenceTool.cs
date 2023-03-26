//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DrunkFish
{
	public class AssetReferenceTool : EditorWindow
	{
		private readonly List<string> referenceList = new List<string>();
		private readonly List<string> referenceAreaList = new List<string>();
		private Object selectAsset;
		private Object referenceArea;

		private bool isReference = true;
		private Vector2 scrollPos;
		
		[MenuItem("BiuBiu/资源引用查找工具", false, 120)]
		private static void Open()
		{
			var window = GetWindow<AssetReferenceTool>(true, "资源引用查找工具", true);
			window.minSize = window.maxSize = new Vector2(400f, 500f);
		}
		
		private void OnGUI()
		{
			selectAsset = EditorGUILayout.ObjectField("要查找的资源：", selectAsset, typeof(Object), false);
			referenceArea = EditorGUILayout.ObjectField("查找范围：", referenceArea, typeof(DefaultAsset), false);
			isReference = EditorGUILayout.ToggleLeft("引用的资源", isReference);
			
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(380f), GUILayout.Height(380f));
			{
				if (!isReference && referenceAreaList.Count != 0)
				{
					foreach (var areaAssetPath in referenceAreaList)
					{
						if (!referenceList.Contains(areaAssetPath))
						{
							var referenceAsset = AssetDatabase.LoadAssetAtPath<Object>(areaAssetPath);
							EditorGUILayout.ObjectField(referenceAsset, typeof(Object), false);
						}
					}
				}
				else
				{
					foreach (var referenceAssetPath in referenceList)
					{
						var referenceAsset = AssetDatabase.LoadAssetAtPath<Object>(referenceAssetPath);
						EditorGUILayout.ObjectField(referenceAsset, typeof(Object), false);
					}	
				}
			}
			EditorGUILayout.EndScrollView();

			if (GUILayout.Button("查找"))
			{
				CheckReference();
			}
			
			if (GUILayout.Button("删除未引用资源"))
			{
				DeleteNotReferenceAssets();
			}
		}

		/// <summary>
		/// 在检查范围内检查选中资源引用的所有资源（未选择检查范围默认全部资源）
		/// </summary>
		private void CheckReference()
		{
			if (selectAsset == null)
			{
				Debug.LogError("AssetReferenceTool : Please select a asset first.");
				return;
			}

			GetAreaAssetList();
			referenceList.Clear();
			var assetPath = AssetDatabase.GetAssetPath(selectAsset);
			
			if (selectAsset is DefaultAsset)
			{
				var assets= Directory.GetFiles(assetPath, "*.*", SearchOption.AllDirectories).Where((s => !s.Contains(".meta")));
				foreach (var asset in assets)
				{
					var dependencies = AssetDatabase.GetDependencies(asset);
					dependencies = dependencies.Except(referenceList).ToArray();
					if (referenceAreaList.Count > 0)
					{
						dependencies = dependencies.Intersect(referenceAreaList).ToArray();
					}
					referenceList.AddRange(dependencies);
				}
			}
			else
			{
				var dependencies = AssetDatabase.GetDependencies(assetPath);
				if (referenceAreaList.Count > 0)
				{
					dependencies = dependencies.Intersect(referenceAreaList).ToArray();
				}
				referenceList.AddRange(dependencies);
			}
			
			referenceList.Remove(assetPath);
		}

		/// <summary>
		/// 获取查找范围内的所有资源
		/// </summary>
		private void GetAreaAssetList()
		{
			referenceAreaList.Clear();
			
			if (referenceArea != null && referenceArea is DefaultAsset)
			{
				var assetPath = AssetDatabase.GetAssetPath(referenceArea);
				var assets= Directory.GetFiles(assetPath, "*.*", SearchOption.AllDirectories).Where((s => !s.Contains(".meta")));
				referenceAreaList.AddRange(assets);

				for (var i = 0; i < referenceAreaList.Count; i++)
				{
					referenceAreaList[i] = referenceAreaList[i].Replace("\\", "/");
				}
			}
		}

		/// <summary>
		/// 删除检查出来未被引用的资源（不选择检查范围默认无法删除）
		/// </summary>
		private void DeleteNotReferenceAssets()
		{
			if (referenceAreaList.Count > 0)
			{
				for (var i = referenceAreaList.Count - 1; i >= 0; i--)
				{
					var path = referenceAreaList[i];
					if (!referenceList.Contains(path))
					{
						AssetDatabase.DeleteAsset(path);
						referenceAreaList.RemoveAt(i);
					}
				}
			}
		}
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BiuBiu.Editor
{
	public class TempTool : EditorWindow
	{
		[MenuItem("BiuBiu/临时工具", false, 150)]
		private static void Open()
		{
			var window = GetWindow<TempTool>(true, "导表工具", true);
			window.minSize = window.maxSize = new Vector2(500f, 500f);
		}

		private void OnGUI()
		{
			if (GUILayout.Button("A"))
			{
				Run();
			}
		}

		private void Run()
		{
			var fileInfoList = Directory.GetFiles("D:\\Study\\UnityProject\\BiuBiu\\BiuBiu\\Assets\\GameAssets\\LuaScripts");
			foreach (var filePath in fileInfoList)
			{
				var fileInfo = new FileInfo(filePath);
				fileInfo.MoveTo(Path.ChangeExtension(filePath, ".lua"));
			}
		}
	}
}
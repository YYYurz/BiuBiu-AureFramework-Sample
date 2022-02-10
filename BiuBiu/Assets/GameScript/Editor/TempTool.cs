//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.IO;
using System.Text;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace BiuBiu.Editor
{
	public class TempTool : EditorWindow
	{
		[MenuItem("BiuBiu/临时工具", false, 150)]
		private static void OpenWindow()
		{
			var window = GetWindow<TempTool>(true, "导表工具", true);
			window.minSize = window.maxSize = new Vector2(500f, 500f);
		}

		private void OnGUI()
		{
			if (GUILayout.Button("Test"))
			{
				Test();
			}
			
			if (GUILayout.Button("修改文本文件的编码格式"))
			{
				ModifyCodeFormat();
			}
		}

		private void Test()
		{
			var a = new int2(1, 1);
			var b = new int2(1, 1);
			if (a.Equals(b))
			{
				Debug.Log("HaHah");
			}
		}

		private void ModifyCodeFormat()
		{
			var fileInfoList = Directory.GetFiles("D:\\Study\\UnityProject\\BiuBiu\\BiuBiu\\Assets\\GameAssets\\LuaScripts", "*.lua", SearchOption.AllDirectories);
			foreach (var filePath in fileInfoList)
			{
				if (filePath.Contains(".meta"))
				{
					continue;
				}
				
				//以UTF-8带BOM格式读取文件内容
				var end = new UTF8Encoding(true);
				var str = string.Empty;
				using (var sr = new StreamReader(filePath, end))
				{
					str = sr.ReadToEnd();
				}
				//以UTF-8不带BOM格式重新写入文件
				end = new UTF8Encoding(false);
				using (var sw = new StreamWriter(filePath, false, end))
				{
					sw.Write(str);
				}
			}
		}
	}
}
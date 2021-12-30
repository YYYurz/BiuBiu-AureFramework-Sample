#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Diagnostics;
using System.Linq;

namespace BiuBiu.Editor
{
	public class Excel2FlatBuffersTool : EditorWindow
	{
		private readonly bool[] isSelect = new bool[100];
		private readonly List<string> selectFileList = new List<string>();

		private string selectFolderPath = "";
		private string helpMessage = "选择Excel文件生成二进制文件与读表类";

		private Vector2 scrollPos;


		[MenuItem("BiuBiu/导表工具", false, 120)]
		private static void Open()
		{
			var window = GetWindow<Excel2FlatBuffersTool>(true, "导表工具", true);
			window.minSize = window.maxSize = new Vector2(500f, 500f);
		}

		private void OnEnable()
		{
			for (var i = 0; i < isSelect.Length; i++)
			{
				isSelect[i] = true;
			}

			selectFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName +
			                   "\\Excel2FlatBuffers\\ExcelTable";
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("目标文件夹：", GUILayout.Width(100f));
				selectFolderPath = EditorGUILayout.TextField(selectFolderPath);
				if (GUILayout.Button("选择其他目录", GUILayout.Width(100f)))
				{
					BrowseOutputDirectory();
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.HelpBox(helpMessage, MessageType.Info);

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("勾选Excel文件：", GUILayout.Width(120));
				if (GUILayout.Button("打开当前目录", GUILayout.Width(150)))
				{
					OpenDirectory(selectFolderPath);
				}

				if (GUILayout.Button("全选", GUILayout.Width(50f)))
				{
					for (var i = 0; i < isSelect.Length; i++)
					{
						isSelect[i] = true;
					}
				}

				if (GUILayout.Button("全不选", GUILayout.Width(50f)))
				{
					for (var i = 0; i < isSelect.Length; i++)
					{
						isSelect[i] = false;
					}
				}
			}
			EditorGUILayout.EndHorizontal();

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500f), GUILayout.Height(385f));
			{
				EditorGUILayout.BeginVertical("box", GUILayout.Width(500f), GUILayout.Height(440f));
				if (Directory.Exists(selectFolderPath))
				{
					var folder = new DirectoryInfo(selectFolderPath);
					var files = folder.GetFiles("*.xlsx");
					selectFileList.Clear();
					for (var i = 0; i < files.Length; i++)
					{
						isSelect[i] = EditorGUILayout.ToggleLeft(files[i].Name, isSelect[i]);
						if (isSelect[i])
						{
							selectFileList.Add(files[i].Name.Replace(".xlsx", ""));
						}
						else
						{
							selectFileList.Remove(files[i].Name.Replace(".xlsx", ""));
						}
					}
				}

				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndScrollView();

			if (GUILayout.Button("生成", GUILayout.Width(500f)))
			{
				Generate();
			}
		}

		private void BrowseOutputDirectory()
		{
			var directory = EditorUtility.OpenFolderPanel("选择文件夹", selectFolderPath, string.Empty);
			if (!string.IsNullOrEmpty(directory))
			{
				selectFolderPath = directory;
			}
		}

		private void OpenDirectory(string path)
		{
			if (string.IsNullOrEmpty(path)) return;

			path = path.Replace("/", "\\");
			if (!Directory.Exists(path))
			{
				return;
			}

			Process.Start("explorer.exe", path);
		}

		private void Generate()
		{
			if (!isSelect.Contains(true))
			{
				helpMessage = "未选择文件！！！！！！";
				return;
			}

			File.WriteAllText(Path.GetFullPath("..") + "\\Excel2FlatBuffers\\ChooseExcel.txt", string.Empty);
			var sw = new StreamWriter(Path.GetFullPath("..") + "\\Excel2FlatBuffers\\ChooseExcel.txt");
			for (var i = 0; i < selectFileList.Count; i++)
			{
				sw.WriteLine(selectFileList[i]);
			}

			sw.Flush();
			sw.Close();
			Run();
		}

		private void Run()
		{
			var startInfo = new ProcessStartInfo("python")
			{
				WorkingDirectory = Path.GetFullPath("..") + "\\Excel2FlatBuffers", Arguments = "一键生成.py"
			};
			Process.Start(startInfo);

			helpMessage = "选择Excel文件生成二进制文件与读表类";
		}
	}
}

#endif
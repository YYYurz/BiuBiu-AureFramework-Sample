//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace BiuBiu.Editor
{
	public class Excel2FlatBuffersTool : EditorWindow
	{
		private readonly List<string> allFileList = new List<string>();
		private readonly List<string> selectFileList = new List<string>();

		private string selectFolderPath;
		private bool isSelectAll;

		private Vector2 scrollPos;

		/// <summary>
		/// 设置是否全选
		/// </summary>
		private bool IsSelectAll
		{
			get
			{
				return isSelectAll;
			}
			set
			{
				if (isSelectAll.Equals(value))
				{
					return;
				}
				
				selectFileList.Clear();
				if (value)
				{
					selectFileList.AddRange(allFileList);	
				}

				isSelectAll = value;
			}
		}

		/// <summary>
		/// 获取或设置Excel表格文件夹路径
		/// </summary>
		private string ExcelDirectory
		{
			get
			{
				return selectFolderPath;
			}
			set
			{
				if (!string.IsNullOrEmpty(selectFolderPath) && selectFolderPath.Equals(value))
				{
					return;
				}

				var folder = new DirectoryInfo(value);
				var fileInfoList = folder.GetFiles("*.xlsx");
				allFileList.Clear();
				foreach (var fileInfo in fileInfoList)
				{
					if (fileInfo.Name.Contains("~$"))
					{
						continue;
					}
					
					allFileList.Add(fileInfo.Name.Replace(".xlsx", ""));
				}
				
				selectFolderPath = value;
			}
		}

		[MenuItem("BiuBiu/导表工具", false, 120)]
		private static void Open()
		{
			var window = GetWindow<Excel2FlatBuffersTool>(true, "导表工具", true);
			window.minSize = window.maxSize = new Vector2(500f, 500f);
		}

		private void OnEnable()
		{
			ExcelDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\Excel2FlatBuffers\\ExcelTable";
			IsSelectAll = true;
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

			EditorGUILayout.HelpBox("选择Excel文件生成二进制文件与读表类", MessageType.Info);
			{
				EditorGUILayout.BeginHorizontal();
				{
					IsSelectAll = EditorGUILayout.ToggleLeft("全选", IsSelectAll);

					if (GUILayout.Button("打开当前目录", GUILayout.Width(150)))
					{
						OpenDirectory(selectFolderPath);
					}
				}
				EditorGUILayout.EndHorizontal();
			}

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500f), GUILayout.Height(385f));
			{
				EditorGUILayout.BeginVertical("box", GUILayout.Width(500f), GUILayout.Height(440f));
				if (Directory.Exists(selectFolderPath))
				{
					foreach (var fileName in allFileList)
					{
						var isSelect = selectFileList.Contains(fileName);
						if (isSelect != EditorGUILayout.ToggleLeft(fileName, isSelect))
						{
							if (!isSelect)
							{
								selectFileList.Add(fileName);
								if (selectFileList.Count == allFileList.Count)
								{
									isSelectAll = true;
								}
							}
							else
							{
								selectFileList.Remove(fileName);
								isSelectAll = false;
							}
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
			var directory = EditorUtility.OpenFolderPanel("选择文件夹", ExcelDirectory, string.Empty);
			if (!string.IsNullOrEmpty(directory))
			{
				ExcelDirectory = directory;
			}
		}

		private static void OpenDirectory(string directory)
		{
			if (string.IsNullOrEmpty(directory))
			{
				return;
			}

			directory = directory.Replace("/", "\\");
			if (!Directory.Exists(directory))
			{
				return;
			}

			Process.Start("explorer.exe", directory);
		}

		private void Generate()
		{
			if (selectFileList.Count == 0)
			{
				Debug.LogError("请先勾选文件！");
				return;
			}

			File.WriteAllText(Path.GetFullPath("..") + "\\Excel2FlatBuffers\\ChooseExcel.txt", string.Empty);
			var sw = new StreamWriter(Path.GetFullPath("..") + "\\Excel2FlatBuffers\\ChooseExcel.txt");
			for (var i = 0; i < allFileList.Count; i++)
			{
				sw.WriteLine(allFileList[i]);
			}

			sw.Flush();
			sw.Close();
			
			RunPythonScript();
		}

		private static void RunPythonScript()
		{
			var startInfo = new ProcessStartInfo("python")
			{
				WorkingDirectory = Path.GetFullPath("..") + "\\Excel2FlatBuffers", Arguments = "一键生成.py"
			};
			Process.Start(startInfo);
		}
	}
}

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Diagnostics;

namespace BB.Editor
{
    public class Excel2FlatBuffersTool : EditorWindow
    {
        Vector2 scrollPos;

        private string SelectFolderPath = "";

        List<string> filesName;

        private string helpMessage = "选择Excel文件生成二进制文件与读表类";

        private bool[] isSelect = new bool[30];

        [MenuItem("BoxBox/Excel2FlatBuffersTool", false, 120)]
        private static void Open()
        {
            Excel2FlatBuffersTool window = GetWindow<Excel2FlatBuffersTool>(true, "Excel2FlatBuffers工具", true);
            window.minSize = window.maxSize = new Vector2(500f, 500f);
        }

        void OnEnable()
        {
            for (int i = 0; i < isSelect.Length; i++)
            {
                isSelect[i] = true;
            }

            SelectFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName +
                               "\\Excel2FlatBuffers\\ExcelTable";
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("目标文件夹：", GUILayout.Width(100f));
                SelectFolderPath = EditorGUILayout.TextField(SelectFolderPath);
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
                    OpenDirectory(SelectFolderPath);
                }

                if (GUILayout.Button("全选", GUILayout.Width(50f)))
                {
                    for (int i = 0; i < isSelect.Length; i++)
                    {
                        isSelect[i] = true;
                    }
                }

                if (GUILayout.Button("全不选", GUILayout.Width(50f)))
                {
                    for (int i = 0; i < isSelect.Length; i++)
                    {
                        isSelect[i] = false;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500f), GUILayout.Height(385f));
            {
                EditorGUILayout.BeginVertical("box", GUILayout.Width(500f), GUILayout.Height(440f));
                if (Directory.Exists(SelectFolderPath))
                {
                    DirectoryInfo folder = new DirectoryInfo(SelectFolderPath);
                    var files = folder.GetFiles("DT*.xlsx");
                    filesName = new List<string>();
                    for (int i = 0; i < files.Length; i++)
                    {
                        isSelect[i] = EditorGUILayout.ToggleLeft(files[i].Name, isSelect[i]);
                        if (isSelect[i])
                        {
                            filesName.Add(files[i].Name.Replace(".xlsx", ""));
                        }
                        else
                        {
                            filesName.Remove(files[i].Name.Replace(".xlsx", ""));
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

        // 选择文件夹
        private void BrowseOutputDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("选择文件夹", SelectFolderPath, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                SelectFolderPath = directory;
            }
        }

        // 打开当前目录
        private void OpenDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            path = path.Replace("/", "\\");
            if (!Directory.Exists(path))
            {
                return;
            }

            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        // 生成
        private void Generate()
        {
            bool isTrue = false;
            for (int i = 0; i < isSelect.Length; i++)
            {
                if (isSelect[i] == true)
                {
                    isTrue = true;
                    break;
                }
            }

            if (!isTrue)
            {
                helpMessage = "未选择文件！！！！！！";
                return;
            }

            System.IO.File.WriteAllText(Path.GetFullPath("..") + "\\Excel2FlatBuffers\\ChooseExcel.txt", string.Empty);
            StreamWriter sw = new StreamWriter(Path.GetFullPath("..") + "\\Excel2FlatBuffers\\ChooseExcel.txt");
            for (int i = 0; i < filesName.Count; i++)
            {
                sw.WriteLine(filesName[i]);
            }

            sw.Flush();
            sw.Close();
            Run();
        }

        private void Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("python");
            string WorkingPath = Path.GetFullPath("..") + "\\Excel2FlatBuffers";
            startInfo.WorkingDirectory = WorkingPath;
            startInfo.Arguments = "一键生成.py";
            Process.Start(startInfo);
            helpMessage = "选择Excel文件生成二进制文件与读表类";
        }
    }
}

#endif
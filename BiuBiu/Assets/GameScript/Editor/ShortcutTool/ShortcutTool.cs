//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace DrunkFish
{
	public static class ShortcutTool
	{
		public static Action<int> Action;

		[MenuItem("Shortcut/F12 _F12")]
		private static void F12()
		{
			Action?.Invoke(1);
		}
	}
}
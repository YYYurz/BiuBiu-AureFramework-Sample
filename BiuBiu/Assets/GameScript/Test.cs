//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using XLua;

namespace BiuBiu
{
	public class Test : EditorWindow
	{
		[MenuItem("BiuBiu/Test", false, 120)]
		private static void Open()
		{
			var window = GetWindow<Test>(true, "Test", true);
			window.minSize = window.maxSize = new Vector2(400f, 500f);
		}



		private GameObject testObj;
		
		private void OnGUI()
		{
			testObj = (GameObject) EditorGUILayout.ObjectField(testObj, typeof(GameObject));
			if (GUILayout.Button("Test"))
			{
				var teList = new List<TroopItem>();
				teList.Add(new TroopItem{load = 1, own_num = 10});
				teList.Add(new TroopItem{load = 2, own_num = 10});
				teList.Add(new TroopItem{load = 3, own_num = 10});
				
				var re = QuickSelectTroopList(10000, 100, teList);

				foreach (var r in re)
				{
					Debug.Log(r.load + "  " + r.own_num + "  " + r.select_num);
				}
			}
		}

		[Hotfix()]
		public struct TroopItem
		{
			int id;  int type;
			int level;  public int load;
			int force;  public int own_num;  public int select_num;
		}


		List<TroopItem> QuickSelectTroopList(int res_max, int march_size_max, List<TroopItem> own_troop_list)
		{
			// 按士兵load量降序排
			own_troop_list.Sort((a, b) => b.load.CompareTo(a.load));
	
			var temp_res = res_max;	// 最大资源量
			var temp_size = march_size_max;	// 最大派兵量
			var result = new List<TroopItem>();
			foreach (var troopItem in own_troop_list)
			{
				// 剩余资源 < 0 或 剩余派兵限制 < 0 主动跳出
				if (temp_size <= 0 || temp_res <= 0)
				{
					break;
				}

				// 计算这个兵种派出量
				var select_num = Mathf.CeilToInt(temp_res / (float) troopItem.load);
				select_num = Mathf.Min(select_num, temp_size);
				select_num = Mathf.Min(select_num, troopItem.own_num);

				// struct 值类型直接拷贝一份
				var item = troopItem;
				item.select_num = select_num;
				
				result.Add(item);

				// 剩余资源以及剩余派兵上限减去当前兵种影响
				temp_res -= select_num * troopItem.load;
				temp_size -= select_num;
			}

			return result;
		}

		public void Te(List<TroopItem> QuickSelectTroopList)
		{
			// QuickSelectTroopList.Sort((a, b) => b.load.CompareTo(a.load));
			var result = new List<TroopItem>();
			var t = QuickSelectTroopList[0];
			t.load = 2;
			result.Add(t);

			MethodInfo a;
			
			Debug.Log(QuickSelectTroopList[0].load);
			Debug.Log(result[0].load);
		}
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ObjectPool;
using UnityEngine;
using XLua;

namespace BiuBiu
{
	/// <summary>
	/// 滚动列表Item
	/// </summary>
	public class UIContentItem : ObjectBase
	{
		private GameObject itemGameObj;
		private RectTransform rectTransform;
		private LuaTable itemTable;
		private LuaTable controllerBase;
		
		public RectTransform RectTransform
		{
			get
			{
				return rectTransform;
			}
		}
		
		/// <summary>
		/// Item的Lua类
		/// </summary>
		public LuaTable ItemTable
		{
			get
			{
				return itemTable;
			}
			set
			{
				itemTable = value;
			}
		}
		
		public void OnInit(int itemIndex)
		{
			itemTable.Set("controller", controllerBase);
			itemTable.Set("gameObject", itemGameObj);
			itemTable.Set("itemIndex", itemIndex);
			GameMain.Lua.CallLuaFunction(itemTable, "OnInit", null, itemTable);
		}

		public void OnRefresh()
		{
			GameMain.Lua.CallLuaFunction(itemTable, "OnRefresh", null, itemTable);
		}

		public override void OnSpawn()
		{
			itemGameObj.SetActive(true);
		}

		public override void OnRecycle()
		{
			itemGameObj.SetActive(false);
		}

		public override void OnRelease()
		{
			GameMain.Lua.CallLuaFunction(itemTable, "OnRelease", null, itemTable);
		}
		
		public static UIContentItem Create(GameObject gameObj, LuaTable controllerBase)
		{
			var uiContentItem = GameMain.ReferencePool.Acquire<UIContentItem>();
			var rectTransform = gameObj.GetComponent<RectTransform>();
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			uiContentItem.itemGameObj = gameObj;
			uiContentItem.rectTransform = rectTransform;
			uiContentItem.controllerBase = controllerBase;
			
			gameObj.SetActive(true);
			
			return uiContentItem;
		}
	}
}
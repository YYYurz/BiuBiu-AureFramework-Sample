//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.ObjectPool;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace BiuBiu
{
	/// <summary>
	/// 滚动列表
	/// </summary>
	[RequireComponent(typeof(ScrollRect))]
	public class UIContentList : MonoBehaviour
	{
		private readonly Dictionary<int, UIContentItem> usingItemObjectDic = new Dictionary<int, UIContentItem>();
		private readonly List<int> recyclingItemIndexList = new List<int>();
		private readonly List<LuaTable> itemLuaScriptList = new List<LuaTable>();
		private readonly List<int> curShowItemIndexList = new List<int>();
		private IObjectPool<UIContentItem> itemObjectPool;
		private ScrollRect scrollRect;
		private RectTransform content;
		private int oneLineItemCount;
		private int lineCount;
		
		private LuaTable controller;

		private string poolName;
		private static int PoolNameIndex = 0;
		private const string ItemObjectName = "ItemObject";

		[SerializeField] private bool horizontal;
		[SerializeField] private bool vertical;
		[SerializeField] private Vector2 spacing;
		[SerializeField] private RectTransform itemPrefab;
		[SerializeField] private string itemLuaScript;


		protected void Awake()
		{
			scrollRect = GetComponent<ScrollRect>();
			scrollRect.horizontal = horizontal;
			scrollRect.vertical = vertical;

			content = scrollRect.content;
			content.pivot = horizontal ? new Vector2(0f, 0.5f) : new Vector2(0.5f, 1f);
			content.anchorMin = horizontal ? Vector2.zero : new Vector2(0f, 1f);
			content.anchorMax = horizontal ? new Vector2(0f, 1f) : new Vector2(1f, 1f);
			content.anchoredPosition = Vector2.zero;
			content.sizeDelta =  Vector2.zero;
			content.localScale = Vector3.one;

			poolName = "UIContentList_" + ++PoolNameIndex;
			itemObjectPool = GameMain.ObjectPool.CreateObjectPool<UIContentItem>(poolName, int.MaxValue, 300);
		}

		private void Update()
		{
			RecycleItem();
			CalculateItems();
		}

		public void RefreshContentList(int itemCount, LuaTable controllerTable)
		{
			if (itemPrefab == null)
			{
				Debug.LogError("UIContentList : Item prefab is null.");
				return;
			}	
			
			if (controllerTable == null)
			{
				Debug.LogError("UIContentList : Base controller is null.");
				return;
			}

			controller = controllerTable;
			itemLuaScriptList.Clear();
			var itemScript = (LuaTable) GameMain.Lua.DoString($"return require('{itemLuaScript}')")[0];
			for (var i = 1; i <= itemCount; i++)
			{
				var itemTable = (LuaTable) GameMain.Lua.CallLuaFunction(itemScript, "New", new[] {typeof(LuaTable)}, itemLuaScript)[0];
				itemLuaScriptList.Add(itemTable);
			}
			
			CalculateContentSize();
			CalculateItems();
		}
		
		private void CalculateContentSize()
		{
			var contentRect = content.rect;
			var itemRect = itemPrefab.rect;

			float width;
			float height;
			
			if (horizontal)
			{
				oneLineItemCount = Mathf.FloorToInt((contentRect.height + spacing.y) / (itemRect.height + spacing.y));
				lineCount = Mathf.CeilToInt(itemLuaScriptList.Count / oneLineItemCount);
				width = lineCount * itemRect.width + lineCount * spacing.x;
				height = 0f;
			}
			else
			{
				oneLineItemCount = Mathf.FloorToInt((contentRect.width + spacing.x) / (itemRect.width + spacing.x));
				lineCount = Mathf.CeilToInt(itemLuaScriptList.Count / oneLineItemCount);
				width = 0f;
				height = lineCount * itemRect.height + lineCount * spacing.y;
			}
			
			content.sizeDelta = new Vector2(width, height);
		}
		
		private void CalculateItems()
		{
			if (CalculateNeedShowItemIndexList())
			{
				return;
			}

			RefreshItems();
		}
		
		/// <summary>
		/// 计算所有需要显示的Item的Index
		/// </summary>
		/// <returns></returns>
		private bool CalculateNeedShowItemIndexList()
		{
			//处理0
			
			var contentAnchoredPosition = content.anchoredPosition;
			var contentRect = content.rect;
			var itemRect = itemPrefab.rect;

			var startLine = Mathf.CeilToInt((contentAnchoredPosition.y + spacing.y) / (itemRect.y + spacing.y));
			startLine = startLine <= 0 ? 1 : startLine;

			var offset = startLine * (itemPrefab.rect.y + spacing.y) - spacing.y - contentAnchoredPosition.y;
			var alignmentHeight = contentRect.y - offset;
			var endLine = Mathf.CeilToInt(alignmentHeight / (itemRect.y + spacing.y)) + startLine;

			var startIndex = (startLine - 1) * oneLineItemCount + 1;
			var endIndex = endLine * oneLineItemCount;

			if (startIndex == curShowItemIndexList[0] || endIndex == curShowItemIndexList[curShowItemIndexList.Count - 1])
			{
				return false;
			}

			foreach (var itemIndex in curShowItemIndexList)
			{
				if (itemIndex < startIndex || itemIndex > endIndex)
				{
					recyclingItemIndexList.Add(itemIndex);
				}
			}
			
			curShowItemIndexList.Clear();
			if (itemLuaScriptList.Count > startIndex)
			{
				for (var i = startIndex; i <= endIndex; i++)
				{
					if (i <= itemLuaScriptList.Count)
					{
						curShowItemIndexList.Add(i - 1);
					}
					else
					{
						break;
					}
				}
			}

			return true;
		}

		private void RefreshItems()
		{
			var itemRect = itemPrefab.rect;

			foreach (var itemIndex in curShowItemIndexList)
			{
				var aheadLine = Mathf.FloorToInt(curShowItemIndexList[0] / oneLineItemCount);
				var curLineIndex = itemIndex % oneLineItemCount;
				var xPos = (curLineIndex - 1) * (itemRect.x + spacing.x) + itemRect.x / 2;
				var yPos = aheadLine * (itemRect.y + spacing.y) + itemRect.y / 2;

				if (TryGetItemObject(itemIndex, out var itemObject))
				{
					itemObject.ItemTable = itemLuaScriptList[itemIndex];
					itemObject.OnRefresh();
					itemObject.RectTransform.anchoredPosition = new Vector2(xPos, yPos);
				}
			}
		}

		private void RecycleItem()
		{
			if (recyclingItemIndexList.Count == 0)
			{
				return;
			}
			
			foreach (var itemIndex in recyclingItemIndexList)
			{
				itemObjectPool.Recycle(usingItemObjectDic[itemIndex]);
				usingItemObjectDic.Remove(itemIndex);
			}
			
			recyclingItemIndexList.Clear();
		}

		private bool TryGetItemObject(int itemIndex, out UIContentItem itemObject)
		{
			itemObject = null;
			if (usingItemObjectDic.ContainsKey(itemIndex))
			{
				return false;
			}
			
			if (itemObjectPool.CanSpawn(ItemObjectName))
			{
				itemObject = itemObjectPool.Spawn(ItemObjectName);
				usingItemObjectDic.Add(itemIndex, itemObject);
				
				return true;
			}

			itemObject = CreateItem(itemIndex);
			return true;
		}

		private UIContentItem CreateItem(int itemIndex)
		{
			var itemGameObj = Instantiate(itemPrefab.gameObject, content);
			var itemObject = UIContentItem.Create(itemGameObj, controller);
			itemObjectPool.Register(itemObject, true, ItemObjectName);
			usingItemObjectDic.Add(itemIndex, itemObject);

			return itemObject;
		}

		private void OnDestroy()
		{
			GameMain.ObjectPool.DestroyObjectPool(poolName, itemObjectPool);
		}
	}
}
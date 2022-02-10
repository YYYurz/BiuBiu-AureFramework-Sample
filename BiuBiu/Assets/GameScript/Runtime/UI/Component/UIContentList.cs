//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

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
		private readonly List<int> curShowItemIndexList = new List<int>();
		private readonly List<LuaTable> itemLuaScriptList = new List<LuaTable>();
		private IObjectPool<UIContentItem> itemObjectPool;
		private RectTransform viewPort;
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
			var scrollRect = GetComponent<ScrollRect>();
			scrollRect.horizontal = horizontal;
			scrollRect.vertical = vertical;

			viewPort = scrollRect.viewport;
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
			if (!CalculateNeedShowItemIndexList())
			{
				return;
			}
			
			RecycleItem();
			RefreshItems();
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
				var itemTable = (LuaTable) GameMain.Lua.CallLuaFunction(itemScript, "New", new[] {typeof(LuaTable)}, itemScript)[0];
				itemLuaScriptList.Add(itemTable);
			}
			
			CalculateContentSize();
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
				lineCount = Mathf.CeilToInt(itemLuaScriptList.Count / (float) oneLineItemCount);
				width = lineCount * itemRect.width + lineCount * spacing.x;
				height = 0f;
			}
			else
			{
				oneLineItemCount = Mathf.FloorToInt((contentRect.width + spacing.x) / (itemRect.width + spacing.x));
				lineCount = Mathf.CeilToInt(itemLuaScriptList.Count / (float) oneLineItemCount);
				width = 0f;
				height = lineCount * itemRect.height + lineCount * spacing.y;
			}
			
			content.sizeDelta = new Vector2(width, height);
		}
		
		/// <summary>
		/// 计算所有需要显示的Item的Index
		/// </summary>
		/// <returns></returns>
		private bool CalculateNeedShowItemIndexList()
		{
			var contentAnchoredPosition = content.anchoredPosition;
			var viewPortRect = viewPort.rect;
			var itemRect = itemPrefab.rect;

			int startLine;
			int endLine;
			if (horizontal)
			{
				startLine = Mathf.CeilToInt((-contentAnchoredPosition.x + spacing.x) / (itemRect.width + spacing.x));
				startLine = startLine <= 0 ? 1 : startLine;

				var offset = startLine * (itemPrefab.rect.width + spacing.x) - spacing.x + contentAnchoredPosition.x;
				var alignmentHeight = viewPortRect.width - offset;
				endLine = Mathf.CeilToInt(alignmentHeight / (itemRect.width + spacing.x)) + startLine;
			}
			else
			{
				startLine = Mathf.CeilToInt((contentAnchoredPosition.y + spacing.y) / (itemRect.height + spacing.y));
				startLine = startLine <= 0 ? 1 : startLine;

				var offset = startLine * (itemPrefab.rect.height + spacing.y) - spacing.y - contentAnchoredPosition.y;
				var alignmentHeight = viewPortRect.height - offset;
				endLine = Mathf.CeilToInt(alignmentHeight / (itemRect.height + spacing.y)) + startLine;
			}

			var startIndex = (startLine - 1) * oneLineItemCount;
			var endIndex = endLine * oneLineItemCount > itemLuaScriptList.Count ? itemLuaScriptList.Count - 1 : endLine * oneLineItemCount - 1;
			var lastStartIndex = curShowItemIndexList.Count == 0 ? -1 : curShowItemIndexList[0]; 
			var lastEndIndex = curShowItemIndexList.Count == 0 ? -1 : curShowItemIndexList[curShowItemIndexList.Count - 1];

			if (startIndex == lastStartIndex || endIndex == lastEndIndex)
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
						curShowItemIndexList.Add(i);
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
				var aheadLine = Mathf.FloorToInt(itemIndex / oneLineItemCount);
				var curLineIndex = itemIndex % oneLineItemCount;
				float xPos;
				float yPos;
				if (horizontal)
				{
					xPos = aheadLine * (itemRect.width + spacing.x) + itemRect.width / 2;
					yPos = -(curLineIndex * (itemRect.height + spacing.y) + itemRect.height / 2);
				}
				else
				{
					xPos = curLineIndex * (itemRect.width + spacing.x) + itemRect.width / 2;
					yPos = -(aheadLine * (itemRect.height + spacing.y) + itemRect.height / 2);
				}

				if (TryGetItemObject(itemIndex, out var itemObject))
				{
					itemObject.ItemTable = itemLuaScriptList[itemIndex];
					itemObject.OnInit(itemIndex);
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
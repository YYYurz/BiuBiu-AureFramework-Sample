//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace BiuBiu
{
	/// <summary>
	/// 滚动列表
	/// </summary>
	[RequireComponent(typeof(ScrollRect))]
	public class UIContentList : MonoBehaviour
	{
		private static int PoolNameIndex = 0;
		private IObjectPool<UIContentItem> itemObjectPool;
		private ScrollRect scrollRect;
		private RectTransform content;

		[SerializeField] private bool horizontal;
		[SerializeField] private bool vertical;
		[SerializeField] private Vector2 spacing;
		[SerializeField] private RectTransform itemPrefab;

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
			
			itemObjectPool = GameMain.ObjectPool.CreateObjectPool<UIContentItem>("UIContentList_" + ++PoolNameIndex, int.MaxValue, 300);
		}

		private void Update()
		{
			
		}

		public void InitContentList(int itemCount)
		{
			if (itemPrefab == null)
			{
				Debug.LogError("UIContentList : Item prefab is null.");
				return;
			}		
			
			CalculateContentSize(itemCount);
			CalculateItemPosition(itemCount);
		}
		
		private void CalculateContentSize(int itemCount)
		{
			var contentRect = content.rect;
			var itemRect = itemPrefab.rect;

			float width;
			float height;
			
			if (horizontal)
			{
				var oneLineCount = Mathf.FloorToInt(contentRect.height + spacing.y) / (itemRect.height + spacing.y);
				var lineCount = Mathf.CeilToInt(itemCount / oneLineCount);
				width = lineCount * itemRect.width + lineCount * spacing.x;
				height = 0f;
			}
			else
			{
				var oneLineCount = Mathf.FloorToInt(contentRect.width + spacing.x) / (itemRect.width + spacing.x);
				var lineCount = Mathf.CeilToInt(itemCount / oneLineCount);
				width = 0f;
				height = lineCount * itemRect.height + lineCount * spacing.y;
			}
			
			content.sizeDelta = new Vector2(width, height);
		}
		
		private void CalculateItemPosition(int itemCount)
		{
			
		}

		private void TryGetItem()
		{
			
		}

		private void CreateItem()
		{
			
		}
	}
}
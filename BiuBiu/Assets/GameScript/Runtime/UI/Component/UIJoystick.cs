//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace BiuBiu
{
	/// <summary>
	/// 摇杆
	/// </summary>
	public class UIJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
	{
		private Vector2 originPos;
		private Vector2 parentPos;
		private Vector2 direction;
		private bool isDragging;

		[SerializeField] private RectTransform dragBgRectTransform;
		[SerializeField] private RectTransform dragHandleRectTransform;

		/// <summary>
		/// 获取摇杆方向
		/// </summary>
		public Vector2 Direction
		{
			get
			{
				return direction;
			}
		}

		private void Start()
		{
			isDragging = false;
			originPos = dragBgRectTransform.anchoredPosition;
			parentPos = transform.parent.GetComponent<RectTransform>().anchoredPosition;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (isDragging)
			{
				return;
			}

			isDragging = true;
			OnRefreshHandle(eventData.position);
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (!isDragging)
			{
				return;
			}

			OnRefreshHandle(eventData.position);
		}
		
		public void OnPointerUp(PointerEventData eventData)
		{
			if (!isDragging)
			{
				return;
			}

			isDragging = false;
			ResetHandle();
		}

		private void OnRefreshHandle(Vector2 position)
		{
			var offset = position - parentPos;
			if (offset.magnitude > 40f)
			{
				direction = offset.normalized;
				var handlePos = direction * 100f;
				dragHandleRectTransform.anchoredPosition = handlePos + originPos;
				GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Direction, direction));
			}
			else
			{
				dragHandleRectTransform.anchoredPosition = offset;
			}
		}

		private void ResetHandle()
		{
			dragHandleRectTransform.anchoredPosition = originPos;
			GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.None, direction));
		}
	}
}
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

		private void Update()
		{
			var direction = Vector2.zero;
			if (Input.GetKey(KeyCode.W))
			{
				direction = Vector2.up;
				GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Direction, direction));
			}
			else if (Input.GetKey(KeyCode.A))
			{
				direction = Vector2.left;
				GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Direction, direction));
			}
			else if (Input.GetKey(KeyCode.S))
			{
				direction = Vector2.down;
				GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Direction, direction));
			}
			else if (Input.GetKey(KeyCode.D))
			{
				direction = Vector2.right;
				GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Direction, direction));
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				direction = Vector2.zero;
				GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.None, direction));
			}
			
			
			if (Input.GetKeyDown(KeyCode.J))
			{
				GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Attack1, direction));
			}
			else if (Input.GetKeyDown(KeyCode.K))
			{
				GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Attack2, direction));
			}
			else if (Input.GetKeyDown(KeyCode.L))
			{
				GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Retreat, direction));
			}
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
				GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.None, direction));
			}
		}

		private void ResetHandle()
		{
			dragHandleRectTransform.anchoredPosition = originPos;
			GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.None, direction));
		}
	}
}
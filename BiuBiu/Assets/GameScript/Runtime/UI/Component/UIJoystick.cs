//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
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

#if UNITY_EDITOR
		private void Update()
		{
			// xbox手柄测试用
			var h = Input.GetAxis("Horizontal");
			var v = Input.GetAxis("Vertical");
			h = Math.Abs(h) < 0.1f ? 0f : h;
			v = Math.Abs(v) < 0.1f ? 0f : -v;
			direction = new Vector2(h, v);

			if (v.Equals(0f) && h.Equals(0f))
			{
				// GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.None, direction.normalized));
			}
			else
			{
				// GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Direction, direction.normalized));
			}
			
			if(Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick1Button0))
			{
				// GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Attack, direction.normalized));
			}
			else if(Input.GetKeyDown(KeyCode.Joystick1Button1))
			{
				// GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Retreat, direction.normalized));
			}
			else if(Input.GetKeyDown(KeyCode.Joystick1Button2))
			{
				// GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Skill, direction.normalized));
			}
		}
#endif

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
				// GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.Direction, direction));
			}
			else
			{
				dragHandleRectTransform.anchoredPosition = offset;
				// GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.None, direction));
			}
		}

		private void ResetHandle()
		{
			dragHandleRectTransform.anchoredPosition = originPos;
			// GameMain.Event.Fire(this, InputEventArgs.Create(ECSConstant.InputType.None, direction));
		}
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BiuBiu
{
	[DisallowMultipleComponent]
	public class UIHit : MonoBehaviour, IPointerClickHandler
	{
		private readonly UnityEvent clickEvent = new UnityEvent();

		public UnityEvent OnClick
		{
			get
			{
				return clickEvent;
			}
		}
		
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			clickEvent.Invoke();
		}
	}
}
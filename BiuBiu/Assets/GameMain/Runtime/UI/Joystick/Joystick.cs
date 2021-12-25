using BB;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 摇杆
/// </summary>
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	[SerializeField] private RectTransform dragBgRectTransform;
	[SerializeField] private RectTransform dragHandleRectTransform;
	[SerializeField] private float dragAreaRadius;

	private Vector2 originPos;
	private Vector2 parentPos;
	
	private bool isDragging;

	private void Start() {
		isDragging = false;
		originPos = dragBgRectTransform.anchoredPosition;
		parentPos = transform.parent.GetComponent<RectTransform>().anchoredPosition;
	}

	public void OnPointerDown(PointerEventData eventData) {
		IsPressPosInArea(eventData.position);
		if (isDragging) {
			return;
		}

		isDragging = true;
		OnRefreshHandle(eventData.position);
	}

	public void OnPointerUp(PointerEventData eventData) {
		if (!isDragging) {
			return;
		}

		isDragging = false;
		ResetHandle();
	}

	public void OnDrag(PointerEventData eventData) {
		if (!isDragging) {
			return;
		}
		
		OnRefreshHandle(eventData.position);
	}

	private void IsPressPosInArea(Vector2 position) {
		var offset = position - parentPos;
		var distance = Vector2.Distance(offset, originPos);
		Debug.Log(distance);
	}

	private void OnRefreshHandle(Vector2 position) {
		var direction = (position - parentPos).normalized;
		var offset = direction * 100f;
		
		dragHandleRectTransform.anchoredPosition = offset + originPos;
		InputComponent.OnRefreshMoveDirectionVector(direction);
	}

	private void ResetHandle() {
		dragHandleRectTransform.anchoredPosition = originPos;
		InputComponent.OnRefreshMoveDirectionVector(Vector2.zero);
	}
}
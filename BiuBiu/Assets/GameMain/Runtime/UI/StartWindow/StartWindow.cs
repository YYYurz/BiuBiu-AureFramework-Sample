using UnityEngine;
using UnityEngine.UI;

public class StartWindow : MonoBehaviour
{
	[SerializeField] private Slider sliProgress;

	public static GameObject CreateStartWindow(Transform parent)
	{
		var obj = Resources.Load<GameObject>("StartWindow/StartWindow");
		return obj == null ? null : Instantiate(obj, parent);
	}

	public void SetSliderProgress(float progress) => sliProgress.value = progress;

	public void DestroySelf() => Destroy(gameObject);
}
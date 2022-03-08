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
	/// 监听UI事件播放声音
	/// </summary>
	public sealed class UISound : MonoBehaviour, IPointerClickHandler
	{
		/// <summary>
		/// 声音配置Id
		/// </summary>
		private enum UISoundId
		{
			ButtonClick = 1006,
		}

		[SerializeField] private UISoundId uiSoundId = UISoundId.ButtonClick;
		
		public void OnPointerClick(PointerEventData eventData)
		{
			GameMain.Sound.PlaySound((uint) uiSoundId, 0f);
		}
	}
}
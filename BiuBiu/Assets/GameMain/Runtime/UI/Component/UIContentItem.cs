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
using UnityEngine.EventSystems;

namespace BiuBiu
{
	/// <summary>
	/// 滚动列表Item
	/// </summary>
	public class UIContentItem : ObjectBase
	{
		
		
		public static UIContentItem Create(GameObject gameObj)
		{
			var uiContentItem = GameMain.ReferencePool.Acquire<UIContentItem>();

			return uiContentItem;
		}
	}
}
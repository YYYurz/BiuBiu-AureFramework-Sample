//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace BiuBiu
{
	public sealed partial class PlayerController : MonoBehaviour
	{
		/// <summary>
		/// 死亡状态
		/// </summary>
		private sealed class StateDead : PlayerControllerStateBase
		{
			private bool canChange;
			
			public override bool CanChange
			{
				get
				{
					return canChange;
				}
			}
		}
	}
}
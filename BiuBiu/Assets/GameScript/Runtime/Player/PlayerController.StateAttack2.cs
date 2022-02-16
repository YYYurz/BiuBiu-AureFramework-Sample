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
		/// 攻击2状态
		/// </summary>
		private sealed class StateAttack2 : PlayerControllerStateBase
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
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Fsm;
using UnityEditor;

namespace BiuBiu
{
	public abstract class PlayerControllerStateBase : FsmState
	{
		public abstract bool CanChange
		{
			get;
		}

		/// <summary>
		/// 暂停
		/// </summary>
		public abstract void Pause();
		
		/// <summary>
		/// 恢复
		/// </summary>
		public abstract void Resume();
	}
}
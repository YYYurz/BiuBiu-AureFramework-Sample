//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Fsm;

namespace BiuBiu
{
	public abstract class PlayerControllerStateBase : FsmState
	{
		public abstract bool CanChange
		{
			get;
		}
	}
}
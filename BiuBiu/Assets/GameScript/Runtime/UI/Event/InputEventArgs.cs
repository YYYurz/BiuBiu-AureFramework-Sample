//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using Unity.Mathematics;

namespace BiuBiu
{
	/// <summary>
	/// 输入事件
	/// </summary>
	public class InputEventArgs : AureEventArgs
	{
		/// <summary>
		/// 输入类型
		/// </summary>
		// public ECSConstant.InputType InputType
		// {
		// 	get;
		// 	private set;
		// }

		/// <summary>
		/// 方向
		/// </summary>
		public float2 Direction
		{
			get;
			private set;
		} 

		// public static InputEventArgs Create(ECSConstant.InputType inputType, float2 direction)
		// {
		// 	var inputEventArgs = GameMain.ReferencePool.Acquire<InputEventArgs>();
		// 	inputEventArgs.InputType = inputType;
		// 	inputEventArgs.Direction = direction;
		//
		// 	return inputEventArgs;
		// }
		
		public override void Clear()
		{
			// InputType = ECSConstant.InputType.None;
			Direction = float2.zero;
		}
	}
}
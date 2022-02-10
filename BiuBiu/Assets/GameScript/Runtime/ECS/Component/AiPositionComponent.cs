//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Burst;
using Unity.Entities;

namespace AureFramework
{
	/// <summary>
	/// 可寻路数据
	/// </summary>
	// [BurstCompile]
	public struct AiPositionComponent : IComponentData
	{
		/// <summary>
		/// 控制状态计数
		/// </summary>
		public int ControlBuffCounter;
	}
}
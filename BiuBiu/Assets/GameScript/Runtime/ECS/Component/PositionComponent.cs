//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Entities;
using Unity.Mathematics;

namespace BiuBiu
{
	/// <summary>
	/// 位置数据
	/// </summary>
	// [BurstCompile]
	public struct PositionComponent : IComponentData
	{
		/// <summary>
		/// 当前位置
		/// </summary>
		public float3 CurPosition;
		
		/// <summary>
		/// 下一个位置
		/// </summary>
		public float3 NextPosition;
	}
}
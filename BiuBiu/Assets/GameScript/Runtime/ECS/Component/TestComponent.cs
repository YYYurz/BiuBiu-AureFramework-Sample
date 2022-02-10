//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Burst;
using Unity.Entities;

namespace BiuBiu
{
	[BurstCompile]
	public struct TestComponent : IComponentData
	{
		public float level;
	}
}
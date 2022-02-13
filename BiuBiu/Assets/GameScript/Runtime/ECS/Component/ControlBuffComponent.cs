//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Entities;

namespace BiuBiu
{
	public struct ControlBuffComponent : IComponentData
	{
		/// <summary>
		/// 击飞剩余时间
		/// </summary>
		public float FlyTime;
		
		/// <summary>
		/// 眩晕剩余时间
		/// </summary>
		public float VertigoTime;
		
		/// <summary>
		/// 击退剩余时间
		/// </summary>
		public float BackTime;
	}
}
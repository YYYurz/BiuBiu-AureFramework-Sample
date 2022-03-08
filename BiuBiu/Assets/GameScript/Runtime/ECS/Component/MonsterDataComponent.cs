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
	/// <summary>
	/// 小怪数据（其实是桶子）
	/// </summary>
	[BurstCompile]
	[GenerateAuthoringComponent]
	public struct MonsterDataComponent : IComponentData
	{
		/// <summary>
		/// 生命值
		/// </summary>
		public float Health;
		
		/// <summary>
		/// 攻击力（并没有什么卵用）
		/// </summary>
		public float AttackDamage;
	}
}
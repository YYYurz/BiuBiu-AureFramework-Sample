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
	/// <summary>
	/// 寻路单位数据
	/// </summary>
	[GenerateAuthoringComponent]
	public struct PathFollowComponent : IComponentData
	{
		/// <summary>
		/// 标记走到寻路路径（坐标点数组）的哪一个索引
		/// </summary>
		public int PathIndex;
		
		/// <summary>
		/// 移动速度
		/// </summary>
		public float MoveSpeed;
	}
}
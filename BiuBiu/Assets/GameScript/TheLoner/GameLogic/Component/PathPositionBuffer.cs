//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Burst;
using Unity.Entities;

namespace TheLoner
{
	/// <summary>
	/// 存储寻路路径结果
	/// </summary>
	[BurstCompile]
	[GenerateAuthoringComponent]
	public struct PathPositionBuffer : IBufferElementData
	{
		/// <summary>
		/// 地图网格索引
		/// </summary>
		public int MapPointIndex;
	}
}
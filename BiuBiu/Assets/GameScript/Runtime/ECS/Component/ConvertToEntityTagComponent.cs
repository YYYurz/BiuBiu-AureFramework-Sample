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
	/// Addressable加载GameObject时的标记，在转换为Entity后将被移除
	/// </summary>
	[GenerateAuthoringComponent]
	public struct ConvertToEntityTagComponent : IComponentData
	{
		/// <summary>
		/// 对应Entity配置表中的Id
		/// </summary>
		public uint EntityId;
	}
}
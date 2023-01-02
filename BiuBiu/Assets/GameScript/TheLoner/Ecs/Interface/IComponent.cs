//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public interface IComponent : IReference
	{
		int EntityId
		{
			get;
			set;
		}

		bool Enabled
		{
			get;
			set;
		}

		bool MarkRemove
		{
			get;
			set;
		}
	}
}
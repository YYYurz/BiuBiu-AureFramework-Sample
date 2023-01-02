//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public class AbstractComponent : IComponent
	{
		public int EntityId
		{
			set;
			get;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public bool MarkRemove
		{
			get;
			set;
		}
		
		public virtual void Clear()
		{
			
		}
	}
}
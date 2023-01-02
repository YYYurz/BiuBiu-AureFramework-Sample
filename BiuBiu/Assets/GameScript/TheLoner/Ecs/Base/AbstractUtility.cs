//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public abstract class AbstractUtility
	{
		private readonly IWorld world;
		
		public IWorld World
		{
			get
			{
				return world;
			}
		}

		public virtual void OnAwake()
		{
			
		}
	}
}
﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public abstract class AbstractSystem
	{
		private readonly IWorld world;

		protected AbstractSystem(IWorld world)
		{
			this.world = world;
		}
		
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

		public virtual void OnUpdate()
		{
			
		}

		public virtual void OnDestroy()
		{
			
		}
	}
}
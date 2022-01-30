//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Entities;
using Unity.Jobs;

namespace BiuBiu
{
	public class PathfindingSystem : ComponentSystemBase
	{
		
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
		}

		public override void Update()
		{
			
		}
		
		private struct PathfindingJob : IJob
		{
			public void Execute()
			{
				
			}
		}
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework;

namespace BiuBiu
{
	public sealed class GamePlayModule : AureFrameworkModule, IGamePlayModule
	{
		public MapConfig CurMapConfig
		{
			get;
		}

		public bool IsStart
		{
			get;
		}

		public bool IsPause
		{
			get;
		}

		public override void Init()
		{
			
		}

		public override void Tick(float elapseTime, float realElapseTime)
		{
			
		}

		public override void Clear()
		{
			
		}
		

		public void CreateGame(int gameId)
		{
			
		}
	}
}
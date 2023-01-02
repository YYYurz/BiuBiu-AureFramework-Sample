//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework;
using TheLoner;

namespace BiuBiu
{
	/// <summary>
	/// 游戏玩法模块 
	/// </summary>
	public sealed class LogicWorldModule : AureFrameworkModule, ILogicWorldModule
	{
		private readonly Ecs ecs = new Ecs();
		
		public override void Init()
		{
			
		}

		public override void Tick(float elapseTime, float realElapseTime)
		{
			
		}

		public override void Clear()
		{
			ecs.Clear();
		}
		
		/// <summary>
		/// 创建游戏
		/// </summary>
		/// <param name="gameId"> 游戏Id </param>
		/// <param name="battleInitData"> 战斗初始化数据 </param>
		public IWorld CreateWorld(uint gameId, BattleInitData battleInitData)
		{
			return ecs.CreateWorld(battleInitData);
		}

		/// <summary>
		/// 销毁逻辑世界
		/// </summary>
		/// <param name="world"></param>
		public void DestroyWorld(IWorld world)
		{
			ecs.DestroyWorld(world);
		}
	}
}
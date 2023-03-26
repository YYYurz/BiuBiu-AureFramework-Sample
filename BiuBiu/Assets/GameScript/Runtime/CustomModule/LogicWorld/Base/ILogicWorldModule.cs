//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using TheLoner;

namespace DrunkFish
{
	/// <summary>
	/// 游戏模块接口
	/// </summary>
	public interface ILogicWorldModule
	{
		/// <summary>
		/// 创建逻辑世界
		/// </summary>
		/// <param name="gameId"> 游戏Id </param>
		/// <param name="battleInitData"> 战斗初始化数据 </param>
		// IWorld CreateWorld(uint gameId, BattleInitData battleInitData);
		
		/// <summary>
		/// 销毁逻辑世界
		/// </summary>
		/// <param name="world"></param>
		// void DestroyWorld(IWorld world);
	}
}
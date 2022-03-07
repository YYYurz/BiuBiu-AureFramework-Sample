//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;

namespace BiuBiu
{
	/// <summary>
	/// 主角击中怪物事件
	/// </summary>
	public class HitMonsterEventArgs : AureEventArgs
	{
		/// <summary>
		/// 这次攻击是否凶猛
		/// </summary>
		public bool IsStrong
		{
			get;
			private set;
		}

		public static HitMonsterEventArgs Create(bool isStrong)
		{
			var hitMonsterEventArgs = GameMain.ReferencePool.Acquire<HitMonsterEventArgs>();
			hitMonsterEventArgs.IsStrong = isStrong;

			return hitMonsterEventArgs;
		}
		
		public override void Clear()
		{
			IsStrong = false;
		}
	}
}
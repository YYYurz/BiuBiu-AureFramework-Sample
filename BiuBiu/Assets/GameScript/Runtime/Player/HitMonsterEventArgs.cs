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
		/// 击中的怪物数量
		/// </summary>
		public int HitMonsterCount
		{
			get;
			private set;
		}

		public static HitMonsterEventArgs Create(int hitMonsterCount)
		{
			var hitMonsterEventArgs = GameMain.ReferencePool.Acquire<HitMonsterEventArgs>();
			hitMonsterEventArgs.HitMonsterCount = hitMonsterCount;

			return hitMonsterEventArgs;
		}
		
		public override void Clear()
		{
			HitMonsterCount = 0;
		}
	}
}
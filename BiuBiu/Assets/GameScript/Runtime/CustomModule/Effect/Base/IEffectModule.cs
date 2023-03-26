//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace DrunkFish
{
	/// <summary>
	/// 特效模块接口
	/// </summary>
	public interface IEffectModule
	{
		/// <summary>
		/// 获取或设置特效对象池容量
		/// </summary>
		int EffectObjectPoolCapacity
		{
			get;
			set;
		}

		/// <summary>
		/// 获取或设置特效对象池过期时间
		/// </summary>
		float EffectObjectPoolExpireTime
		{
			get;
			set;
		}

		/// <summary>
		/// 清理所有特效
		/// </summary>
		void ClearAllEffect();

		/// <summary>
		/// 播放特效
		/// </summary>
		/// <param name="effectAsset"> 特效资源名称 </param>
		/// <param name="pos"> 特效播放位置 </param>
		/// <param name="rot"> 特效播放角度 </param>
		/// <param name="parentTrans"> 特效父物体 </param>
		void PlayEffect(string effectAsset, Vector3 pos, Quaternion rot, Transform parentTrans = null);

		/// <summary>
		/// 暂停所有特效
		/// </summary>
		void PauseAllEffect();

		/// <summary>
		/// 恢复所有特效
		/// </summary>
		void ResumeAllEffect();
	}
}
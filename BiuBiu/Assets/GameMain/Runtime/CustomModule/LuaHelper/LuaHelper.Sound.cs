//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Sound;
using UnityEngine;

namespace BiuBiu
{
	public static partial class LuaHelper
	{
		public static int PlaySound(string soundAssetName, string soundGroupName, SoundParams soundParams = null)
		{
			if (soundParams == null)
			{
				soundParams = SoundParams.Create();
			}

			return GameMain.Sound.PlaySound(soundAssetName, soundGroupName, soundParams);
		}

		public static void PauseSound(int soundId, float fadeOutSeconds = 0f)
		{
			GameMain.Sound.PauseSound(soundId, fadeOutSeconds);
		}
		
		public static void ResumeSound(int soundId, float fadeInSeconds = 0f)
		{
			GameMain.Sound.ResumeSound(soundId, fadeInSeconds);
		}
	}
}
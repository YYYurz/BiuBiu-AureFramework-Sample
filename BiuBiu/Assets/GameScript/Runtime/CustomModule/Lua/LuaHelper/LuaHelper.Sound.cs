//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace BiuBiu
{
	public static partial class LuaHelper
	{
		public static int PlaySound(uint soundId, float fadeInSeconds = 0f, GameObject bindingObject = null)
		{
			return GameMain.Sound.PlaySound(soundId, fadeInSeconds, bindingObject);
		}

		public static void StopSound(int soundId, float fadeOutSeconds = 0f)
		{
			GameMain.Sound.StopSound(soundId, fadeOutSeconds);
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
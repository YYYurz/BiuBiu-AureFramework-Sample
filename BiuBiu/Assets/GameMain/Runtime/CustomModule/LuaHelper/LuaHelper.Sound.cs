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
		public static void PlaySound(string soundAssetName, string soundGroupName, SoundParams soundParams = null)
		{
			if (soundParams == null)
			{
				soundParams = SoundParams.Create();
			}

			GameMain.Sound.PlaySound(soundAssetName, soundGroupName, soundParams);
		}
	}
}
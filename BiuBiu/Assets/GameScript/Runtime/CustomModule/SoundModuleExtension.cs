//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Sound;
using UnityEngine;

namespace DrunkFish
{
	/// <summary>
	/// 声音模块扩展类
	/// </summary>
	public static class SoundModuleExtension
	{
		public static int PlaySound(this ISoundModule soundModule, uint soundId, float fadeInSeconds, GameObject bindingObject = null)
		{
			var soundInfo = GameMain.DataTable.GetDataTableReader<SoundTableReader>().GetInfo((uint)soundId);
			var soundParam = SoundParams.Create();
			soundParam.Volume = soundInfo.Volume;
			soundParam.Loop = soundInfo.Loop == 1;
			soundParam.FadeInSeconds = fadeInSeconds;
			
			return soundModule.PlaySound(soundInfo.AssetPath, soundInfo.GroupName, bindingObject, soundParam);
		}
	}
}
//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace BiuBiu
{
	public static partial class LuaHelper
	{
		private static readonly List<ISoundGroup> SoundGroupCacheList = new List<ISoundGroup>();

		/// <summary>
		/// 获取或设置静音
		/// </summary>
		public static bool Mute
		{
			get
			{
				GetAllSoundGroup();
				foreach (var soundGroup in SoundGroupCacheList)
				{
					if (!soundGroup.Mute)
					{
						return false;
					}	
				}
				
				return true;
			}
			set
			{
				GetAllSoundGroup();
				foreach (var soundGroup in SoundGroupCacheList)
				{
					soundGroup.Mute = value;
				}
			}
		}

		/// <summary>
		/// 获取或设置音量
		/// </summary>
		public static float Volume
		{
			get
			{
				GetAllSoundGroup();
				return SoundGroupCacheList[0].Volume;
			}
			set
			{
				GetAllSoundGroup();
				foreach (var soundGroup in SoundGroupCacheList)
				{
					soundGroup.Volume = value;
				}
			}
		}

		public static int PlaySound(uint soundId, float fadeInSeconds = 0f, GameObject bindingObject = null)
		{
			var group = GameMain.Sound.GetSoundGroup("");
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

		private static void GetAllSoundGroup()
		{
			if (SoundGroupCacheList.Count > 0)
			{
				return;
			}

			SoundGroupCacheList.Add(GameMain.Sound.GetSoundGroup("Sound"));
			SoundGroupCacheList.Add(GameMain.Sound.GetSoundGroup("Bgm"));
		}
	}
}
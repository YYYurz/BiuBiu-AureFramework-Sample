//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using UnityEngine;

namespace BiuBiu
{
	[Serializable]
	public class Test
	{
		private readonly string testStr;
		private float volume;

		public Test(string testStr)
		{
			this.testStr = testStr;
		}
		
		[SerializeField]
		public float Volume
		{
			get
			{
				return volume;
			}
			set
			{
				volume = value;
				if (!string.IsNullOrEmpty(testStr))
				{
					Debug.Log(testStr + volume);
				}
			}
		}
	}
}
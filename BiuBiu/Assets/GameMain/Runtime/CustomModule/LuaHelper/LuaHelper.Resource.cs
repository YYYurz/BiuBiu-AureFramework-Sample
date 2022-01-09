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
		/// <summary>
		/// 加载二进制资源
		/// </summary>
		/// <param name="filePath"> 资源路径 </param>
		/// <returns></returns>
		public static byte[] GetBytesFile(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				Debug.LogError("LuaCallStatic : File path is null.");
				return null;
			}

			return AssetUtils.LoadBytes(filePath);
		}
	}
}
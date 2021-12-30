//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.IO;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 资源工具
	/// </summary>
	public static class AssetUtils
	{
		/// <summary>
		/// 同步加载二进制文本资源
		/// </summary>
		/// <param name="filePath"> 资源路径 </param>
		/// <returns></returns>
		public static byte[] LoadBytes(string filePath)
		{
			byte[] result;
#if UNITY_EDITOR
			var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath).Replace("/", "\\");
			if (!File.Exists(fullPath))
			{
				return null;
			}
			Debug.Log(fullPath);
			result = File.ReadAllBytes(fullPath);
#else
			var textAsset = GameMain.Resource.LoadAssetSync<TextAsset>(filePath);
			result = textAsset.bytes;
#endif
			return result;
		}
	}
}
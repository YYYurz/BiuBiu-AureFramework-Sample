//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;
using ILogger = TheLoner.ILogger;

namespace BiuBiu
{
	public class Logger : ILogger
	{
		public void Log(string content)
		{
			Debug.Log(content);
		}

		public void LogWarning(string content)
		{
			Debug.LogWarning(content);
		}

		public void LogError(string content)
		{
			Debug.LogError(content);
		}
	}
}
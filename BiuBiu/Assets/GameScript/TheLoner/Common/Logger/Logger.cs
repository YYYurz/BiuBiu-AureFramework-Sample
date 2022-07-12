//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public static class Logger
	{
		private static ILogger logger;

		public static void Log(string content)
		{
			logger?.Log(content);
		}

		public static void LogWarning(string content)
		{
			logger?.LogWarning(content);
		}
		
		public static void LogError(string content)
		{
			logger?.LogError(content);
		}
	}
}
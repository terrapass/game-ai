using System;
using System.Diagnostics;
using System.ComponentModel;

namespace Terrapass.GameAi.Goap.Logging
{
	public interface ILogger
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		void ForceLog(LogLevel level, string message, params object[] messageArgs);
	}

	public static class LoggerExtensions
	{
		public static void LogFatal(this ILogger logger, string message, params object[] messageArgs)
		{
			logger.ForceLog(LogLevel.FATAL, message, messageArgs);
		}

		public static void LogError(this ILogger logger, string message, params object[] messageArgs)
		{
			logger.ForceLog(LogLevel.ERROR, message, messageArgs);
		}

		public static void LogWarning(this ILogger logger, string message, params object[] messageArgs)
		{
			logger.ForceLog(LogLevel.WARNING, message, messageArgs);
		}

		public static void LogInfo(this ILogger logger, string message, params object[] messageArgs)
		{
			logger.ForceLog(LogLevel.INFO, message, messageArgs);
		}

		[Conditional("DEBUG")]
		public static void LogDebug(this ILogger logger, string message, params object[] messageArgs)
		{
			logger.ForceLog(LogLevel.DEBUG, message, messageArgs);
		}

		[Conditional("DEBUG")]
		public static void LogTrace(this ILogger logger, string message, params object[] messageArgs)
		{
			logger.ForceLog(LogLevel.TRACE, message, messageArgs);
		}
	}
}


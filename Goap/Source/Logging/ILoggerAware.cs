using System;
using System.ComponentModel;

namespace Terrapass.GameAi.Goap.Logging
{
	public interface ILoggerAware
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		ILogger Logger { get; set; }
	}

	public static class LoggerAwareExtensions
	{
		public static void ResetLogger(this ILoggerAware loggerAware, ILogger logger = null)
		{
			loggerAware.Logger = logger != null ? logger : new NullLogger();
		}
	}
}


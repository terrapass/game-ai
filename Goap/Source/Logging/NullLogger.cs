using System;

namespace Terrapass.GameAi.Goap.Logging
{
	public sealed class NullLogger : ILogger
	{
		public void ForceLog(LogLevel level, string message, params object[] messageArgs)
		{
			
		}
	}
}


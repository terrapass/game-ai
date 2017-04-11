using System;

namespace Terrapass.GameAi.Goap.Debug
{
	public static class PreconditionUtils
	{
		private const string DEFAULT_ENSURE_NOT_NULL_FORMAT 	= "{0} must not be null";
		private const string DEFAULT_ENSURE_NOT_BLANK_FORMAT 	= "{0} must not be null or empty";

		public static T EnsureNotNull<T>(
			T value, 
			string valueName, 
			string format = DEFAULT_ENSURE_NOT_NULL_FORMAT, 
			params object[] args
		)
		{
			if(value == null)
			{
				throw new ArgumentNullException(valueName, string.Format(format, valueName, args));
			}
			return value;
		}

		public static string EnsureNotBlank(
			string value, 
			string valueName, 
			string format = DEFAULT_ENSURE_NOT_BLANK_FORMAT,
			params object[] args
		)
		{
			if(value == null)
			{
				throw new ArgumentNullException(valueName, string.Format(format, valueName, args));
			}
			if(value.Length == 0)
			{
				throw new ArgumentException(valueName, string.Format(format, valueName, args));
			}
			return value;
		}
	}
}


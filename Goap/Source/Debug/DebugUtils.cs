using System;
using System.Diagnostics;

namespace Terrapass.GameAi.Goap.Debug
{
	public static class DebugUtils
	{
		private const string ASSERTION_MESSAGE = "DEBUG ASSERTION FAILED: {0}";

		/// <summary>
		/// Check the specified condition. 
		/// This method is a thin wrapper around System.Diagnostics.Debug.Assert().
		/// </summary>
		/// <remarks>
		/// Calls to this method will be omitted from Release builds.
		/// </remarks>
		/// <param name="condition">Condition to be checked. For assertion to pass it must be true.</param>
		/// <param name="format">Format of the message to be displayed if condition is false.</param>
		/// <param name="args">Message format parameters.</param>
		[Conditional("DEBUG")]
		public static void Assert(bool condition, string format, params object[] args)
		{
			System.Diagnostics.Debug.Assert(
				condition, 
				string.Format(
					ASSERTION_MESSAGE,
					String.Format(
						format,
						args
					)
				)
			);
		}
	}
}


using System;

namespace Terrapass.GameAi.Goap.Agents
{
	public enum ExecutionStatus
	{
		None = 0,
		InProgress = 1,
		Complete = 2,
		Failed = 3,
		InInterruption = 4,
		Interrupted = 5
	}

	public static class ExecutionStatusExtensions
	{
		public static bool IsCurrentActionAvailable(this ExecutionStatus status)
		{
			return status != ExecutionStatus.None && status != ExecutionStatus.Complete;
		}

		public static bool IsFinal(this ExecutionStatus status)
		{
			return status != ExecutionStatus.InProgress && status != ExecutionStatus.InInterruption;
		}
	}
}


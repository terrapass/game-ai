using System;

namespace Terrapass.GameAi.Goap.Time
{
	public class ResettableStopwatchExecutionTimer : StopwatchExecutionTimer, IResettableTimer
	{
		public ResettableStopwatchExecutionTimer() : base() {}
		public ResettableStopwatchExecutionTimer(bool startPaused) : base(startPaused) {}

		#region IResettableTimer implementation

		public void Reset (bool startPaused = true)
		{
			this.AccumulatedTime = 0;
			this.StartTime = CurrentTime;
			this.IsPaused = startPaused;
		}

		#endregion
	}
}


using System;
using System.Diagnostics;

namespace Terrapass.GameAi.Goap.Time
{
	/**
	 * This class relies on System.Diagnostics.Stopwatch and allows to measure execution times.
	 */
	public class StopwatchExecutionTimer : ITimer
	{
		private long startTime;
		private long accumulatedTime;
		private bool isPaused;

		public StopwatchExecutionTimer(bool startPaused = false)
		{
			this.StartTime = CurrentTime;
			this.AccumulatedTime = 0;
			this.IsPaused = startPaused;
		}

		public bool Resume() 
		{
			if(this.IsPaused)
			{
				this.StartTime = CurrentTime;
				this.IsPaused = false;
				return true;
			} 
			else 
			{
				return false;
			}
		}

		public bool Pause()
		{
			if(!this.IsPaused) 
			{
				this.AccumulatedTime += CurrentTime - this.startTime;
				this.IsPaused = true;
				return true;
			} 
			else 
			{
				return false;
			}
		}

		public float ElapsedSeconds
		{
			get {
				return (float) ((
					this.IsPaused 
						? this.AccumulatedTime 
						: this.AccumulatedTime + (CurrentTime - this.StartTime)
				) / Stopwatch.Frequency);
			}
		}

		public bool IsPaused
		{
			get {
				return this.isPaused;
			}
			protected set {
				this.isPaused = value;
			}
		}

		protected long StartTime
		{
			get {
				return this.startTime;
			}
			set {
				this.startTime = value;
			}
		}

		protected long AccumulatedTime
		{
			get {
				return this.accumulatedTime;
			}
			set {
				this.accumulatedTime = value;
			}
		}

		protected static long CurrentTime
		{
			get {
				return Stopwatch.GetTimestamp();
			}
		}
	}
}


using UnityEngine;
using System;

namespace Terrapass.Time
{
	/**
	 * This class relies on UnityEngine.Time and allows to measure execution times.
	 */
	public class ExecutionTimer : ITimer
	{
		private float startTime;
		private float accumulatedTime;
		private bool isPaused;
		
		public ExecutionTimer()
			: this(false)
		{

		}
		
		public ExecutionTimer(bool startPaused)
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
				return this.IsPaused 
					? this.AccumulatedTime 
					: this.AccumulatedTime + (CurrentTime - this.StartTime);
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

		protected float StartTime
		{
			get {
				return this.startTime;
			}
			set {
				this.startTime = value;
			}
		}

		protected float AccumulatedTime
		{
			get {
				return this.accumulatedTime;
			}
			set {
				this.accumulatedTime = value;
			}
		}

		protected static float CurrentTime
		{
			get {
				return UnityEngine.Time.unscaledTime;
			}
		}
	}
}


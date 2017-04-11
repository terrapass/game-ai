using System;

namespace Terrapass.GameAi.Goap.Time
{
	public interface ITimer
	{
		float ElapsedSeconds {get;}
		bool IsPaused {get;}

		bool Resume();
		bool Pause();
	}
}


using System;

namespace Terrapass.GameAi.Goap.Time
{
	public interface IResettableTimer : ITimer
	{
		void Reset(bool startPaused = true);
	}	
}


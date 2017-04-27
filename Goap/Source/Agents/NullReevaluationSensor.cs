using System;

namespace Terrapass.GameAi.Goap.Agents
{
	public sealed class NullReevaluationSensor : IReevaluationSensor
	{
		public NullReevaluationSensor()
		{
			
		}

		#region IReevaluationSensor implementation

		public bool IsReevaluationNeeded
		{
			get {
				return false;
			}
		}

		#endregion
	}
}


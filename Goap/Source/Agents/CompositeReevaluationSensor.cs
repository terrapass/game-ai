using System;
using System.Collections.Generic;
using System.Linq;

using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Agents
{
	public class CompositeReevaluationSensor : IReevaluationSensor
	{
		private readonly IList<IReevaluationSensor> sensors;

		public CompositeReevaluationSensor(IEnumerable<IReevaluationSensor> sensors = null)
		{
			this.sensors = sensors != null
				? new List<IReevaluationSensor>(sensors)
				: new List<IReevaluationSensor>();
		}

		public void AddSensor(IReevaluationSensor sensor)
		{
			this.sensors.Add(PreconditionUtils.EnsureNotNull(sensor, nameof(sensor)));
		}

		public void RemoveSensor(IReevaluationSensor sensor)
		{
			this.sensors.Remove(PreconditionUtils.EnsureNotNull(sensor, nameof(sensor)));
		}

		#region IReevaluationSensor implementation
		public bool IsReevaluationNeeded
		{
			get {
				return this.sensors.Any((sensor) => sensor.IsReevaluationNeeded);
			}
		}
		#endregion
	}
}


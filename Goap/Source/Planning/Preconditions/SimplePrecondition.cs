using System;

using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Planning.Preconditions
{
	public sealed class SimplePrecondition : IPrecondition
	{
		private readonly SymbolId symbolId;
		private readonly Predicate<int> predicate;
		private readonly Func<int, double> distanceMetric;
		
		public SimplePrecondition(SymbolId symbolId, Predicate<int> predicate, Func<int, double> distanceMetric = null)
		{
			this.symbolId = symbolId;
			this.predicate = PreconditionUtils.EnsureNotNull(predicate, nameof(predicate));
			this.distanceMetric = distanceMetric;
		}

		#region IPrecondition implementation

		public bool IsSatisfiedBy(WorldState worldState)
		{
			return this.predicate(worldState[this.symbolId]);
		}

		public double GetDistanceFrom(WorldState worldState)
		{
			return this.distanceMetric != null
				? this.distanceMetric(worldState[this.symbolId])
				: this.IsSatisfiedBy(worldState)
					? 0.0
					: 1.0;
		}

		public SymbolId SymbolId
		{
			get {
				return this.symbolId;
			}
		}

		public ValueRange AsValueRange
		{
			get {
				throw new NotImplementedException();
			}
		}

		public bool IsSatisfiable
		{
			get {
				return true;
			}
		}

		#endregion
	}
}


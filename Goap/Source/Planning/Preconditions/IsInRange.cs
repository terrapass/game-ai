using System;

namespace Terrapass.GameAi.Goap.Planning.Preconditions
{
	public class IsInRange : IPrecondition
	{
		private readonly SymbolId symbolId;
		private readonly ValueRange range;

		public IsInRange(SymbolId symbolId, ValueRange range)
		{
			this.symbolId = symbolId;
			this.range = range;
		}

		#region IPrecondition implementation

		public bool IsSatisfiedBy(WorldState worldState)
		{
			return this.range.Contains(worldState[this.symbolId]);
		}

		public double GetDistanceFrom(WorldState worldState)
		{
			var value = worldState[this.symbolId];
			return this.range.AbsDistanceFrom(value);
		}

		public ValueRange AsValueRange
		{
			get {
				return this.range;
			}
		}

		public SymbolId SymbolId
		{
			get {
				return this.symbolId;
			}
		}

		public bool IsSatisfiable
		{
			get {
				return !this.range.IsEmpty;
			}
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{0} in {1}", this.SymbolId, this.range);
		}
	}
}


using System;

namespace Terrapass.GameAi.Goap.Planning.Preconditions
{
	public sealed class IsEqual : IsInRange
	{
//		private readonly int targetValue;

		public IsEqual(SymbolId id, int targetValue)
			//: base(id, (value) => value == targetValue, (value) => Math.Abs(targetValue - value))
			: base(id, ValueRange.Exactly(targetValue))
		{
//			this.targetValue = targetValue;
		}

//		public override string ToString()
//		{
//			return string.Format("({0} == {1})", SymbolId, targetValue);
//		}
	}
}


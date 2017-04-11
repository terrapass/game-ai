using System;

namespace Terrapass.GameAi.Goap.Planning.Preconditions
{
	public sealed class IsNotGreater : IsInRange
	{
		//private readonly int targetValue;

		public IsNotGreater(SymbolId symbolId, int targetValue)
			//: base(symbolId, (value) => value <= targetValue, (value) => Math.Max(0, value - targetValue))
			: base(symbolId, ValueRange.LessThanOrEqual(targetValue))
		{
//			this.targetValue = targetValue;
		}

//		public override string ToString()
//		{
//			return string.Format("({0} <= {1})", SymbolId, targetValue);
//		}
	}
}


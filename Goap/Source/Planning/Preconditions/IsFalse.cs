 using System;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Planning.Preconditions
{
	public sealed class IsFalse : IsInRange
	{
		public IsFalse(SymbolId symbolId)
			//: base(symbolId, (value) => value == 0, (value) => (value != 0) ? 1.0 : 0.0)
			: base(symbolId, ValueRange.Exactly(0))
		{
			
		}

//		public override string ToString()
//		{
//			return string.Format("(NOT {0})", SymbolId);
//		}
	}
}


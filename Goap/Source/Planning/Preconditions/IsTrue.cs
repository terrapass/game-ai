using System;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Planning.Preconditions
{
	public sealed class IsTrue : IsInRange
	{
		public IsTrue(SymbolId symbolId)
			//: base(symbolId, (value) => value != 0, (value) => (value != 0) ? 0.0 : 1.0)
			: base(symbolId, ValueRange.Exactly(1))
		{
			
		}

//		public override string ToString()
//		{
//			return string.Format("({0})", SymbolId);
//		}

//		public bool IsSatisfiedBy(WorldState worldState)
//		{
//			// TODO: Maybe check for UnknownSymbolException and return false if caught?
//			return (bool)worldState[this.symbolId];
//		}
	}
}


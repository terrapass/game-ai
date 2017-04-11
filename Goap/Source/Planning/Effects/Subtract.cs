using System;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Planning.Effects
{
	public sealed class Subtract : SingleSymbolEffect
	{
		private readonly int delta;

		public Subtract(SymbolId symbolId, int delta)
			: base(symbolId, (initialValue) => initialValue - delta/*, (initialValue) => initialValue + delta*/)
		{
			this.delta = delta;
		}

		public override int? ValueDelta
		{
			get {
				return -delta;
			}
		}
	}
}


using System;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Planning.Effects
{
	public sealed class SetTrue : SingleSymbolEffect
	{
		public SetTrue(SymbolId symbolId)
			: base(symbolId, (initialValue) => 1)
		{
			
		}

		public override int? ValueAssigned
		{
			get {
				return 1;
			}
		}
	}
}


using System;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Planning.Effects
{
	public abstract class SingleSymbolEffect : IEffect
	{
		private readonly SymbolId symbolId;
		private readonly Func<int, int> effectApplication;
		//private readonly Func<int, int> effectDeapplication;

		public SingleSymbolEffect(SymbolId symbolId, Func<int, int> effectApplication/*, Func<int, int> effectDeapplication = null*/)
		{
			this.symbolId = symbolId;
			this.effectApplication = PreconditionUtils.EnsureNotNull(effectApplication, "effectApplication");
//			this.effectDeapplication = effectDeapplication == null
//				? ((value) => value)	// Use identity as default deapplication
//				: effectDeapplication;
		}

		public WorldState ApplyTo(WorldState initialState)
		{
			return initialState.BuildUpon()
				.SetSymbol(this.symbolId, this.effectApplication(initialState[this.symbolId]))
				.Build();
		}

//		public WorldState UnapplyTo(WorldState initialState)
//		{
//			return initialState.BuildUpon()
//				.SetSymbol(this.symbolId, this.effectDeapplication(initialState[this.symbolId]))
//				.Build();
//		}

//		public IEnumerable<SymbolId> RelevantSymbols
//		{
//			get {
//				return new List<SymbolId>() { symbolId };
//			}
//		}

		public SymbolId SymbolId
		{
			get {
				return this.symbolId;
			}
		}

		public virtual int? ValueAssigned
		{
			get {
				return null;
			}
		}

		public virtual int? ValueDelta
		{
			get {
				return null;
			}
		}
	}
}


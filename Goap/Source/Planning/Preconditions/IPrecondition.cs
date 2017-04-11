using System;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Planning.Effects;

namespace Terrapass.GameAi.Goap.Planning.Preconditions
{
	public interface IPrecondition
	{
		bool IsSatisfiedBy(WorldState worldState);

		double GetDistanceFrom(WorldState worldState);

		// This property is really a bit of a crutch.
		// It works around the problem, where preconditions 
		// cannot be made to retrieve relevant symbols from IKnowledgeProvider
		// themselves, since they operate directly on WorldState.
		// Maybe instead WorldState should be made into a Cloneable class,
		// possibly via an intermediate IWorldStateAccessor : Cloneable interface,
		// whose another implementation would serve as a proxy, dispatching
		// symbol lookups to IKnowledgeProvider, if a corresponding symbol is
		// not yet present in the world state?
		//IEnumerable<SymbolId> RelevantSymbols {get;}

		/// <summary>
		/// Returns a new IPrecondition, based on this one, whose requirements
		/// are adjusted based on the given effect.
		/// E.g.: if this precondition is "Wood >= 20", adjusting for effect "Wood -= 20"
		/// will result in new precondition "Wood >= 0".
		/// </summary>
		/// <returns>New precondition.</returns>
		/// <param name="effect">Effect.</param>
		//IPrecondition AdjustFor(IEffect effect);

		SymbolId SymbolId { get; }
		ValueRange AsValueRange { get; }
		bool IsSatisfiable { get; }
	}
}


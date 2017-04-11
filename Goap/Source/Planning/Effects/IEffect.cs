using System;
using System.Collections.Generic;

namespace Terrapass.GameAi.Goap.Planning.Effects
{
	public interface IEffect
	{
		WorldState ApplyTo(WorldState initialState);
		//WorldState UnapplyTo(WorldState initialState);

		// This property is really a bit of a crutch.
		// It works around the problem, where effects
		// cannot be made to retrieve relevant symbols from IKnowledgeProvider
		// themselves, since they operate directly on WorldState.
		// Maybe instead WorldState should be made into a Cloneable class,
		// possibly via an intermediate IWorldStateAccessor : Cloneable interface,
		// whose another implementation would serve as a proxy, dispatching
		// symbol lookups to IKnowledgeProvider, if a corresponding symbol is
		// not yet present in the world state?
		//IEnumerable<SymbolId> RelevantSymbols {get;}

		SymbolId SymbolId { get; }
		int? ValueAssigned { get; }
		int? ValueDelta { get; }
	}
}


using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Planning.Internal
{
	internal class RelevantSymbolsPopulator : IWorldStatePopulator
	{
		private readonly IEnumerable<SymbolId> relevantSymbols;

		public RelevantSymbolsPopulator(IEnumerable<PlanningAction> availableActions, Goal goal)
		{
			this.relevantSymbols = PreconditionUtils.EnsureNotNull(availableActions, "availableActions").Aggregate(
				new List<SymbolId>(), 
				(soFar, action) => {
					soFar.AddRange(action.GetRelevantSymbols());
					return soFar;
				}
			).Concat(PreconditionUtils.EnsureNotNull(goal, "goal").PreconditionSymbols);
		}

		#region IWorldStatePopulator implementation
		public WorldState PopulateWorldState(IKnowledgeProvider knowledgeProvider, WorldState initialWorldState = default(WorldState))
		{
			PreconditionUtils.EnsureNotNull(knowledgeProvider, "knowledgeProvider");

			var builder = initialWorldState.BuildUpon();
			foreach(var symbolId in relevantSymbols)
			{
				if(!initialWorldState.Contains(symbolId))
				{
					builder.SetSymbol(symbolId, knowledgeProvider.GetSymbolValue(symbolId));
				}
			}
			return builder.Build();
		}
		#endregion
		
	}
}


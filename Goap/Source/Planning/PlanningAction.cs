using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Planning.Preconditions;
using Terrapass.GameAi.Goap.Planning.Effects;

namespace Terrapass.GameAi.Goap.Planning
{
	// TODO: Create ActionTemplate class to facilitate parametrized actions,
	// such as TakeFromStorage(RES_WOOD, 20).
	// Actual instances of Action would then be created from these templates
	// by ActionFactory.
	// For action templates there needs to be a way to specify parametric preconditions and effects,
	// where actual values, used in preconditions and effects, would depend/be equal to action parameters.
	// Action parameters can be integers for the time being.
	// E.g.: action TakeWoodFromStorage(X)
	//			preconds: 	NotLessThan(WoodInStorage, X)
	//			effects: 	Add(Wood, X)
	//						Subtract(WoodInStorage, X)
	// In the future, symbol IDs might also be parametrized.
	// E.g.: action TakeFromStorage(RES, X)
	//			preconds: 	NotLessThan(ResourcesInStorage(RES), X)
	//			effects: 	Add(AgentResources(RES), X)
	//						Subtract(ResourcesInStorage(RES), X)
	public sealed class PlanningAction
	{
		// TODO: Replace with (parametric) ActionId?
		public string Name { get; }

		// TODO: Replace enumerables with composites?
		private readonly IEnumerable<IPrecondition> preconditions;
		private readonly IEnumerable<IEffect> effects;

		public PlanningAction(string name, IEnumerable<IPrecondition> preconditions, IEnumerable<IEffect> effects, double cost)
		{
			this.Name = PreconditionUtils.EnsureNotBlank(name, "name");
			this.preconditions = new List<IPrecondition>(PreconditionUtils.EnsureNotNull(preconditions, "preconditions"));
			this.effects = new List<IEffect>(PreconditionUtils.EnsureNotNull(effects, "effects"));
			this.Cost = cost;
		}

		public IEnumerable<IPrecondition> Preconditions
		{
			get {
				return this.preconditions;
			}
		}

		public IEnumerable<IEffect> Effects
		{
			get {
				return this.effects;
			}
		}

		public bool IsAvailableIn(WorldState worldState)
		{
			// Equivalent to AND
			return preconditions.Aggregate(true, (soFar, precondition) => soFar && precondition.IsSatisfiedBy(worldState));
		}

		// TODO: Refactor
		public IEnumerable<SymbolId> PreconditionSymbols
		{
			get {
				return preconditions.Select((precond) => precond.SymbolId);
			}
		}

		// TODO: Refactor
		public IEnumerable<SymbolId> AffectedSymbols
		{
			get {
				return effects.Select((effect) => effect.SymbolId);
			}
		}

		public WorldState Apply(WorldState initialState)
		{
			// Equivalent to chaining effect applications
			return effects.Aggregate(initialState, (soFar, effect) => effect.ApplyTo(soFar));
		}

		// TODO: Add support for non-constant action cost
		// via something like GetCost(IKnowledgeProvider knowledgeProvider)
		// or GetCost(WorldState worldState),
		// which would allow the cost to be specified as Func<IKnowledgeProvider, double>
		// or Func<WorldState, double> respectively,
		// and thus be dependent on the current world state.
		public double Cost { get; }
	}

	// TODO: Refactor!
	public static class ActionExtensions
	{
		public static IEnumerable<SymbolId> GetRelevantSymbols(this PlanningAction action)
		{
			return action.PreconditionSymbols.Concat(action.AffectedSymbols);
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;

using Terrapass.GameAi.Goap.Planning.Preconditions;

using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Planning
{
	// TODO: Parametrization! (by analogy with Action class; see comment for Action)
	// Also see paper notes regarding goal preconditions, such as Maximize(RES_MONEY)
	// (A* implementation needs to be extended to return the best incomplete path, 
	// if no full path has been found .
	// TODO: Goal and Action interfaces look suspiciously similar, consider refactoring.
	public sealed class Goal
	{
		// TODO: Maybe replace with (parametric) GoalId to facilitate actions such as HaveAtLeast(RES_MONEY, 1000).
		public string Name {get;}

		// TODO: Replace with a composite (will also solve code duplication between Goal and Action).
		private readonly IEnumerable<IPrecondition> preconditions;

		public IEnumerable<IPrecondition> Preconditions
		{
			get {
				return this.preconditions;
			}
		}

		public Goal(string name, IEnumerable<IPrecondition> preconditions)
		{
			this.Name = PreconditionUtils.EnsureNotBlank(name, "name");
			this.preconditions = new List<IPrecondition>(PreconditionUtils.EnsureNotNull(preconditions, "preconditions"));
		}

		public bool IsReachedIn(WorldState worldState)
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

		// Can this really be used as a heuristic for forward planning????
		public double GetDistanceFrom(WorldState worldState)
		{
			return this.preconditions.Sum((precondition) => precondition.GetDistanceFrom(worldState));
		}
	}
}

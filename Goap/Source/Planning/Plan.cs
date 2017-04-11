using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Planning
{
	public sealed class Plan
	{
		public IEnumerable<PlanningAction> Actions { get; }
		public Goal Goal { get; }
		// TODO: This should actually be an ordered collection of action ids,
		//public IEnumerable<ActionId> ActionIds {get;}

		public int Length
		{
			get {
				return Actions.Count();
			}
		}

		public double Cost
		{
			get {
				return Actions.Sum((action) => action.Cost);
			}
		}

		public Plan(IEnumerable<PlanningAction> actions, Goal goal)
		{
			this.Actions = new List<PlanningAction>(PreconditionUtils.EnsureNotNull(actions, "actions")).AsReadOnly();
			this.Goal = PreconditionUtils.EnsureNotNull(goal, "goal");
		}

		public override string ToString()
		{
			string result = "";
			bool first = true;
			foreach(string actionName in this.Actions.Select((action) => action.Name))
			{
				if(!first)
				{
					result += " -> ";
				}
				result += actionName;
				first = false;
			}
			return result;
		}
	}
}


using System;
using Terrapass.GameAi.Goap.Logging;
using System.Collections.Generic;
using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Planning
{
	public class BlockingPlanningSystem : IPlanningSystem
	{
		private readonly IPlanner planner;
		//private readonly ILogger logger;
		private IDictionary<int, Plan> plans;

		private int nextId = 0;

		public BlockingPlanningSystem(IPlanner planner, ILogger logger = null)
		{
			this.planner = PreconditionUtils.EnsureNotNull(planner, nameof(planner));
			//this.logger = logger != null ? logger : new NullLogger();
			this.plans = new Dictionary<int, Plan>();
		}

		#region IPlanningSystem implementation
		public int RequestPlan(
			IKnowledgeProvider knowledgeProvider, 
			IEnumerable<PlanningAction> availableActions, 
			Goal goal
		)
		{
			PreconditionUtils.EnsureNotNull(knowledgeProvider, nameof(knowledgeProvider));
			PreconditionUtils.EnsureNotNull(availableActions, nameof(availableActions));
			PreconditionUtils.EnsureNotNull(goal, nameof(goal));

			var id = nextId++;

			try
			{
				this.plans.Add(id, this.planner.FormulatePlan(knowledgeProvider, availableActions, goal));
			}
			catch(PlanNotFoundException)
			{
				this.plans.Add(id, null);
			}

			return id;
		}

		public void CancelPlanning(int id)
		{
			this.plans.Remove(id);
		}

		public PlanningStatus GetPlanningStatus(int id)
		{
			return plans[id] == null
				? PlanningStatus.FAILED
				: PlanningStatus.SUCCESSFUL;
		}

		public Plan RetrievePlan(int id)
		{
			var result = plans[id];
			plans.Remove(id);
			return result;
		}
		#endregion
	}
}


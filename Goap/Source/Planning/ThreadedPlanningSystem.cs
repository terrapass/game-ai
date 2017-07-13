using System;
using System.Collections.Generic;
using System.Threading;

using Terrapass.GameAi.Goap.Debug;
using Terrapass.GameAi.Goap.Logging;

namespace Terrapass.GameAi.Goap.Planning
{
	public class ThreadedPlanningSystem : IPlanningSystem
	{
		private sealed class PlanningTask
		{
			private readonly ThreadedPlanningSystem planningSystem;

			private readonly IKnowledgeProvider knowledgeProvider;
			private readonly IEnumerable<PlanningAction> availableActions;
			private readonly Goal goal;

			private volatile bool isComplete;
			private volatile Plan plan;

			public bool IsComplete
			{
				get {
					return this.isComplete;
				}
			}

			public Plan Plan
			{
				get {
					return this.plan;
				}
			}

			public PlanningTask(
				ThreadedPlanningSystem planningSystem, 
				IKnowledgeProvider knowledgeProvider,
				IEnumerable<PlanningAction> availableActions,
				Goal goal
			)
			{
				DebugUtils.Assert(planningSystem != null, "planning system must not be null");

				DebugUtils.Assert(knowledgeProvider != null, "knowledgeProvider must not be null");
				DebugUtils.Assert(availableActions != null, "availableActions must not be null");
				DebugUtils.Assert(goal != null, "goal must not be null");

				this.planningSystem = planningSystem;

				this.knowledgeProvider = knowledgeProvider;
				this.availableActions = availableActions;
				this.goal = goal;

				this.isComplete = false;
				this.plan = null;
			}

			public void Execute(Object context)
			{
				// FIXME: Logger might not be thread-safe.
				//planningSystem.logger.LogInfo("Executing planning request for goal {0}...", planRequest.Goal.Name);

				try
				{
					this.plan = this.planningSystem.planner.FormulatePlan(
						this.knowledgeProvider,
						this.availableActions, 
						this.goal
					);
				}
				catch(PlanNotFoundException)
				{
					// TODO: Log?
					this.plan = null;
				}
				this.isComplete = true;

				//planningSystem.logger.LogInfo("Finished executing planning request for goal {0}", planRequest.Goal.Name);
			}
		}

		private readonly IPlanner planner;
		private readonly ILogger logger;
		private IDictionary<int, PlanningTask> planningTasks;

		private int nextId = 0;

		public ThreadedPlanningSystem(IPlanner planner, ILogger logger = null)
		{
			this.planner = PreconditionUtils.EnsureNotNull(planner, nameof(planner));
			this.logger = logger != null ? logger : new NullLogger();
			this.planningTasks = new Dictionary<int, PlanningTask>();
		}

		#region IPlanningSystem implementation
		public int RequestPlan(
			IKnowledgeProvider knowledgeProvider, // THIS THING ACCESSES A UNITY OBJECT => NOT THREAD SAFE
			IEnumerable<PlanningAction> availableActions, 
			Goal goal
		)
		{
			PreconditionUtils.EnsureNotNull(knowledgeProvider, nameof(knowledgeProvider));
			PreconditionUtils.EnsureNotNull(availableActions, nameof(availableActions));
			PreconditionUtils.EnsureNotNull(goal, nameof(goal));

			var task = new PlanningTask(this, knowledgeProvider, availableActions, goal);
			logger.LogInfo("Received logging request for goal {0}", goal.Name);
			ThreadPool.QueueUserWorkItem(task.Execute);
			int id = nextId++;
			this.planningTasks.Add(id, task);
			return id;
		}

		public void CancelPlanning(int id)
		{
			// TODO: Cancel task
			this.planningTasks.Remove(id);
		}

		public PlanningStatus GetPlanningStatus(int id)
		{
			return !planningTasks[id].IsComplete
				? PlanningStatus.IN_PROGRESS
				: planningTasks[id].Plan == null
					? PlanningStatus.FAILED
					: PlanningStatus.SUCCESSFUL;
		}

		public Plan RetrievePlan(int id)
		{
			var result = planningTasks[id].Plan;
			planningTasks.Remove(id);
			return result;
		}
		#endregion
	}
}


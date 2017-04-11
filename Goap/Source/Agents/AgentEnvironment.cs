using System;
using System.Collections.Generic;
using System.Linq;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Agents
{
	public sealed class AgentEnvironment
	{
		public IGoalSelector GoalSelector { get; }
		public IPlanner Planner { get; }
		public IKnowledgeProvider KnowledgeProvider { get; }
		public IPlanExecutor PlanExecutor { get; }
		//public IReevaluationSensor Sensor { get; }
		// etc.

		public AgentEnvironment(
			IGoalSelector goalSelector,
			IPlanner planner,
			IKnowledgeProvider knowledgeProvider,
			IPlanExecutor planExecutor
		)
		{
			this.GoalSelector = PreconditionUtils.EnsureNotNull(goalSelector, "goalSelector");
			this.Planner = PreconditionUtils.EnsureNotNull(planner, "planner");
			this.KnowledgeProvider = PreconditionUtils.EnsureNotNull(knowledgeProvider, "knowledgeProvider");
			this.PlanExecutor = PreconditionUtils.EnsureNotNull(planExecutor, "planExecutor");
		}

		public IPlanExecution CurrentPlanExecution
		{
			get {
				return this.PlanExecutor.CurrentExecution;
			}
		}

		public IEnumerable<PlanningAction> SupportedPlanningActions
		{
			get {
				return this.PlanExecutor.SupportedPlanningActions;
			}
		}

		public class Builder
		{
			private IGoalSelector goalSelector;
			private IPlanner planner;
			private IKnowledgeProvider knowledgeProvider;
			private IPlanExecutor planExecutor;

			public Builder()
			{
				
			}

			public Builder WithGoalSelector(IGoalSelector goalSelector)
			{
				this.goalSelector = PreconditionUtils.EnsureNotNull(goalSelector, "goalSelector");
				return this;
			}

			public Builder WithPlanner(IPlanner planner)
			{
				this.planner = PreconditionUtils.EnsureNotNull(planner, "planner");
				return this;
			}

			public Builder WithKnowledgeProvider(IKnowledgeProvider knowledgeProvider)
			{
				this.knowledgeProvider = PreconditionUtils.EnsureNotNull(knowledgeProvider, "knowledgeProvider");
				return this;
			}

			public Builder WithPlanExecutor(IPlanExecutor planExecutor)
			{
				this.planExecutor = PreconditionUtils.EnsureNotNull(planExecutor, "planExecutor");
				return this;
			}

			public AgentEnvironment Build()
			{
				try
				{
					return new AgentEnvironment(goalSelector, planner, knowledgeProvider, planExecutor);
				}
				catch(ArgumentNullException)
				{
					throw new InvalidOperationException(
						string.Format(
							"{0} hasn't been properly configured before the call to Build()",
							this.GetType()
						)
					);
				}
			}
		}
	}
}


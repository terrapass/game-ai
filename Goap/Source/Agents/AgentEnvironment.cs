using System;
using System.Collections.Generic;
using System.Linq;

using Terrapass.GameAi.Goap.Debug;
using Terrapass.GameAi.Goap.Logging;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Agents
{
	public sealed class AgentEnvironment
	{
		public IGoalSelector GoalSelector { get; }
		public IPlanningSystem PlanningSystem { get; }
		public IKnowledgeProvider KnowledgeProvider { get; }
		public IPlanExecutor PlanExecutor { get; }
		public IReevaluationSensor ReevaluationSensor {get;}
		// etc.

		public ILogger Logger { get; }

		public AgentEnvironment(
			IGoalSelector goalSelector,
			IPlanningSystem planningSystem,
			IKnowledgeProvider knowledgeProvider,
			IPlanExecutor planExecutor,
			IReevaluationSensor reevaluationSensor = null,
			ILogger logger = null
		)
		{
			this.GoalSelector = PreconditionUtils.EnsureNotNull(goalSelector, "goalSelector");
			this.PlanningSystem = PreconditionUtils.EnsureNotNull(planningSystem, "planningSystem");
			this.KnowledgeProvider = PreconditionUtils.EnsureNotNull(knowledgeProvider, "knowledgeProvider");
			this.PlanExecutor = PreconditionUtils.EnsureNotNull(planExecutor, "planExecutor");
			this.ReevaluationSensor = reevaluationSensor != null ? reevaluationSensor : new NullReevaluationSensor();

			this.Logger = logger != null ? logger : new NullLogger();
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
			private IPlanningSystem planningSystem;
			private IKnowledgeProvider knowledgeProvider;
			private IPlanExecutor planExecutor;
			private IReevaluationSensor reevaluationSensor;
			private ILogger logger;

			public Builder()
			{
				
			}

			public Builder WithGoalSelector(IGoalSelector goalSelector)
			{
				this.goalSelector = PreconditionUtils.EnsureNotNull(goalSelector, "goalSelector");
				return this;
			}

			public Builder WithPlanningSystem(IPlanningSystem planningSystem)
			{
				this.planningSystem = PreconditionUtils.EnsureNotNull(planningSystem, "planningSystem");
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

			public Builder WithReevaluationSensor(IReevaluationSensor reevaluationSensor)
			{
				this.reevaluationSensor = reevaluationSensor;
				return this;
			}

			public Builder WithLogger(ILogger logger)
			{
				this.logger = logger;
				return this;
			}

			public AgentEnvironment Build()
			{
				try
				{
					return new AgentEnvironment(goalSelector, planningSystem, knowledgeProvider, planExecutor, reevaluationSensor, logger);
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


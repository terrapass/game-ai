using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Planning;
using Terrapass.GameAi.Goap.Time;

using Terrapass.GameAi.Goap.Planning.Preconditions;

namespace Terrapass.GameAi.Goap.Agents
{
	public delegate double GoalEvaluator();

	public class GoalSelector : IGoalSelector
	{
		public const float DEFAULT_REEVALUATION_PERIOD = 0.0f;
		public static readonly Goal IDLE_GOAL = new Goal("Idle", new List<IPrecondition>());

		private readonly IDictionary<Goal, GoalEvaluator> evaluators;
		private readonly float reevaluationPeriod;
		private readonly IResettableTimer reevaluationTimer;

		private IEnumerable<Goal> relevantGoals;

		public GoalSelector(IDictionary<Goal, GoalEvaluator> evaluators, float reevaluationPeriod = DEFAULT_REEVALUATION_PERIOD)
		{
			this.evaluators = PreconditionUtils.EnsureNotNull(evaluators, "evaluators");
			this.reevaluationPeriod = reevaluationPeriod;
			this.reevaluationTimer = new ResettableStopwatchExecutionTimer(false);

			this.ForceReevaluation();
		}

		#region IGoalSelector implementation
		public IEnumerable<Goal> RelevantGoals
		{
			get {
				if(relevantGoals == null || reevaluationTimer.ElapsedSeconds > reevaluationPeriod)
				{
					this.ForceReevaluation();
				}
				return this.relevantGoals;
			}
		}
		public Goal DefaultGoal
		{
			get {
				return IDLE_GOAL;
			}
		}

		public void ForceReevaluation()
		{
			this.relevantGoals = from relevanceKvp in (
					from evalKvp in evaluators 
					select new KeyValuePair<Goal, double>(evalKvp.Key, evalKvp.Value())
				) where relevanceKvp.Value > 0
				orderby relevanceKvp.Value descending
				select relevanceKvp.Key;

			reevaluationTimer.Reset(false);
		}
		#endregion

		public class Builder
		{
			private IDictionary<Goal, GoalEvaluator> evaluators;
			private float reevaluationPeriod;

			public Builder()
			{
				this.evaluators = new Dictionary<Goal, GoalEvaluator>();
				this.reevaluationPeriod = DEFAULT_REEVALUATION_PERIOD;
			}

			public Builder WithGoal(Goal goal, GoalEvaluator evaluator)
			{
				this.evaluators.Add(goal, evaluator);
				return this;
			}

			public Builder WithReevaluationPeriod(float reevaluationPeriod)
			{
				this.reevaluationPeriod = reevaluationPeriod;
				return this;
			}

			public GoalSelector Build()
			{
				return new GoalSelector(
					this.evaluators,
					this.reevaluationPeriod
				);
			}
		}
	}
}


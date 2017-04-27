using System;
using System.Collections.Generic;
using System.Linq;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Agents
{
	public class Agent : IAgent
	{
		private readonly AgentEnvironment env;

		private Goal currentGoal;

		public Agent(AgentEnvironment environment)
		{
			this.env = PreconditionUtils.EnsureNotNull(environment, nameof(environment));
			this.currentGoal = environment.GoalSelector.DefaultGoal;
		}

		public void Update()
		{
			// If the plan has been completed, interrupted or there is no plan yet,
			// select a new goal, plan for it and execute the plan.
			if(env.CurrentPlanExecution.Status == ExecutionStatus.None
			   || env.CurrentPlanExecution.Status == ExecutionStatus.Complete
			   || env.CurrentPlanExecution.Status == ExecutionStatus.Interrupted)
			{
				// If the plan was interrupted, force goal reevaluation and plan for the most relevant goal,
				// if not - don't force reevalutaion and just plan for the most relevant goal.
				this.AchieveRelevantGoal(env.CurrentPlanExecution.Status == ExecutionStatus.Interrupted);
			}
			// If the execution of the current plan has failed, attepmt to replan for the same goal,
			// if replanning fails, select a new goal and proceed with it.
			else if(env.CurrentPlanExecution.Status == ExecutionStatus.Failed)
			{
				try
				{
					// Attempt to re-plan for the current goal
					env.PlanExecutor.SubmitForExecution(
						env.Planner.FormulatePlan(env.KnowledgeProvider, env.SupportedPlanningActions, this.currentGoal)
					);
				}
				catch(PlanNotFoundException)
				{
					// Find the most relevant goal and plan for it
					this.AchieveRelevantGoal(false);
				}
			}
			else if(env.CurrentPlanExecution.Status == ExecutionStatus.InProgress)
			{
				// If reevaluation sensor fires, force goal reevaluation and,
				// if a different goal is selected than the one currently pursued,
				// interrupt the current plan execution.
				if(env.ReevaluationSensor.IsReevaluationNeeded)
				{
					env.GoalSelector.ForceReevaluation();
					var mostRelevantGoal = env.GoalSelector.RelevantGoals.FirstOrDefault();
					if(mostRelevantGoal != null
					   && !env.PlanExecutor.CurrentExecution.Plan.Goal.Equals(mostRelevantGoal))
					{
						env.PlanExecutor.InterruptExecution();
					}
				}
			}

			env.PlanExecutor.Update();
		}

		private Plan GetPlanFor(Goal goal)
		{
			return env.Planner.FormulatePlan(env.KnowledgeProvider, env.SupportedPlanningActions, goal);
		}

		private void AchieveRelevantGoal(bool forceReevaluation)
		{
			if(forceReevaluation)
			{
				env.GoalSelector.ForceReevaluation();
			}

			var relevantGoals = env.GoalSelector.RelevantGoals;
			bool planSubmitted = false;
			foreach(var goal in relevantGoals)
			{
				try
				{						
					env.PlanExecutor.SubmitForExecution(this.GetPlanFor(goal));
					this.currentGoal = goal;
					planSubmitted = true;
					break;
				}
				catch(PlanNotFoundException)
				{
					// TODO: Log
					continue;
				}
			}

			// If no relevant goal can be achieved, use the default goal.
			if(!planSubmitted)
			{
				env.PlanExecutor.SubmitForExecution(this.GetPlanFor(env.GoalSelector.DefaultGoal));
			}
		}
	}
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Terrapass.GameAi.Goap.Debug;
using Terrapass.GameAi.Goap.Logging;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Agents
{
	public class Agent : IAgent
	{
		private readonly AgentEnvironment env;

		private int? currentPlanningId;

		private IEnumerator<Goal> relevantGoalEnumerator;
		private bool resetGoalEnumeration;

		private ExecutionStatus lastExecutionStatus;

		public Agent(AgentEnvironment environment)
		{
			this.env = PreconditionUtils.EnsureNotNull(environment, nameof(environment));

			this.currentPlanningId = null;

			this.resetGoalEnumeration = false;

			this.lastExecutionStatus = ExecutionStatus.Interrupted;
		}

		public void Update()
		{
			env.Logger.LogDebug(
				"cur. plan ID: {0}{1}, plan exec. status: {2}",
				currentPlanningId,
				currentPlanningId == null
					? ""
					: string.Format(" (status: {0})", env.PlanningSystem.GetPlanningStatus(currentPlanningId.Value)),
				env.CurrentPlanExecution.Status
			);

			Plan activePlan = null;

			// Wait for planning to finish
			if(currentPlanningId.HasValue)
			{
				if(env.PlanningSystem.GetPlanningStatus(currentPlanningId.Value) == PlanningStatus.IN_PROGRESS)
				{
					return;
				}
				else
				{
					activePlan = env.PlanningSystem.RetrievePlan(currentPlanningId.Value);
					currentPlanningId = null;
				}
			}

			bool executionStatusChanged = (env.CurrentPlanExecution.Status != lastExecutionStatus);

			// If the plan has been completed, interrupted or there is no plan yet,
			// select a new goal, plan for it and execute the plan.
			if(env.CurrentPlanExecution.Status == ExecutionStatus.None
			   || env.CurrentPlanExecution.Status == ExecutionStatus.Complete
			   || env.CurrentPlanExecution.Status == ExecutionStatus.Interrupted)
			{
				// If the plan was interrupted, force goal reevaluation and plan for the most relevant goal,
				// if not - don't force reevalutaion and just plan for the most relevant goal.
				//this.AchieveRelevantGoal(env.CurrentPlanExecution.Status == ExecutionStatus.Interrupted);
				if(activePlan != null)
				{
					env.PlanExecutor.SubmitForExecution(activePlan);
				}
				else
				{
					if(executionStatusChanged)
					{
						// TODO: Also make the agent do "thinking" action until the plan is formulated.
						// Corresponding instruction to plan executor (?) must be somewhere here
						// but must only be performed once per selection-planning process.

						if(env.CurrentPlanExecution.Status == ExecutionStatus.Interrupted)
						{
							env.GoalSelector.ForceReevaluation();
						}

						this.ResetGoalEnumeration();
					}

					// If there are other relevant goals, attempt planning for the next relevant goal
					if(RelevantGoalEnumerator.MoveNext())
					{
						currentPlanningId = StartPlanningFor(RelevantGoalEnumerator.Current);
					}
					// If we've reached the end of the relevant goals enumeration, reevaluate
					else
					{
						env.GoalSelector.ForceReevaluation();
					}
				}
			}
			// If the execution of the current plan has failed, attepmt to replan for the same goal,
			// if replanning fails, select a new goal and proceed with it.
			else if(env.CurrentPlanExecution.Status == ExecutionStatus.Failed)
			{
				if(executionStatusChanged)
				{
					// Attempt to re-plan for the current goal
					currentPlanningId = StartPlanningFor(CurrentGoal);

					this.ResetGoalEnumeration();

					// TODO: Also make the agent do "re-thinking" action until the plan is formulated.
					// Corresponding instruction to plan executor (?) must be somewhere here
					// but must only be performed once per selection-planning process.
				}
				else
				{
					if(activePlan != null)
					{
						env.PlanExecutor.SubmitForExecution(activePlan);
					}
					else
					{
						if(RelevantGoalEnumerator.MoveNext())
						{
							currentPlanningId = StartPlanningFor(RelevantGoalEnumerator.Current);
						}
						else
						{
							env.GoalSelector.ForceReevaluation();
						}
					}
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
						&& !mostRelevantGoal.Equals(CurrentGoal))
					{
						env.PlanExecutor.InterruptExecution();
					}
				}
			}

			this.lastExecutionStatus = env.PlanExecutor.CurrentExecution.Status;

			env.PlanExecutor.Update();
		}

		private int StartPlanningFor(Goal goal)
		{
			return env.PlanningSystem.RequestPlan(env.KnowledgeProvider, env.SupportedPlanningActions, goal);
		}

		private IEnumerator<Goal> RelevantGoalEnumerator
		{
			get {
				if(this.relevantGoalEnumerator == null || this.resetGoalEnumeration)
				{
					env.Logger.LogDebug("Resetting relevant goals enumerator");
					this.relevantGoalEnumerator = this.RelevantGoalIteratorBlock;
					this.resetGoalEnumeration = false;
				}
				return this.relevantGoalEnumerator;
			}
		}

		private IEnumerator<Goal> RelevantGoalIteratorBlock
		{
			get {
				var relevantGoals = env.GoalSelector.RelevantGoals;
				env.Logger.LogDebug("There are {0} relevant goals", relevantGoals.Count());
				foreach(var goal in relevantGoals)
				{
					env.Logger.LogDebug("Selecting {0} from {1} relevant goals", goal.Name, relevantGoals.Count());
					yield return goal;
				}
			}
		}

		private void ResetGoalEnumeration()
		{
			this.resetGoalEnumeration = true;
		}

		private Goal CurrentGoal
		{
			get {
				return env.PlanExecutor.CurrentExecution.Plan.Goal;
			}
		}
	}
}


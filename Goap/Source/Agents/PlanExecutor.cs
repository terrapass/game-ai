using System;
using System.Collections.Generic;
using System.Linq;

using Terrapass.GameAi.Goap.Planning;
using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Agents
{
	public class PlanExecutor : IPlanExecutor, IPlanExecution
	{
		private readonly IActionFactory actionFactory;

		private ExecutionStatus currentExecutionStatus;
		private Plan plan;
		private int currentActionIndex;	// TODO: Store and manipulate an enumerator instead.

		private IAction currentAction;	// ditto

		public PlanExecutor(IActionFactory actionFactory)
		{
			this.actionFactory = PreconditionUtils.EnsureNotNull(actionFactory, "actionFactory");

			this.currentExecutionStatus = ExecutionStatus.None;
			this.plan = null;
			this.currentActionIndex = -1;

			this.currentAction = null;
		}

		#region IPlanExecutor implementation

		public void SubmitForExecution(Plan plan)
		{
			if(!this.currentExecutionStatus.IsFinal() && this.currentExecutionStatus != ExecutionStatus.None)
			{
				throw new InvalidOperationException(
					string.Format(
						"SubmitForExecution() was called before current plan was fully completed, failed or interrupted (plan status: {0})",
						currentExecutionStatus
					)
				);
			}

			this.plan = plan;

			if(plan.Length > 0)
			{
				this.currentExecutionStatus = ExecutionStatus.InProgress;
				this.currentActionIndex = 0;

				this.currentAction = actionFactory.FromPlanningAction(plan.Actions.First().Name);
			}
			else
			{
				this.currentExecutionStatus = ExecutionStatus.Complete;
				this.currentActionIndex = -1;

				this.currentAction = null;
			}
		}

		public void InterruptExecution()
		{
			if(this.plan == null)
			{
				throw new InvalidOperationException(string.Format("InterruptExecution() cannot be called when {0} has no current plan", this.GetType()));
			}

			if(this.currentExecutionStatus == ExecutionStatus.InProgress)
			{
				this.currentExecutionStatus = ExecutionStatus.InInterruption;

				if(this.currentAction.Status == ExecutionStatus.InProgress)
				{
					this.currentAction.StartInterruption();
				}
			}
		}

		public void Update()
		{
			if(this.currentExecutionStatus != ExecutionStatus.None && !this.currentExecutionStatus.IsFinal())
			{
				DebugUtils.Assert(this.currentAction != null, "currentAction must not be null at this point");

				if(this.currentAction.Status == ExecutionStatus.None)
				{
					this.currentAction.StartExecution();
				}

				this.currentAction.Update();

				if(this.currentAction.Status == ExecutionStatus.Interrupted || this.currentAction.Status == ExecutionStatus.Failed)
				{
					this.currentExecutionStatus = this.currentAction.Status;
				}
				else if(this.currentAction.Status == ExecutionStatus.Complete)
				{
					this.currentActionIndex++;

					if(this.currentActionIndex >= this.plan.Length)
					{
						this.currentAction = null;
						this.currentExecutionStatus = ExecutionStatus.Complete;
					}
					else
					{
						this.currentAction = actionFactory.FromPlanningAction(plan.Actions.ElementAt(currentActionIndex).Name);
					}
				}
			}
		}

		public IPlanExecution CurrentExecution
		{
			get {
				return this;
			}
		}

		#endregion

		#region IPlanExecution implementation

		public ExecutionStatus Status
		{
			get {
				return this.currentExecutionStatus;
			}
		}

		public Plan Plan
		{
			get {
				return this.plan;
			}
		}

		public int CurrentActionIndex
		{
			get {
				return this.currentActionIndex;
			}
		}

		public IEnumerable<PlanningAction> SupportedPlanningActions
		{
			get {
				return this.actionFactory.SupportedPlanningActions;
			}
		}

		#endregion
	}
}


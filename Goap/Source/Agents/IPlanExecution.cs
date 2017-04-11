using System;
using System.Linq;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Agents
{
	public interface IPlanExecution
	{
		ExecutionStatus Status {get;}
		Plan Plan {get;}
		int CurrentActionIndex {get;}
	}

	public static class PlanExecutionExtensions
	{
		public static PlanningAction GetCurrentAction(this IPlanExecution execution)
		{
			if(!execution.Status.IsCurrentActionAvailable())
			{
				throw new InvalidOperationException(
					string.Format(
						"Current action is may not be retrieved when plan execution status is {0}",
						execution.Status
					)
				);
			}
			return execution.Plan.Actions.ElementAt(execution.CurrentActionIndex);
		}
	}
}


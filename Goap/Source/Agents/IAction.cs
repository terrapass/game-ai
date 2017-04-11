using System;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Agents
{
	public interface IAction
	{
		//PlanningAction AsPlanningAction {get;}

		void StartExecution();
		void StartInterruption();

		void Update();

		ExecutionStatus Status {get;}
	}
}


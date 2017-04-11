using System;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Agents
{
	public interface IPlanExecutor
	{
		void SubmitForExecution(Plan plan);
		void InterruptExecution();

		IPlanExecution CurrentExecution {get;}

//		void AddAction(/*ActionId*/string name, IAction action);
//		void RemoveAction(/*ActionId*/string name);

		void Update();

		IEnumerable<PlanningAction> SupportedPlanningActions { get; }
	}
}


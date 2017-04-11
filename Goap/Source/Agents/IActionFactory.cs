using System;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Agents
{
	public interface IActionFactory
	{
		IEnumerable<PlanningAction> SupportedPlanningActions {get;}

		IAction FromPlanningAction(/*ActionId*/string planningActionName);
	}
}


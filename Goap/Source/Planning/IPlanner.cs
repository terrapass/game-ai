using System;
using System.Collections.Generic;

namespace Terrapass.GameAi.Goap.Planning
{
	public interface IPlanner
	{
		Plan FormulatePlan(IKnowledgeProvider knowledgeProvider, IEnumerable<PlanningAction> availableActions, Goal goal);
	}
}


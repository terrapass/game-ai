using System;
using System.Collections.Generic;

namespace Terrapass.GameAi.Goap.Planning
{
	public interface IPlanningSystem
	{
		int RequestPlan(
			IKnowledgeProvider knowledgeProvider,
			IEnumerable<PlanningAction> availableActions,
			Goal goal
		);
		void CancelPlanning(int id);

		PlanningStatus GetPlanningStatus(int id);
		Plan RetrievePlan(int id);
	}
}

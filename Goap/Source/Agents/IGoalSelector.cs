using System;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Agents
{
	public interface IGoalSelector
	{
		/// <summary>
		/// Returns a collection, containing all of the currently relevant goals (relevance > 0),
		/// sorted from most to least relevant.
		/// Irrelevant goals (relevance <= 0) are not included.
		/// </summary>
		/// <value>Relevant goals.</value>
		IEnumerable<Goal> RelevantGoals {get;}

		/// <summary>
		/// Goal to use when there are either no relevant goals or no plan could be formulated
		/// for relevant goals.
		/// </summary>
		/// <value>The default goal.</value>
		Goal DefaultGoal {get;}

		void ForceReevaluation();
	}
}


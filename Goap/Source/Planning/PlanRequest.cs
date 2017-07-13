using System;
using System.Collections.Generic;

namespace Terrapass.GameAi.Goap.Planning
{
	public struct PlanRequest
	{
		public enum Kind
		{
			TRIVIAL = 0,
			REGULAR = 1,
			IMPOSSIBLE = 2
		};

		public IKnowledgeProvider KnowledgeProvider { get; }
		public IEnumerable<PlanningAction> AvailableActions { get; }
		public Goal Goal {get;}

		public PlanRequest(
			IKnowledgeProvider knowledgeProvider,
			IEnumerable<PlanningAction> availableActions,
			Goal goal
		)
		{
			this.KnowledgeProvider = knowledgeProvider;
			this.AvailableActions = availableActions;
			this.Goal = goal;
		}

		public Kind RequestKind
		{
			get {
				if(Goal == null)
				{
					return Kind.TRIVIAL;
				}
				else if(AvailableActions == null || KnowledgeProvider == null)
				{
					return Kind.IMPOSSIBLE;
				}
				return Kind.REGULAR;
			}
		}
	}
}


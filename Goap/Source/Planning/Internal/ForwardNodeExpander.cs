using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Graphs;

namespace Terrapass.GameAi.Goap.Planning.Internal
{
	internal class ForwardNodeExpander : INodeExpander<ForwardNode>
	{
		private readonly IEnumerable<PlanningAction> availableActions;

		public ForwardNodeExpander(IEnumerable<PlanningAction> availableActions)
		{
			this.availableActions = PreconditionUtils.EnsureNotNull(availableActions, "availableActions");
		}


		#region INodeExpander implementation
		public IEnumerable<IGraphEdge<ForwardNode>> ExpandNode(ForwardNode node)
		{
			if(node.IsGoal)
			{
				throw new ArgumentException(
					string.Format("Goal {0} cannot be expanded", node.GetType()),
					"node"
				);
			}

			return (from action in availableActions 
					where action.IsAvailableIn(node.WorldState) 
				select new ForwardEdge(
					action,
					node,
					ForwardNode.MakeRegularNode(
						action.Apply(node.WorldState),
						this
					)
				)).Cast<IGraphEdge<ForwardNode>>();
		}
		#endregion
	}
}


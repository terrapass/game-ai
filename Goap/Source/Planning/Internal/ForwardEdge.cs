using System;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Graphs;

namespace Terrapass.GameAi.Goap.Planning.Internal
{
	internal class ForwardEdge : IGraphEdge<ForwardNode>
	{
		public PlanningAction Action { get; }

		public ForwardEdge(PlanningAction action, ForwardNode sourceNode, ForwardNode targetNode)
		{
			this.Action = PreconditionUtils.EnsureNotNull(action, "action");
			this.SourceNode = sourceNode;
			this.TargetNode = targetNode;
		}

		#region IGraphEdge implementation

		public double Cost
		{
			get {
				return this.Action.Cost;
			}
		}

		public ForwardNode SourceNode { get; }

		public ForwardNode TargetNode { get; }

		#endregion
	}
}


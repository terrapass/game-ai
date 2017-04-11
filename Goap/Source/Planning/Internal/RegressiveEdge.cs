using System;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Graphs;

namespace Terrapass.GameAi.Goap.Planning.Internal
{
	internal class RegressiveEdge : IGraphEdge<RegressiveNode>
	{
		public PlanningAction Action { get; }

		public RegressiveEdge(PlanningAction action, RegressiveNode sourceNode, RegressiveNode targetNode)
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

		public RegressiveNode SourceNode { get; }

		public RegressiveNode TargetNode { get; }

		#endregion
	}
}


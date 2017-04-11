using System;

namespace Terrapass.GameAi.Goap.Graphs
{
	/// <summary>
	/// This is the required signature for path cost heuristic functions, 
	/// utilized by <see cref="T:AstarPathfinder"/> and,
	/// possibly in the future, other pathfinders.
	/// </summary>
	public delegate double PathCostHeuristic<in GraphNode>(
		GraphNode sourceNode,
		GraphNode targetNode
	) where GraphNode : IGraphNode<GraphNode>;
}


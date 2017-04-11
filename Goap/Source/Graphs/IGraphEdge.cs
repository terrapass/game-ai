using System;

namespace Terrapass.GameAi.Goap.Graphs
{
	/// <summary>
	/// Interface for abstract graph edges.
	/// </summary>
	public interface IGraphEdge<GraphNode> where GraphNode : IGraphNode<GraphNode>
	{
		Double Cost {get;}

		// FIXME: Edge having a reference to both source and target nodes
		// may lead to a loss of data consistency.
		// E.g. node A might have edge E in its OutgoingEdges,
		// but E might have node B (B != A) set as its SourceNode. Oops.
		// Actually A* doesn't even need this reference to SourceNode.
		// Either SourceNode should be removed, or graph construction
		// should always be encapsulated in a factory, which would
		// ensure that this constraint always holds.
		GraphNode SourceNode {get;}
		GraphNode TargetNode {get;}
	}
}


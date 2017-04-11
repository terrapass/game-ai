using System;
using System.Linq;
using System.Collections.Generic;

namespace Terrapass.GameAi.Goap.Graphs
{
	/// <summary>
	/// Interface for abstract graph nodes.
	/// </summary>
	/// <remarks>
	/// A generic construct, similar to curiously recurring template pattern from C++,
	/// is used in this case to guarantee type safety when calling PathCostHeuristic from a pathfinder.
	/// Using generics removes the need for the PathCostHeuristic implementer 
	/// to downcast from IGraphNode to the concrete node type in order to calculate heuristic.
	/// This CRTP-like construct is needed in this case because of OutgoingEdges property, 
	/// which needs to be of type "IEnumerable of exactly the same type implementing IGraphNode".
	/// </remarks>
	public interface IGraphNode<GraphNode> where GraphNode : IGraphNode<GraphNode>	// Holy shit, this actually works!
	{
		IEnumerable<IGraphEdge<GraphNode>> OutgoingEdges {get;}
	}

	public static class GraphNodeExtensions {
		public static IEnumerable<GraphNode> GetNeighbours<GraphNode>(
			this IGraphNode<GraphNode> node
		) where GraphNode : IGraphNode<GraphNode>
		{
			var neighbours = new List<GraphNode>();

			neighbours.AddRange(
				(from edge in node.OutgoingEdges select edge.TargetNode).Distinct()
			);

			return neighbours;
		}
	}
}


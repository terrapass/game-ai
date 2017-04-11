using System;
using System.Collections.Generic;

namespace Terrapass.GameAi.Goap.Graphs
{
	/// <summary>
	/// This interface describes classes, capable of finding paths on generalized graphs.
	/// </summary>
	public interface IPathfinder<GraphNode> where GraphNode : IGraphNode<GraphNode>
	{
		// TODO: Describe the motivation behind the use of 2 potentially different equality comparers in <remarks>.
		/// <summary>
		/// Finds a path between the given source and target nodes on a graph.
		/// </summary>
		/// <returns>The path.</returns>
		/// <param name="sourceNode">Source node.</param>
		/// <param name="targetNode">Target node.</param>
		/// <param name="targetEqualityComparer">
		/// The pathfinder will use this comparer, instead of <code>nodeEqualityComparer</code>,
		/// to test whether the node it currently examines is the target one.
		/// </param>
		/// <param name="nodeEqualityComparer">
		/// This comparer will be used by the pathfinder to distinguish between the nodes,
		/// except when testing whether the currently examined node is the target.
		/// </param>
		/// <exception cref="T:PathNotFoundException">
		/// When pathfinder fails to find a path between the given nodes.
		/// </exception>
		/// <remarks>
		/// TODO
		/// </remarks>
		Path<GraphNode> FindPath(
			GraphNode sourceNode,
			GraphNode targetNode,
			IEqualityComparer<GraphNode> targetEqualityComparer,
			IEqualityComparer<GraphNode> nodeEqualityComparer
		);
	}

	public static class PathfinderExtensions
	{
		public static Path<GraphNode> FindPath<GraphNode>(
			this IPathfinder<GraphNode> pathfinder,
			GraphNode sourceNode,
			GraphNode targetNode,
			IEqualityComparer<GraphNode> targetEqualityComparer
		) where GraphNode : IGraphNode<GraphNode>
		{
			return pathfinder.FindPath(
				sourceNode,
				targetNode,
				targetEqualityComparer,
				EqualityComparer<GraphNode>.Default
			);
		}

		public static Path<GraphNode> FindPath<GraphNode>(
			this IPathfinder<GraphNode> pathfinder,
			GraphNode sourceNode,
			GraphNode targetNode
		) where GraphNode : IGraphNode<GraphNode>
		{
			return pathfinder.FindPath(
				sourceNode,
				targetNode,
				EqualityComparer<GraphNode>.Default
			);
		}
	}
}


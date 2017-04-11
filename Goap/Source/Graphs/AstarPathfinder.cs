// Uncomment to make AstarPathfinder make tests for correctness of GraphNode.GetHashCode()
//#define ASTAR_VALIDATE_NODE_HASH_IMPLEMENTATION
using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;
using Terrapass.GameAi.Goap.Time;

using Terrapass.GameAi.Goap.Utils.Collections;

namespace Terrapass.GameAi.Goap.Graphs
{
	/// <summary>
	/// This pathfinder uses A* algorithm to find paths on a graph.
	/// </summary>
	public class AstarPathfinder<GraphNode> : IPathfinder<GraphNode> where GraphNode : IGraphNode<GraphNode>
	{
		/// <summary>
		/// Partial path with estimated cost, comparable by estimated cost.
		/// </summary>
		private class PartialPath : IComparable<PartialPath>
		{
			private IList<IGraphEdge<GraphNode>> edges;
			//private double? costSoFar;

			public double EstimatedTotalCost { get; private set; }

			public PartialPath(double estimatedTotalCost)
			{
				this.edges = new List<IGraphEdge<GraphNode>>();
				this.EstimatedTotalCost = estimatedTotalCost;
				//this.costSoFar = null;
			}

			public PartialPath(PartialPath other)
			{
				this.edges = new List<IGraphEdge<GraphNode>>(other.edges);
				this.EstimatedTotalCost = other.EstimatedTotalCost;
				//this.costSoFar = other.costSoFar;
			}

			public bool IsEmpty
			{
				get {
					return this.edges.Count == 0;
				}
			}

			public int EdgeCount
			{
				get {
					return this.edges.Count;
				}
			}

			public GraphNode LastNode
			{
				get {
					var lastEdge = this.edges.LastOrDefault();
					if(lastEdge == null)
					{
						// Throwing an exception, instead of returning null,
						// to avoid the need to add a new() constraint on GraphNode,
						// thus disallowing the use of structs as GraphNodes
						// (structs and other value types cannot be null in C#).
						throw new InvalidOperationException();
					}
					return lastEdge.TargetNode;
				}
			}

			public double CostSoFar
			{
				get {
//					if(this.costSoFar == null || !this.costSoFar.HasValue)
//					{
//						this.costSoFar = new Nullable<double>(this.edges.Sum(edge => edge.Cost));
//					}
//					return this.costSoFar.Value;
					return this.edges.Sum(edge => edge.Cost);
				}
			}

			public void AppendEdge(IGraphEdge<GraphNode> edge, double estimatedRemainingCost)
			{
				this.edges.Add(edge);
				//this.costSoFar = null;
				this.EstimatedTotalCost = this.CostSoFar + edge.Cost + estimatedRemainingCost;
			}

			public Path<GraphNode> ToPath()
			{
				return new Path<GraphNode>(this.edges);
			}

			#region IComparable implementation

			public int CompareTo(PartialPath other)
			{
				return this.EstimatedTotalCost.CompareTo(other.EstimatedTotalCost);
			}

			#endregion
		}

		private readonly PathCostHeuristic<GraphNode> heuristic;
		private readonly int maxSearchDepth;
		private readonly float maxSecondsPerSearch;
		//private readonly bool assumeNonNegativeCosts;

		public AstarPathfinder(AstarPathfinderConfiguration<GraphNode> configuration = null)
		{
			// If no configuration is provided, use default.
			if(configuration == null)
			{
				configuration = new AstarPathfinderConfiguration<GraphNode>.Builder().Build();
			}

			this.heuristic = configuration.Heuristic;
			this.maxSearchDepth = configuration.MaxSearchDepth;
			this.maxSecondsPerSearch = configuration.MaxSecondsPerSearch;
			//this.assumeNonNegativeCosts = configuration.AssumeNonNegativeCosts;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Terrapass.GameAi.Goap.Graphs.AstarPathfinder`1"/> class.
		/// </summary>
		/// <param name="heuristic">Heuristic to be used.</param>
		/// <param name="maxSearchDepth">
		/// Maximum depth for search, i.e. maximum number of edges, allowed in a path.
		/// A negative value indicates no depth limit.
		/// </param>
		public AstarPathfinder(
			PathCostHeuristic<GraphNode> heuristic, 
			int maxSearchDepth = AstarPathfinderConfiguration<GraphNode>.UNLIMITED_SEARCH_DEPTH
		) : this(new AstarPathfinderConfiguration<GraphNode>.Builder()
				.UseHeuristic(heuristic)
				.LimitSearchDepth(maxSearchDepth)
				.Build())
		{

		}

		#region IPathfinder implementation
		/// <inheritdoc />
		public Path<GraphNode> FindPath(
			GraphNode sourceNode,
			GraphNode targetNode,
			IEqualityComparer<GraphNode> targetEqualityComparer,
			IEqualityComparer<GraphNode> nodeEqualityComparer
		)
		{
			// If there is a limit on search time, we need to initialize the timer
			var timer = new StopwatchExecutionTimer();

			// Set of already visited nodes
			var closed = new HashSet<GraphNode>(nodeEqualityComparer);
			// Priority queue of partial and potentially complete paths
			var open = new ListBasedPriorityQueue<PartialPath>();
			open.Add(new PartialPath(this.heuristic(sourceNode, targetNode)));

			// Best known path to target in the open priority queue
			// (Used as a fallback, if time limit is exceeded before the search ends)
			PartialPath bestPathToTarget = null;
			// Cost of the cheapest known path to target
			// (Used to discard costlier paths, if the assumption of non-negative edge costs is in effect)
//			double minTotalCost = double.PositiveInfinity;

			// While there are unexplored nodes
			while(open.Count > 0)
			{
				// Pop partial path with the lowest estimated cost from the priority queue
				var currentPartialPath = open.PopFront();
				// Node to explore
				GraphNode currentNode;

				// If partial path is empty (and therefore has no last node to speak of),
				// it means we are at the source of our search.
				if(currentPartialPath.IsEmpty)
				{
					// Explore the source node
					currentNode = sourceNode;
				}
				else
				{
					// Explore the last node of the current partial path
					currentNode = currentPartialPath.LastNode;
				}

				// If the currently examined node has been examined previously, don't consider it further.
				// This has the effect of discarding the partial path, which led to it, from the open priority queue.
				// FIXME: As far as I understand, this is correct behaviour only if the heuristic is admissable! 
				// In general case (i.e. with inadmissable heuristics), closed nodes may need to be reopened.
				// To control this behaviour an additional flag may be added to the constructor and checked here.
				if(closed.Contains(currentNode))
				{
					continue;
				}
				// Will only be called if ASTAR_VALIDATE_NODE_HASH_IMPLEMENTATION is defined
				ValidateNodeHashImplementation(closed, currentNode);

				// If the currently examined node is the target
				if(targetEqualityComparer.Equals(currentNode, targetNode))
				{
					// Hurray!
					return currentPartialPath.ToPath();
				}

				// If there is a time limit, check the timer.
				// If the time limit has been exceeded, return the best known path to target or,
				// if there has been no such path discovered, break and report a failure.
				if(this.maxSecondsPerSearch < float.PositiveInfinity && timer.ElapsedSeconds > this.maxSecondsPerSearch)
				{
					if(bestPathToTarget != null)
					{
						// "Good enough"
						return bestPathToTarget.ToPath();
					}
					else
					{
						throw new PathfindingTimeoutException(this.GetType(), this.maxSecondsPerSearch);
					}
				}

				// If we have a limit on max search depth, and current path is at max depth limit, 
				// don't add paths to currentNode's neighbours to the open priority queue, 
				// since those paths will exceed the max search depth, and don't place currentNode
				// into closed set, since there might be a more expensive path with smaller edge count,
				// which leads to it without exceeding the max seatch depth limit.
				if(this.maxSearchDepth >= 0 && currentPartialPath.EdgeCount >= this.maxSearchDepth)
				{
					continue;
				}

				// FIXME: Remove the commented out code.
				// The following check makes no sense: priority queue already guarantees that
				// the current path is at most as expensive as the best known path to target.
				// Maybe if it checked for >=, instead of >, i.e. assumed positive and not merely non-negative costs,
				// it would have miniscule effect, but even so the benefits would likely be negligible.

				// If the assumption of non-negative costs is in effect, we can discard the current path
				// from further consideration, if it's already costlier than the cheapest known path 
				// that reaches the target node, because if all the edges have non-negative cost,
				// we can be certain that it will not get any cheaper.
				// (At this point we know that the current path does not reach the target,
				// otherwise we would've already returned it earlier in this loop iteration.)
//				if(this.assumeNonNegativeCosts && currentPartialPath.CostSoFar > minTotalCost)
//				{
//					continue;
//				}

				// Mark currentNode as visited by placing it into closed set.
				closed.Add(currentNode);
				// Insert paths from sourceNode to currentNode's neighbours into the open priority queue
				foreach(var outgoingEdge in currentNode.OutgoingEdges)
				{
					var neighbour = outgoingEdge.TargetNode;
					var pathToNeighbour = new PartialPath(currentPartialPath);
					pathToNeighbour.AppendEdge(outgoingEdge, this.heuristic(neighbour, targetNode));
					open.Add(pathToNeighbour);

					// If there is a time limit, check if the neighbour is the target node
					// and update bestPathToTarget, if needed.
					// This allows us to keep track of the best path to target, found so far.
					if(this.maxSecondsPerSearch < float.PositiveInfinity 
						&& targetEqualityComparer.Equals(neighbour, targetNode)
						// Theoretically, EstimatedTotalCost could be used here, instead of CostSoFar,
						// since heuristic SHOULD return 0 for target node.
						// However, this behaviour is not enforced, and so CostSoFar is used for reliability.
						&& (bestPathToTarget == null || pathToNeighbour.CostSoFar < bestPathToTarget.CostSoFar))
					{
						bestPathToTarget = pathToNeighbour;
					}

					// If the assumption of non-negative costs is in effect, check if the neighbour
					// is the target node and update minTotalCost, if needed.
//					if(this.assumeNonNegativeCosts && targetEqualityComparer.Equals(neighbour, targetNode))
//					{
//						// Theoretically, EstimatedTotalCost could be used here, instead of CostSoFar,
//						// since heuristic SHOULD return 0 for target node.
//						// However, this behaviour is not enforced, and so CostSoFar is used for reliability.
//						minTotalCost = Math.Min(minTotalCost, pathToNeighbour.CostSoFar);
//					}
				}
			}

			// Found no path to targetNode
			throw new NoPathExistsException(
				this.GetType(), 
				this.maxSearchDepth != AstarPathfinderConfiguration<GraphNode>.UNLIMITED_SEARCH_DEPTH
					? (int?)this.maxSearchDepth
					: (int?)null
			);

		}
		#endregion

		[System.Diagnostics.Conditional("ASTAR_VALIDATE_NODE_HASH_IMPLEMENTATION")]
		private static void ValidateNodeHashImplementation(HashSet<GraphNode> closedSet, GraphNode currentNode)
		{
			var debugEqualClosed = closedSet.FirstOrDefault((node) => node.Equals(currentNode));
			DebugUtils.Assert(
				debugEqualClosed == null,
				"{0}.GetHashCode() is broken: hashes are different for equal values: ({0}(HASH {1}) == {2}(HASH {3})", 
				currentNode, 
				currentNode.GetHashCode(),
				debugEqualClosed,
				debugEqualClosed != null ? debugEqualClosed.GetHashCode() : -1
			);
		}
	}
}


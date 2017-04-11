using System;
using System.Collections.Generic;
using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Graphs
{
	// TODO: Figure out a way to limit A* search by time and make it return the best found
	// (but not necessarily the best overall) path, if search time limit is exceeded. Maybe use IDA*?
	public class AstarPathfinderConfiguration<GraphNode> where GraphNode : IGraphNode<GraphNode>
	{
		public const int UNLIMITED_SEARCH_DEPTH = -1;
		public const float UNLIMITED_SECONDS_PER_SEARCH = float.PositiveInfinity;
		//public const bool DEFAULT_ASSUME_NON_NEGATIVE_COSTS = false;

		/// <summary>
		/// Heuristic, which returns 0 for any two nodes.
		/// Using this heuristic for A* makes it equivalent to Dijkstra's algorithm.
		/// </summary>
		/// <returns>Heuristic path cost - always zero.</returns>
		/// <param name="sourceNode">Source node.</param>
		/// <param name="targetNode">Target node.</param>
		public static double ZeroPathCostHeuristic(GraphNode sourceNode, GraphNode targetNode)
		{
			return 0;
		}

		public PathCostHeuristic<GraphNode> Heuristic { get; }
		public int MaxSearchDepth { get; }
		public float MaxSecondsPerSearch { get; }
		//public bool AssumeNonNegativeCosts { get; }

		private AstarPathfinderConfiguration(
			PathCostHeuristic<GraphNode> heuristic,
			int maxSearchDepth,
			float maxSecondsPerSearch//,
			//bool assumeNonNegativeCosts
		)
		{
			this.Heuristic = PreconditionUtils.EnsureNotNull(heuristic, "heuristic must not be null");
			this.MaxSearchDepth = maxSearchDepth;
			this.MaxSecondsPerSearch = maxSecondsPerSearch;
			//this.AssumeNonNegativeCosts = assumeNonNegativeCosts;
		}

		public class Builder
		{
			private PathCostHeuristic<GraphNode> heuristic;
			private int maxSearchDepth;
			private float maxSecondsPerSearch;
			//private bool assumeNonNegativeCosts;

			/// <summary>
			/// Initializes a new instance of the <see cref="Terrapass.GameAi.Goap.Graphs.AstarPathfinderConfiguration`1+Builder"/>
			/// with default configuration (zero heuristic, no limit on search depth, no assumptions).
			/// </summary>
			public Builder()
			{
				this.heuristic = ZeroPathCostHeuristic;
				this.maxSearchDepth = UNLIMITED_SEARCH_DEPTH;
				this.maxSecondsPerSearch = UNLIMITED_SECONDS_PER_SEARCH;
				//this.assumeNonNegativeCosts = DEFAULT_ASSUME_NON_NEGATIVE_COSTS;
			}

			/// <summary>
			/// Specifies the heuristic to be used by A*.
			/// </summary>
			/// <returns>Builder instance.</returns>
			/// <param name="heuristic">Heuristic to be used.</param>
			public Builder UseHeuristic(PathCostHeuristic<GraphNode> heuristic)
			{
				this.heuristic = PreconditionUtils.EnsureNotNull(heuristic, "heuristic");
				return this;
			}

			/// <summary>
			/// Limits search depth.
			/// </summary>
			/// <returns>Builder instance.</returns>
			/// <param name="maxSearchDepth">
			/// Maximum depth for search, i.e. maximum number of edges, allowed in a path.
			/// A negative value indicates no depth limit.
			/// </param>
			public Builder LimitSearchDepth(int maxSearchDepth)
			{
				this.maxSearchDepth = maxSearchDepth;
				return this;
			}

			/// <summary>
			/// Limits search time.
			/// This is a soft time limit: pathfinder will stop the search only
			/// when it discovers that the time limit has already been exceeded.
			/// At that point, it will return the best known (but not necessarily the best overall) 
			/// path to target or, if no paths to target are found by that time, report a failure.
			/// </summary>
			/// <returns>Builder instance.</returns>
			/// <param name="seconds">Max seconds per path search.</param>
			public Builder LimitSearchTime(float seconds)
			{
				this.maxSecondsPerSearch = seconds;
				return this;
			}

			/// <summary>
			/// Limits search time.
			/// This is a soft time limit: pathfinder will stop the search only
			/// when it discovers that the time limit has already been exceeded.
			/// At that point, it will return the best known (but not necessarily the best overall) 
			/// path to target or, if no paths to target are found by that time, report a failure.
			/// </summary>
			/// <returns>Builder instance.</returns>
			/// <param name="seconds">Max seconds per path search.</param>
			public Builder LimitSearchTime(double seconds)
			{
				this.maxSecondsPerSearch = (float)seconds;
				return this;
			}

			/// <summary>
			/// Allows A* to assume that all edges have non-negative costs.
			/// This allows it to discard certain potential paths quicker,
			/// thus improving performance, but makes it return suboptimal paths,
			/// if in fact there ARE edges with negative cost.
			/// </summary>
			/// <returns>Builder instance.</returns>
//			public Builder AssumeNonNegativeCosts()
//			{
//				this.assumeNonNegativeCosts = true;
//				return this;
//			}

			public AstarPathfinderConfiguration<GraphNode> Build()
			{
				return new AstarPathfinderConfiguration<GraphNode>(
					this.heuristic,
					this.maxSearchDepth,
					this.maxSecondsPerSearch//,
					//this.assumeNonNegativeCosts
				);
			}
		}
	}
}
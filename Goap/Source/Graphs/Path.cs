using System;
using System.Linq;
using System.Collections.Generic;

namespace Terrapass.GameAi.Goap.Graphs
{
	/// <summary>
	/// Path between two nodes of an abstract graph.
	/// </summary>
	public sealed class Path<GraphNode> where GraphNode : IGraphNode<GraphNode>
	{
		/// <summary>
		/// Total path cost, i.e. sum of costs of all of the edges.
		/// </summary>
		/// <value>Path cost</value>
		public double Cost {get;}

		/// <summary>
		/// Ordered collection of edges, which form the path.
		/// </summary>
		/// <value>Path edges.</value>
		public IEnumerable<IGraphEdge<GraphNode>> Edges {get;}

		public Path(IEnumerable<IGraphEdge<GraphNode>> edges)
		{
			this.Edges = new List<IGraphEdge<GraphNode>>(edges);
			this.Cost = edges.Sum(edge => edge.Cost);
		}
	}
}


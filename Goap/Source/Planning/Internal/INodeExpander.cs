using System;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Graphs;

namespace Terrapass.GameAi.Goap.Planning.Internal
{
	internal interface INodeExpander<Node> where Node : IGraphNode<Node>
	{
		IEnumerable<IGraphEdge<Node>> ExpandNode(Node node);
	}
}


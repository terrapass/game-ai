using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Graphs;

namespace Terrapass.GameAi.Goap.Planning.Internal
{
	internal sealed class ForwardNode : IGraphNode<ForwardNode>, IEquatable<ForwardNode>
	{
		private readonly WorldState worldState;
		private readonly INodeExpander<ForwardNode> nodeExpander;

		private readonly Goal goal;

		private IEnumerable<IGraphEdge<ForwardNode>> outgoingEdges;

		public bool IsGoal { get; }

		public WorldState WorldState
		{
			get {
				if(this.IsGoal)
				{
					throw new InvalidOperationException(
						string.Format(
							"WordState property cannot be retrieved from a goal {0}",
							this.GetType()
						)
					);
				}
				return this.worldState;
			}
		}

		public Goal Goal
		{ 
			get {
				if(!this.IsGoal)
				{
					throw new InvalidOperationException(
						string.Format(
							"Goal property can only be retrieved from a goal {0}",
							this.GetType()
						)
					);
				}
				return this.goal;
			}
		}

		// Preconditions are checked in static factory methods Make*()
		private ForwardNode(WorldState worldState, INodeExpander<ForwardNode> nodeExpander, Goal goal, bool isGoal)
		{
			DebugUtils.Assert(
				isGoal || nodeExpander != null,
				"nodeExpander must not be null for regular (non-goal) nodes"
			);

			this.worldState = worldState;
			this.nodeExpander = nodeExpander;
			this.goal = goal;
			this.IsGoal = isGoal;

			this.outgoingEdges = null;
		}

		public static ForwardNode MakeRegularNode(WorldState worldState, INodeExpander<ForwardNode> nodeExpander)
		{
			return new ForwardNode(
				PreconditionUtils.EnsureNotNull(worldState, "worldState"), 
				PreconditionUtils.EnsureNotNull(nodeExpander, "nodeExpander"),
				default(Goal),
				false
			);
		}

		public static ForwardNode MakeGoalNode(Goal goal)
		{
			return new ForwardNode(default(WorldState), null, goal, true);
		}

		#region IGraphNode implementation

		public IEnumerable<IGraphEdge<ForwardNode>> OutgoingEdges
		{
			get {
				if(this.IsGoal)
				{
					throw new InvalidOperationException(
						string.Format(
							"OutgoingEdges property cannot be retrieved from a goal {0}",
							this.GetType()
						)
					);
				}

				if(this.outgoingEdges == null)
				{
					this.outgoingEdges = this.nodeExpander.ExpandNode(this);
				}

				DebugUtils.Assert(
					outgoingEdges.All(edge => edge.SourceNode == this),
					"Node expander must make this node the source of every outgoing edge"
				);

				return this.outgoingEdges;
			}
		}

		#endregion


		#region IEquatable implementation
		public bool Equals(ForwardNode other)
		{
			if(this.IsGoal && other.IsGoal)
			{
				throw new InvalidOperationException(
					string.Format(
						"Equals() cannot be used to compare two goal instances of {0}",
						this.GetType()
					)
				);
			}

			if(!this.IsGoal && !other.IsGoal)
			{
				return this.WorldState.Equals(other.WorldState);
			}

			// If one is a goal node, and another is a regular node
			var regularNode = this.IsGoal ? other : this;
			var goalNode = this.IsGoal ? this : other;

			return goalNode.Goal.IsReachedIn(regularNode.WorldState);
		}
		#endregion

		public override bool Equals(object obj)
		{
			if(obj == null)
				return false;
			if(ReferenceEquals(this, obj))
				return true;
			if(obj.GetType() != typeof(ForwardNode))
				return false;
			ForwardNode other = (ForwardNode)obj;
			return this.Equals(other);
		}
		

		public override int GetHashCode()
		{
			unchecked
			{
				return worldState.GetHashCode();
			}
		}
		
	}
}


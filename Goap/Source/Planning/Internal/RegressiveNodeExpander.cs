using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Graphs;
using Terrapass.GameAi.Goap.Planning.Preconditions;
using Terrapass.GameAi.Goap.Planning.Effects;

namespace Terrapass.GameAi.Goap.Planning.Internal
{
	internal class RegressiveNodeExpander : INodeExpander<RegressiveNode>
	{
		private readonly IEnumerable<PlanningAction> availableActions;

		public RegressiveNodeExpander(IEnumerable<PlanningAction> availableActions)
		{
			this.availableActions = PreconditionUtils.EnsureNotNull(availableActions, "availableActions");
		}

		#region INodeExpander implementation
		public IEnumerable<IGraphEdge<RegressiveNode>> ExpandNode(RegressiveNode node)
		{
			if(PreconditionUtils.EnsureNotNull(node, "node").IsTarget)
			{
				throw new InvalidOperationException(
					string.Format(
						"{0} cannot expand a target {1}",
						this.GetType(),
						node.GetType()
					)
				);
			}

			var result = new List<IGraphEdge<RegressiveNode>>();
			foreach(var action in availableActions)
			{
				bool incompatible = false;	// Whether any effect contradicts existing constraints
				// TODO: Ensure this is calculated properly.
				bool useful = false;		// Whether any effect affects existing constraints
				bool deadend = false;		// Whether any resulting constraint is unsatisfiable

				var resultingConstraintsBuilder = node.CurrentConstraints.BuildUpon();
				foreach(var effect in action.Effects)
				{
					if(effect.ValueAssigned != null)
					{
						if(!node.CurrentConstraints[effect.SymbolId].Contains(effect.ValueAssigned.Value))
						{
							// (At least) one of this action's effects contradicts the current constraints,
							// so this action cannot immediately precede the current node.
							incompatible = true;
							break;
						}
						else
						{
							// If updating the symbol value matters
							if(!node.CurrentConstraints[effect.SymbolId].Equals(ValueRange.AnyValue))
							{
								// Since the effect resets symbol to a new value,
								// its value before action is not important
								// (unless there is a corresponding precondition on the action itself,
								// but this is checked later)
								resultingConstraintsBuilder.SetRange(effect.SymbolId, ValueRange.AnyValue);
								useful = true;
							}
						}
					}
					else if(effect.ValueDelta != null)
					{
						// Shift constraints to account for this effect's contribution into symbol value.
						resultingConstraintsBuilder.SetRange(
							effect.SymbolId, node.CurrentConstraints[effect.SymbolId].ShiftBy(-effect.ValueDelta.Value)
						);
						//useful = true;
						//useful = (useful || resultingConstraintsBuilder[effect.SymbolId].Size > node.CurrentConstraints[effect.SymbolId].Size);
						// The action is useful if at least one of its effects 
						// shifts at least one of existing constraints
						// closer to the initial state value for corresponding symbol.
						useful = (
							useful 
							|| 
							resultingConstraintsBuilder[effect.SymbolId].AbsDistanceFrom(node.InitialState[effect.SymbolId]) 
								< 
							node.CurrentConstraints[effect.SymbolId].AbsDistanceFrom(node.InitialState[effect.SymbolId])
						);
					}
					else
					{
						throw new InvalidOperationException(
							string.Format(
								"Effect {0} cannot be used with RegressivePlanner: either ValueAssigned or ValueDelta must return non-null",
								effect.GetType()
							)
						);
					}
				}

				if(incompatible)
				{
					continue;
				}

				foreach(var precond in action.Preconditions)
				{
					resultingConstraintsBuilder.IntersectRange(precond.SymbolId, precond.AsValueRange);
					// If we've arrived at an unsatisfiable constraint
					if(resultingConstraintsBuilder[precond.SymbolId].IsEmpty)
					{
						deadend = true;
						break;
					}
				}

				if(!incompatible && useful && !deadend)
				{
					result.Add(
						new RegressiveEdge(
							action,
							node,
							RegressiveNode.MakeRegular(
								resultingConstraintsBuilder.Build(),
								node.InitialState,
								this
							)
						)
					);
				}
			}

			return result;
		}
		#endregion
	}
}


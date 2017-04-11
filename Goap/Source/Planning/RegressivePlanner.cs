using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Graphs;
using Terrapass.GameAi.Goap.Planning.Preconditions;

namespace Terrapass.GameAi.Goap.Planning
{
	using Internal;

	public class RegressivePlanner : IPlanner
	{
		public const int DEFAULT_MAX_PLAN_LENGTH = 20;
		public const float DEFAULT_SEARCH_TIME_SECONDS = 0.5f;

		private readonly int maxPlanLength;
		private readonly IPathfinder<RegressiveNode> pathfinder;

		public RegressivePlanner(int maxPlanLength = DEFAULT_MAX_PLAN_LENGTH, float searchTimeSeconds = DEFAULT_SEARCH_TIME_SECONDS)
		{
			if(maxPlanLength <= 0)
			{
				throw new ArgumentException("maxPlanLength must be positive", "maxPlanLength");
			}
			this.maxPlanLength = maxPlanLength;
			this.pathfinder = new AstarPathfinder<RegressiveNode>(
				new AstarPathfinderConfiguration<RegressiveNode>.Builder()
					.UseHeuristic(PathfindingHeuristic)
					.LimitSearchDepth(maxPlanLength)
					.LimitSearchTime(searchTimeSeconds)
					.Build()
			);
		}

		#region IPlanner implementation

		public Plan FormulatePlan(
			IKnowledgeProvider knowledgeProvider,
			IEnumerable<PlanningAction> availableActions, 
			Goal goal
		)
		{
			// TODO: Ideally, world state should be populated lazily, not in advance, like here.
			// Some symbols may not be needed at all to build a plan!
			// (also see comments on IPrecondition and IEffect RelevantActions property)
			var initialWorldState = new RelevantSymbolsPopulator(availableActions, goal)
				.PopulateWorldState(knowledgeProvider);

			// TODO: After lazy population is implemented, the following part of the method is the only thing
			// that must remain.
			var goalConstraints = new RegressiveStatePopulator().Populate(goal);
			try
			{
				var path = this.pathfinder.FindPath(
					RegressiveNode.MakeRegular(
						goalConstraints,
						initialWorldState,
						new RegressiveNodeExpander(availableActions)
					),
					RegressiveNode.MakeTarget(initialWorldState)
				);

//				UnityEngine.Debug.Log("Regressive path:");
//				for(int i = 0; i < path.Edges.Count(); i++)
//				{
//					if(i == 0)
//					{
//						UnityEngine.Debug.LogFormat("Initial: {0}", path.Edges.ElementAt(i).SourceNode);
//					}
//					UnityEngine.Debug.LogFormat("After {0}: {1}", ((RegressiveEdge)path.Edges.ElementAt(i)).Action.Name, path.Edges.ElementAt(i).TargetNode);
//
//					if(i > 0)
//					{
//						DebugUtils.Assert(path.Edges.ElementAt(i - 1).TargetNode == path.Edges.ElementAt(i).SourceNode, "path must be consistent");
//					}
//				}

				// FIXME: Downcasting to RegressiveNode - may be fixed by adding another generic parameter to IPathfinder.
				//return new Plan(SortActions(from edge in path.Edges.Reverse() select ((RegressiveEdge)edge).Action, initialWorldState));
				return new Plan(from edge in path.Edges.Reverse() select ((RegressiveEdge)edge).Action, goal);
			}
			catch(PathNotFoundException e)
			{
				throw new PlanNotFoundException(this, maxPlanLength, goal, e);
			}
		}

		#endregion

		private static double PathfindingHeuristic(RegressiveNode sourceNode, RegressiveNode targetNode)
		{
			DebugUtils.Assert(!sourceNode.IsTarget, "sourceNode must not be a target {0}", typeof(RegressiveNode));
			DebugUtils.Assert(targetNode.IsTarget, "targetNode must be a target {0}", typeof(RegressiveNode));

			// Heuristic value is the sum of distances between
			// constraint ranges and corresponding values in the initial state.
			return sourceNode.CurrentConstraints
				.Sum((kvp) => kvp.Value.AbsDistanceFrom(targetNode.InitialState[kvp.Key]));
//			var result = sourceNode.CurrentConstraints
//				.Sum((kvp) => kvp.Value.AbsDistanceFrom(targetNode.InitialState[kvp.Key]));
//			UnityEngine.Debug.LogFormat(
//				"h = {0} for node w/ {1} constraints. ({2}) and initial state {3} ", 
//				result, 
//				sourceNode.CurrentConstraints.Count(), 
//				sourceNode.CurrentConstraints,
//				targetNode.InitialState
//			);
//			return result;
		}
	}
}


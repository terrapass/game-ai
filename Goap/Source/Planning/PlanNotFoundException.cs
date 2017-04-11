using System;

namespace Terrapass.GameAi.Goap.Planning
{
	
	[Serializable]
	public class PlanNotFoundException : Exception
	{
		private const string MESSAGE_TEMPLATE = "{0} failed to find a plan, containing at most {1} actions, to satisfy the goal \"{2}\"{3}";
		private const string DETAILS_TEMPLATE = ": {0}";

		public IPlanner Planner { get;}
		public int MaxPlanLength { get;}
		public Goal Goal { get;}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PlanNotFoundException"/> class
		/// </summary>
		public PlanNotFoundException(IPlanner planner, int maxPlanLength, Goal goal, Exception inner = null)
			: this(planner, maxPlanLength, goal, null, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PlanNotFoundException"/> class
		/// </summary>
		/// <param name="planner">The planner, which failed. </param>
		/// <param name="maxPlanLength">Max plan length setting of the planner. </param>
		/// <param name="goal">Goal.</param>
		/// <param name="details">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public PlanNotFoundException(IPlanner planner, int maxPlanLength, Goal goal, string details, Exception inner = null)
			: base(
				string.Format(
					MESSAGE_TEMPLATE,
					planner.GetType(),
					maxPlanLength,
					goal.Name,
					details != null ? string.Format(DETAILS_TEMPLATE, details) : ""
				),
				inner
			)
		{
			this.Planner = planner;
			this.MaxPlanLength = maxPlanLength;
			this.Goal = goal;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PlanNotFoundException"/> class
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected PlanNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) 
			: base(info, context)
		{
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Planning;

namespace Terrapass.GameAi.Goap.Agents
{
	public class SimpleActionFactory : IActionFactory
	{
		private readonly IDictionary<string, Func<IAction>> actionFactories;
		private readonly IEnumerable<PlanningAction> supportedPlanningActions;

		public SimpleActionFactory(
			IDictionary<PlanningAction, Func<IAction>> actionFactories
		)
		{
			PreconditionUtils.EnsureNotNull(actionFactories, "actionFactories");

			this.actionFactories = actionFactories.ToDictionary((kvp) => kvp.Key.Name, (kvp) => kvp.Value);
			this.supportedPlanningActions = actionFactories.Keys;
		}

		#region IActionFactory implementation
		public IAction FromPlanningAction(string planningActionName)
		{
			return actionFactories[planningActionName]();
		}

		public IEnumerable<PlanningAction> SupportedPlanningActions
		{
			get {
				return this.supportedPlanningActions;
			}
		}
		#endregion
	}
}


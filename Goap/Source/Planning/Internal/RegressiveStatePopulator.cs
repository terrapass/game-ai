using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Planning.Preconditions;

namespace Terrapass.GameAi.Goap.Planning.Internal
{
	internal class RegressiveStatePopulator : IRegressiveStatePopulator
	{
		#region IRegressiveStatePopulator implementation
		public RegressiveState Populate(Goal goal, RegressiveState initialState = default(RegressiveState))
		{
			var builder = initialState.BuildUpon();
			foreach(var precondition in goal.Preconditions)
			{
				// IntersectRange() has the effect of merging preconditions on the same symbol
				builder.IntersectRange(precondition.SymbolId, precondition.AsValueRange);
			}
			return builder.Build();
		}
		#endregion
		
	}
}


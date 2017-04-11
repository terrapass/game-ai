using System;

namespace Terrapass.GameAi.Goap.Planning.Internal
{
	internal interface IRegressiveStatePopulator
	{
		RegressiveState Populate(Goal goal, RegressiveState initialState = default(RegressiveState));
	}
}


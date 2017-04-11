using System;

namespace Terrapass.GameAi.Goap.Graphs
{
	
	[Serializable]
	public class PathfindingTimeoutException : PathNotFoundException
	{
		private const string DEFAULT_MESSAGE = "time limit exceeded ({0}s)";

		public float TimeLimitSeconds { get;}

		public PathfindingTimeoutException(Type pathfinderType, float timeLimitSeconds)
			: this(pathfinderType, timeLimitSeconds, null)
		{
		}

		public PathfindingTimeoutException(Type pathfinderType, float timeLimitSeconds, Exception inner)
			: base(
				pathfinderType, 
				string.Format(
					DEFAULT_MESSAGE, 
					timeLimitSeconds
				), 
				inner
			)
		{
			this.TimeLimitSeconds = timeLimitSeconds;
		}

		protected PathfindingTimeoutException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}


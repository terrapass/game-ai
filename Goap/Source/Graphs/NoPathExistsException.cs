using System;

namespace Terrapass.GameAi.Goap.Graphs
{
	
	[Serializable]
	public class NoPathExistsException : PathNotFoundException
	{
		private const string DEFAULT_MESSAGE = "no path exists{0}";
		private const string DETAILS_TEMPLATE = " (given max search depth {0})";

		public int? MaxSearchDepth { get;}

		public NoPathExistsException(Type pathfinderType)
			: this(pathfinderType, null, null)
		{
		}

		public NoPathExistsException(Type pathfinderType, int? maxSearchDepth)
			: this(pathfinderType, maxSearchDepth, null)
		{
		}

		public NoPathExistsException(Type pathfinderType, Exception inner)
			: this(pathfinderType, null, inner)
		{
		}

		public NoPathExistsException(Type pathfinderType, int? maxSearchDepth, Exception inner)
			: base(
				pathfinderType, 
				string.Format(
					DEFAULT_MESSAGE, 
					maxSearchDepth == null 
					? string.Format(DETAILS_TEMPLATE, maxSearchDepth)
					: ""
				), 
				inner
			)
		{
			this.MaxSearchDepth = maxSearchDepth;
		}

		protected NoPathExistsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}


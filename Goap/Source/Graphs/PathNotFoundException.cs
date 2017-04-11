using System;

namespace Terrapass.GameAi.Goap.Graphs
{
	/// <summary>
	/// Thrown by IPathfinder implementers, when they fail to find a path on a graph.
	/// </summary>
	[Serializable]
	public class PathNotFoundException : Exception
	{
		private const string DEFAULT_MESSAGE = "{0} failed to find a path: {1}";
		private const string DEFAULT_DETAILS = "unknown reason";

		public Type PathfinderType { get;}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PathNotFoundException"/> class
		/// </summary>
		/// <param name="pathfinderType">Type of the pathfinder, which failed to find a path.</param>
		public PathNotFoundException(Type pathfinderType) : this(pathfinderType, DEFAULT_DETAILS)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Terrapass.GameAi.Goap.Graphs.PathNotFoundException"/> class.
		/// </summary>
		/// <param name="pathfinderType">Type of the pathfinder, which failed to find a path.</param>
		/// <param name="inner">The exception that is the cause of the current exception.</param>
		public PathNotFoundException(Type pathfinderType, Exception inner)
			: this(pathfinderType, inner.Message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PathNotFoundException"/> class
		/// </summary>
		/// <param name="pathfinderType">Type of the pathfinder, which failed to find a path.</param>
		/// <param name="details">A <see cref="T:System.String"/> that describes the exception. </param>
		public PathNotFoundException(Type pathfinderType, string details)
			: this(pathfinderType, details, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PathNotFoundException"/> class
		/// </summary>
		/// <param name="pathfinderType">Type of the pathfinder, which failed to find a path.</param>
		/// <param name="details">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public PathNotFoundException(Type pathfinderType, string details, Exception inner)
			: base(string.Format(DEFAULT_MESSAGE, pathfinderType.Name, details), inner)
		{
			this.PathfinderType = pathfinderType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PathNotFoundException"/> class
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected PathNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}


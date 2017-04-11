using System;

namespace Terrapass.GameAi.Goap.Planning
{
	[Serializable]
	public class UnknownSymbolException : Exception
	{
		private const string MESSAGE_TEMPLATE = "Unknown world state symbol {0}{1}";
		private const string DETAILS_TEMPLATE = " ({0})";

		public SymbolId SymbolId { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:UnknownSymbolException"/> class
		/// </summary>
		/// <param name="symbolId">The unknown <see cref="T:Terrapass.GameAi.Goap.Planning.WorldStateSymbolId"/>.</param>
		public UnknownSymbolException(SymbolId symbolId)
			: this(symbolId, null, null)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:UnknownSymbolException"/> class
		/// </summary>
		/// <param name="symbolId">The unknown <see cref="T:Terrapass.GameAi.Goap.Planning.WorldStateSymbolId"/>.</param>
		/// <param name="details">A <see cref="T:System.String"/> that describes the exception. </param>
		public UnknownSymbolException(SymbolId symbolId, string details)
			: this(symbolId, details, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:UnknownSymbolException"/> class
		/// </summary>
		/// <param name="symbolId">The unknown <see cref="T:Terrapass.GameAi.Goap.Planning.WorldStateSymbolId"/>.</param>
		/// <param name="details">A <see cref="T:System.String"/> that describes the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception.</param>
		public UnknownSymbolException(SymbolId symbolId, string details, Exception inner)
			: base(
				string.Format(
					MESSAGE_TEMPLATE, symbolId, details != null ? string.Format(DETAILS_TEMPLATE, details) : ""
				),
				inner
			)
		{
			this.SymbolId = symbolId;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:UnknownSymbolException"/> class
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected UnknownSymbolException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}


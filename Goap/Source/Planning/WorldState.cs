using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Planning
{
	public struct WorldState : IKnowledgeProvider, IEquatable<WorldState>, IEnumerable<KeyValuePair<SymbolId, int>>
	{
		private readonly IDictionary<SymbolId, int> symbols;

		private WorldState(IDictionary<SymbolId, int> symbols)
		{
			DebugUtils.Assert(symbols != null, "symbols dictionary must not be null");
			this.symbols = symbols;
		}

		public bool Contains(SymbolId symbolId)
		{
			return symbols != null && symbols.ContainsKey(symbolId);
		}

		public int this[SymbolId key]
		{
			get {
				try
				{
					return this.symbols[key];
				}
				catch(KeyNotFoundException e)
				{
					throw new UnknownSymbolException(
						key,
						string.Format(
							"No value for {0} is stored in {1}",
							key,
							this.GetType()
						),
						e
					);
				}
				catch(NullReferenceException e)
				{
					throw new UnknownSymbolException(
						key,
						string.Format(
							"Unable to retrieve value for {0}: {1} is empty",
							key,
							this.GetType()
						),
						e
					);
				}
			}
		}

		#region IKnowledgeProvider implementation

		public int GetSymbolValue(SymbolId symbolId)
		{
			return this[symbolId];
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<KeyValuePair<SymbolId, int>> GetEnumerator()
		{
			return this.symbols.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEquatable implementation

		public bool Equals(WorldState other)
		{
			// TODO: Maybe simply compare hashCodes? (Make sure that GetHashCode() is implemented properly first.)
			return (this.symbols == null && other.symbols == null)
				|| (this.symbols.Count == other.symbols.Count && !this.symbols.Except(other.symbols).Any());
		}

		#endregion

		public override bool Equals(object obj)
		{
			if(obj == null)
				return false;
			if(ReferenceEquals(this, obj))
				return true;
			if(obj.GetType() != typeof(WorldState))
				return false;
			WorldState other = (WorldState)obj;
			return this.Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				// 0 for no symbols
				if(symbols == null)
				{
					return 0;
				}

				// Order is not important.
				int hash = 0;
				foreach(var kvp in symbols)
				{
					hash += 17 * kvp.Key.GetHashCode() + 23 * kvp.Value.GetHashCode();
				}
				return hash;
			}
		}

		public override string ToString()
		{
			return string.Format(
				"[{0}]",
				symbols == null
					? ""
				: symbols.Aggregate("", (soFar, kvp) => soFar + string.Format("{0}={1}; ", kvp.Key, kvp.Value))
			);
		}

		public Builder BuildUpon()
		{
			return new Builder(this);
		}

		public class Builder
		{
			private readonly IDictionary<SymbolId, int> symbols;

			public Builder(WorldState original = default(WorldState))
			{
				if(original.symbols == null)	// Check for default instance
				{
					this.symbols = new Dictionary<SymbolId, int>();
				}
				else
				{
					this.symbols = new Dictionary<SymbolId, int>(original.symbols);
				}
			}

			public Builder ClearSymbols()
			{
				this.symbols.Clear();
				return this;
			}

			public Builder UnsetSymbol(SymbolId key)
			{
				this.symbols.Remove(key);
				return this;
			}

			public Builder SetSymbol(SymbolId key, int value)
			{
				this.symbols[key] = value;
				return this;
			}

			public int this[SymbolId key]
			{
				get {
					try
					{
						return this.symbols[key];
					}
					catch(KeyNotFoundException e)
					{
						throw new UnknownSymbolException(
							key,
							string.Format(
								"No value for {0} is stored in {1}",
								key,
								this.GetType()
							),
							e
						);
					}
				}
				set {
					this.SetSymbol(key, value);
				}
			}

			public WorldState Build()
			{
				return new WorldState(this.symbols);
			}
		}
	}

//	public struct WorldState
//	{
//		private readonly IDictionary<string, WorldStateSymbol> symbols;
//
//		private WorldState(IDictionary<string, WorldStateSymbol> symbols)
//		{
//			DebugUtils.Assert(symbols != null, "symbols dictionary must not be null");
//			this.symbols = symbols;
//		}
//
//		public WorldStateSymbol this[string key]
//		{
//			get {
//				return this.symbols[key];
//			}
//		}
//
//		public Builder BuildUpon()
//		{
//			return new Builder(this);
//		}
//
//		public class Builder
//		{
//			private readonly IDictionary<string, WorldStateSymbol> symbols;
//
//			public Builder(WorldState original = default(WorldState))
//			{
//				if(original.symbols == null)	// Check for default instance
//				{
//					this.symbols = new Dictionary<string, WorldStateSymbol>();
//				}
//				else
//				{
//					this.symbols = new Dictionary<string, WorldStateSymbol>(original.symbols);
//				}
//			}
//
//			public Builder ClearSymbols()
//			{
//				this.symbols.Clear();
//				return this;
//			}
//
//			public Builder UnsetSymbol(string key)
//			{
//				this.symbols.Remove(key);
//				return this;
//			}
//
//			public Builder SetSymbol(string key, WorldStateSymbol symbol)
//			{
//				this.symbols[key] = symbol;
//				return this;
//			}
//
//			public WorldStateSymbol this [string key]
//			{
//				get {
//					return this.symbols[key];
//				}
//				set {
//					this.SetSymbol(key, value);
//				}
//			}
//
//			public WorldState Build()
//			{
//				return new WorldStateSymbol(this.symbols);
//			}
//		}
//	}
}


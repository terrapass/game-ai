using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

using Terrapass.GameAi.Goap.Planning.Preconditions;

namespace Terrapass.GameAi.Goap.Planning.Internal
{
	internal struct RegressiveState : IEquatable<RegressiveState>, IEnumerable<KeyValuePair<SymbolId, ValueRange>>
	{
		private readonly IDictionary<SymbolId, ValueRange> ranges;

		private RegressiveState(IDictionary<SymbolId, ValueRange> ranges)
		{
			DebugUtils.Assert(ranges != null, "ranges must not be null");
			this.ranges = ranges;
		}

		public bool Contains(SymbolId symbolId)
		{
			return ranges != null && ranges.ContainsKey(symbolId);
		}

		public ValueRange this[SymbolId key]
		{
			get {
				try
				{
					return this.ranges[key];
				}
				catch(KeyNotFoundException)
				{
					return ValueRange.AnyValue;
				}
			}
		}

		#region IEnumerable implementation

		public IEnumerator<KeyValuePair<SymbolId, ValueRange>> GetEnumerator()
		{
			return ranges.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEquatable implementation
		public bool Equals(RegressiveState other)
		{
			return (this.ranges == null && other.ranges == null)
				|| (this.ranges.Count == other.ranges.Count && !this.ranges.Except(other.ranges).Any());
		}
		#endregion

		public override bool Equals(object obj)
		{
			if(obj == null)
				return false;
			if(ReferenceEquals(this, obj))
				return true;
			if(obj.GetType() != typeof(RegressiveState))
				return false;
			RegressiveState other = (RegressiveState)obj;
			return this.Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				// 0 for no constraints
				if(ranges == null)
				{
					return 0;
				}

				// Only non-AnyValue constraints contribute to the hash value, order is not important.
				int hash = 0;
				foreach(var kvp in ranges)
				{
					hash += ValueRange.AnyValue.Equals(kvp.Value)
						? 0
						: 17 * kvp.Key.GetHashCode() + 23 * kvp.Value.GetHashCode();
				}
				return hash;
			}
		}

		public override string ToString()
		{
			return string.Format(
				"[{0}]",
				ranges == null
					? ""
					: ranges.Aggregate("", (soFar, kvp) => soFar + string.Format("{0} in {1}; ", kvp.Key, kvp.Value))
			);
		}

		public Builder BuildUpon()
		{
			return new Builder(this);
		}

		public class Builder
		{
			private readonly IDictionary<SymbolId, ValueRange> ranges;

			public Builder(RegressiveState original = default(RegressiveState))
			{
				if(original.ranges == null)	// Check for default instance
				{
					this.ranges = new Dictionary<SymbolId, ValueRange>();
				}
				else
				{
					this.ranges = new Dictionary<SymbolId, ValueRange>(original.ranges);
				}
			}

			public Builder ClearRanges()
			{
				this.ranges.Clear();
				return this;
			}

			public Builder UnsetRange(SymbolId key)
			{
				this.ranges.Remove(key);
				return this;
			}

			public Builder SetRange(SymbolId key, ValueRange value)
			{
				this.ranges[key] = value;
				return this;
			}

			public Builder IntersectRange(SymbolId key, ValueRange intersectedRange)
			{
				this.ranges[key] = this[key].Intersect(intersectedRange);
				return this;
			}

			public ValueRange this[SymbolId key]
			{
				get {
					try
					{
						return this.ranges[key];
					}
					catch(KeyNotFoundException)
					{
						return ValueRange.AnyValue;
//						throw new UnknownSymbolException(
//							key,
//							string.Format(
//								"No value range for {0} is stored in {1}",
//								key,
//								this.GetType()
//							),
//							e
//						);
					}
				}
				set {
					this.SetRange(key, value);
				}
			}

			public RegressiveState Build()
			{
				return new RegressiveState(this.ranges);
			}
		}
	}
}


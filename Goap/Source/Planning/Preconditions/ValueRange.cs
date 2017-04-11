using System;
using System.Linq;
using System.Collections.Generic;

using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Planning.Preconditions
{
	public struct ValueRange : IEquatable<ValueRange>
	{
		public static readonly ValueRange AnyValue = new ValueRange(int.MinValue, int.MaxValue);
		public static readonly ValueRange Empty = new ValueRange(1, 0);

		public static ValueRange Exactly(int value)
		{
			return new ValueRange(value, value);
		}

		public static ValueRange LessThan(int value)
		{
			return new ValueRange(int.MinValue, value - 1);
		}

		public static ValueRange LessThanOrEqual(int value)
		{
			return new ValueRange(int.MinValue, value);
		}

		public static ValueRange GreaterThan(int value)
		{
			return new ValueRange(value + 1, int.MaxValue);
		}

		public static ValueRange GreaterThanOrEqual(int value)
		{
			return new ValueRange(value, int.MaxValue);
		}

		public static ValueRange Between(int minValue, int maxValue)
		{
			return new ValueRange(minValue, maxValue);
		}

		public int MinValue {get;}
		public int MaxValue {get;}

		private ValueRange(int minValue, int maxValue)
		{
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}

		public bool Contains(int value)
		{
			return value >= MinValue && value <= MaxValue;
		}

		public bool IsEmpty
		{
			get {
				return MaxValue < MinValue;
			}
		}

		public bool IsSingleValue
		{
			get {
				return MinValue == MaxValue;
			}
		}

		public int Size
		{
			get {
				return this.IsEmpty
					? 0
					: (MaxValue - MinValue) + 1;
			}
		}

		public int AbsDistanceFrom(int value)
		{
			return this.Contains(value)
				? 0
				: value < this.MinValue ? this.MinValue - value : value - this.MaxValue;
		}

		public ValueRange Intersect(ValueRange other)
		{
			return new ValueRange(
				Math.Max(this.MinValue, other.MinValue),
				Math.Min(this.MaxValue, other.MaxValue)
			);
		}

		public ValueRange ShiftBy(int shift)
		{
			return new ValueRange(
				SafeAdd(MinValue, shift),
				SafeAdd(MaxValue, shift)
			);
		}

		private static int SafeAdd(int initialValue, int addedValue)
		{
			return (initialValue == int.MinValue || initialValue == int.MaxValue)
				? initialValue
					: initialValue + ((initialValue >= 0) ? Math.Min(addedValue, int.MaxValue - initialValue) : Math.Max(addedValue, -(initialValue - int.MinValue)));
		}

		#region IEquatable implementation

		public bool Equals(ValueRange other)
		{
			return this.MinValue == other.MinValue && this.MaxValue == other.MaxValue;
		}

		#endregion

		public override bool Equals(object obj)
		{
			if(obj == null)
				return false;
			if(ReferenceEquals(this, obj))
				return true;
			if(obj.GetType() != typeof(ValueRange))
				return false;
			ValueRange other = (ValueRange)obj;
			return this.Equals(other);
		}
		

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 23;
				hash = hash * 37 + MinValue.GetHashCode();
				hash = hash * 37 + MaxValue.GetHashCode();
				return hash;
			}
		}
		

		public override string ToString()
		{
			return this.IsEmpty
				? "{}"
				: this.MinValue == this.MaxValue
					? string.Format("{{{0}}}", MinValue)
					: string.Format("[{0}, {1}]", MinValue, MaxValue);
		}
//		private readonly int[] points;
//
//		private ValueRange(int minValue, int maxValue)
//			: this(new int[] {minValue, maxValue})
//		{
//			
//		}
//
//		private ValueRange(int[] points)
//		{
//			DebugUtils.Assert(points != null, "points must not be null");
//			DebugUtils.Assert(points.Length > 0, "points must not be empty");
//			DebugUtils.Assert(points.Length % 2 == 0, "points must contain an even number of elements");
//			this.points = points;
//		}
//
//		public bool Contains(int value)
//		{
//			DebugUtils.Assert(points.Length % 2 == 0, "points must contain an even number of elements");
//
//			for(int i = 0; i < points.Length; i += 2)
//			{
//				if(value >= points[i] && value <= points[i + 1])
//				{
//					return true;
//				}
//			}
//			return false;
//		}
//
//		public ValueRange ShiftBy(int shift)
//		{
//			return new ValueRange(
//				this.points.Select(
//					(p) => (p == int.MinValue || p == int.MaxValue)
//						? p
//						: p + (p >= 0) ? Math.Min(shift, int.MaxValue - p) : Math.Max(shift, -(p - int.MinValue))
//				).ToArray()
//			);
//		}
	}
}


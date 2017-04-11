using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrapass.GameAi.Goap.Utils.Collections
{
	/// <summary>
	/// Priority queue implementation, based on the underlying SortedList<T, object> instance,
	/// where each unneeded object value is null.
	/// </summary>
	/// <remarks>
	/// SortedListBasedPriorityQueue does NOT allow duplicate items!
	/// (This is because the underlying SortedListBasedPriorityQueue does not allow them.)
	/// </remarks>
	public class SortedListBasedPriorityQueue<T> : IPriorityQueue<T>
	{
		private readonly SortedList<T, object> sortedList;

		public SortedListBasedPriorityQueue()
		{
			this.sortedList = new SortedList<T, object>();
		}

		public SortedListBasedPriorityQueue(IComparer<T> comparer)
		{
			this.sortedList = new SortedList<T, object>(comparer);
		}

		#region ICollection implementation
		public void Add(T item)
		{
			this.sortedList.Add(item, null);
		}

		public void Clear()
		{
			this.sortedList.Clear();
		}

		public bool Contains(T item)
		{
			return this.sortedList.ContainsKey(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.sortedList.Keys.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			return this.sortedList.Remove(item);
		}

		public int Count
		{
			get {
				return this.sortedList.Count;
			}
		}

		public bool IsReadOnly
		{
			get {
				return false;
			}
		}
		#endregion

		#region IEnumerable implementation

		public IEnumerator<T> GetEnumerator ()
		{
			return this.sortedList.Keys.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable)this.sortedList.Keys).GetEnumerator();
		}

		#endregion

		#region IPriorityQueue implementation

		public T PopFront()
		{
			var poppedValue = this.Front;
			this.sortedList.RemoveAt(0);
			return poppedValue;
		}

		public T Front
		{
			get {
				if(this.Count == 0)
				{
					throw new InvalidOperationException(
						string.Format(
							"Attempted to retrieve Front from an empty {0}",
							this.GetType()
						)
					);
				}
				return this.sortedList.Keys[0];
			}
		}
		#endregion
	}
}


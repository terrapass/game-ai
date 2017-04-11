using System;
using System.Collections.Generic;

namespace Terrapass.GameAi.Goap.Utils.Collections
{
	public interface IPriorityQueue<T> : ICollection<T>
	{
		T Front {get;}

		T PopFront();
	}
}


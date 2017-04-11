using System;

using Terrapass.GameAi.Goap.Debug;

namespace Terrapass.GameAi.Goap.Planning
{
	public struct SymbolId : IEquatable<SymbolId>
	{
		public string Name {get;}
		// TODO: Add array, containing parameters
		// for symbol IDs such as AgentAtNode(NODE_ID)

		public SymbolId(string name)
		{
			this.Name = PreconditionUtils.EnsureNotBlank(name, "name");
		}

		#region IEquatable implementation

		public bool Equals(SymbolId other)
		{
			return Name == other.Name;
		}

		#endregion

		public override bool Equals(object obj)
		{
			if(obj == null)
				return false;
			if(ReferenceEquals(this, obj))
				return true;
			if(obj.GetType() != typeof(SymbolId))
				return false;
			SymbolId other = (SymbolId)obj;
			return this.Equals(other);
		}
		

		public override int GetHashCode()
		{
			unchecked
			{
				return (Name != null ? Name.GetHashCode() : 0);
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}


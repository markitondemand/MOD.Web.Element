using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOD.Web.Element
{
	/// <summary>
	/// Node class. This is the base class for all Rednr classes
	/// </summary>
	public class Node : INode
	{
		public Node Parent;
		public virtual void ToString(StringBuilder sb) { }
	}
}

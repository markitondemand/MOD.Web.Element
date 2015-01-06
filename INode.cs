using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOD.Web.Element
{
	/// <summary>
	/// INode interface. Usefull for polymorphism
	/// </summary>
	public interface INode
	{
		/// <summary>
		/// Converts the node to a string.
		/// </summary>
		/// <param name="sb"></param>
		void ToString(StringBuilder sb);
	}
}

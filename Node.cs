using System.Text;

namespace MOD.Web.Element
{
	/// <summary>
	/// Node class. This is the base class for all Rednr classes
	/// </summary>
	public class Node : INode
	{
		/// <summary>
		/// A link to the parent node.
		/// </summary>
		public Node Parent;

		/// <summary>
		/// Renders the Node as a string.  Left virtual for child classes to implement.
		/// </summary>
		/// <param name="sb"></param>
		public virtual void ToString(StringBuilder sb) { }
	}
}

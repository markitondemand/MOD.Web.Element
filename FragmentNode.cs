using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MOD.Web.Element
{
	/// <summary>
	/// Fragment: a collection on nodes. Use this when you don't have a parent element
	/// </summary>
	public class FragmentNode : IFragment
	{
		public FragmentNode()
		{
			_children = new List<INode>();
		}

		/// <summary>
		/// Stores child nodes
		/// </summary>
		public IList<INode> Children { get { return _children; }}
		private List<INode> _children;

		/// <summary>
		/// Writes the rendered element to the given TextWriter
		/// </summary>
		public virtual TextWriter Write(TextWriter tw)
		{
			foreach(INode node in _children)
			{
				if (node != null) {
					node.Write(tw);
				}
			}
			return tw;
		}

		/// <summary>
		/// Returns the html representation of the node and it's children. Use Write() instead if possible.
		/// </summary>
		public override string ToString()
		{
			StringWriter sw = new StringWriter();
			Write(sw);
			return sw.ToString();
		}

		/// <summary>
		/// Returns the html representation of the node and it's children. Same as calling ToString();
		/// </summary>
		public virtual string ToHtmlString()
		{
			return this.ToString();
		}
	}
}

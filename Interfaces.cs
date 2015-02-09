using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MOD.Web.Element
{
	/// <summary>
	/// INode is the basis for all other nodes.
	/// </summary>
	public interface INode : IHtmlString
	{
		/// <summary>
		/// All nodes must be writeable.
		/// </summary>
		TextWriter Write(TextWriter tw);
		/// <summary>
		/// Shortcut for rendering to a string.
		/// Use Write() instead if you have access to an TextWriter such as HttpContext.Response.Output
		/// </summary>
		string ToString();
	}
	
	/// <summary>
	/// A fragment with a tag and attributes
	/// </summary>
	public interface IElement : IFragment, INode
	{
		/// <summary>
		/// The Name of the tag represented by this Element.
		/// </summary>
		string TagName { get; set; }

		/// <summary>
		/// The Attributes of this Element, represented as key/value pairs.
		/// Generally represents classes, ID, inline styles, or tag specific attributes like src for Images or href for anchors.
		/// Can be anything though.
		/// </summary>
		IDictionary<string,string> Attributes { get; }

		/// <summary>
		/// Sets the tag name of the element, parsing the given tag name expression for ids and classes specified as css selectors.
		/// </summary>
		/// <remarks>
		/// Exposing this becuase I don't want this logic to live in an extension method so that it can be subclassed if desired.
		/// </remarks>
		void ParseTag(string @tag);
	}

	/// <summary>
	/// Text nodes contain a basic value
	/// </summary>
	public interface IText : INode
	{
		/// <summary>
		/// Contents of the node
		/// </summary>
		IConvertible Value { get; set; }

		/// <summary>
		/// Determines whether to html-encode the content when rendering.
		/// </summary>
		bool IsHtmlEncoded { get; }
	}
	/// <summary>
	/// Fragments have a list of nodes
	/// </summary>
	public interface IFragment : INode
	{
		/// <summary>
		/// The list of child nodes.
		/// </summary>
		IList<INode> Children { get; }
	}

	/// <summary>
	/// Flags for special handling of tags
	/// </summary>
	[Flags]
	public enum SpecialHandling
	{
		/// <summary>
		/// This is here as a default or 'null' value
		/// </summary>
		None = 0x00,
		/// <summary>
		/// Flag this node as not having children so that it doesn't print the end tag ( <br/> )
		/// </summary>
		SelfClosing = 0x01
	}
}

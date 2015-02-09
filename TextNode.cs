using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MOD.Web.Element
{
	/// <summary>
	/// Represents a Node that is only text, no children and not a tag.
	/// Used for storing text-only data in the DOM tree.
	/// </summary>
	public sealed class TextNode : IText
	{
		/// <summary>
		/// Use this constructor if you want the value html-encoded
		/// </summary>
		public TextNode(IConvertible value) : this(false,value)
		{
		}

		/// <summary>
		/// Use this constructor if you want the value left raw
		/// </summary>
		public TextNode(bool disableEncoding, IConvertible value)
		{
			Value = value;
			IsHtmlEncoded = !disableEncoding;
		}

		/// <summary>
		/// The actual stored value.
		/// </summary>
		public IConvertible Value { get; set; }

		/// <summary>
		/// If true, will convert HTML to URL-friendly format.
		/// " =&gt; &amp;quot;
		/// &amp; =&gt; &amp;amp;
		/// ' =&gt; &amp;#39;
		/// &lt; =&gt; &amp;lt;
		/// &gt; =&gt; &amp;gt;
		/// </summary>
		public bool IsHtmlEncoded { get; private set; }

		/// <summary>
		/// Renders the output to the given TextWriter
		/// </summary>
		public TextWriter Write(TextWriter tw)
		{
			if (Value != null) {
				tw.Write(this.ToString());
			}
			return tw;
		}

		/// <summary>
		/// Returns the output as a string. Use Write() instead if possible
		/// </summary>
		public override string ToString()
		{
			if (Value != null) {
				return IsHtmlEncoded
					? HttpUtility.HtmlEncode(Value.ToString())
					: Value.ToString()
				;
			}
			return "";
		}

		/// <summary>
		/// Returns the output as a string. Same as calling ToString();
		/// </summary>
		public string ToHtmlString()
		{
			return this.ToString();
		}
	}
}

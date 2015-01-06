using System;
using System.Text;
using System.Web;

namespace MOD.Web.Element
{
	/// <summary>
	/// Represents a Node that is only text, no children and not a tag.
	/// Used for storing text-only data in the DOM tree.
	/// </summary>
	public class Text : Node
	{
		/// <summary>
		/// Default constructor.  Does nothing.
		/// </summary>
		public Text() { }

		/// <summary>
		/// Creates the text node containing one string that is the concatenation of the array of strings specified.
		/// </summary>
		/// <param name="text">An array of strings that are the text of this node.  Will be joined into one long string.</param>
		public Text(params string[] text) : this(String.Join("", text)) { }

		/// <summary>
		/// Initializes the Node to contain the string specified.  Specifies that any HTML in the string will be encoded.
		/// </summary>
		/// <param name="text">The string that will be the contents of this Node.</param>
		public Text(string text) : this(text, false) { }

		/// <summary>
		/// Initializes the node to contain the specified string, with the option to be encoded or not.
		/// </summary>
		/// <param name="text">The text to contain.</param>
		/// <param name="leaveRaw">True for strings that should not be HTML-encoded.  False will cause the string to be HTML encoded when it is rendered.</param>
		public Text(string text, bool leaveRaw)
		{
			Value = text;
			IsEncoded = !leaveRaw;
		}

		/// <summary>
		/// Converts the node to a string and appends it to the specified StringBuilder.
		/// </summary>
		/// <param name="sb">The StringBuilder to which the string contents will be appended</param>
		public override void ToString(StringBuilder sb)
		{
			sb.Append(ToString());
		}

		/// <summary>
		/// Gets the string contained in this Node.
		/// </summary>
		/// <returns>A string, which will be HTML-encoded if leaveRaw was set to false (or not specified, as this is the default behavior).</returns>
		public override string ToString()
		{
			if (IsEncoded)
			{
				return HttpUtility.HtmlEncode(Value); //This doesn't encode unicode characters!!??
			}
			else
			{
				return Value;
			}
		}

		/// <summary>
		/// If true, will convert HTML to URL-friendly format.
		/// " =&gt; &amp;quot;
		/// &amp; =&gt; &amp;amp;
		/// ' =&gt; &amp;#39;
		/// &lt; =&gt; &amp;lt;
		/// &gt; =&gt; &amp;gt;
		/// </summary>
		public bool IsEncoded;

		/// <summary>
		/// The actual string value.
		/// </summary>
		public string Value;
	}
}

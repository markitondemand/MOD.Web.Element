using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MOD.Web.Element
{
	/// <summary>
	/// Represents an element node in the DOM tree. It's represented as a fragment with a tag
	/// </summary>
	public class ElementNode : FragmentNode, IElement
	{
		/// <summary>
		/// Construct the Element with the type specified by the string parameter.
		/// For example, new Element("a") will create an anchor tag.
		/// </summary>
		/// <param name="tag">The Tag type ("a","div","table","ul", etc) </param>
		public ElementNode(string tag)
		{
			Init(tag);
		}

		/// <summary>
		/// Does the initialization of the element; called by the constructor
		/// </summary>
		protected virtual void Init(string tag)
		{
			_attributes = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
			ParseTag(tag);
		}

		/// <summary>
		/// The Name of the tag represented by this Element.
		/// </summary>
		public string TagName { get; set; }

		/// <summary>
		/// The Attributes of this Element, represented as key/value pairs.
		/// Generally represents classes, ID, inline styles, or tag specific attributes like src for Images or href for anchors.
		/// Can be anything though.
		/// </summary>
		public IDictionary<string,string> Attributes { get { return _attributes; }}
	
		private Dictionary<string,string> _attributes;
		private static readonly char[] TAG_DELIMS = new[] { '#', '.' };

		/// <summary>
		/// Sets the tag name of the element, parsing the given tag name expression for ids and classes specified as css selectors.
		/// </summary>
		/// <remarks>
		/// Class names can be separated by dots or by spaces.  This function looks for class names by tokenizing the string on dots '.'
		/// and then adds each class to the class attribute of this node, which just concatenates all class names into one string, separated by spaces.
		/// So "div#myId.class1.class2.class3" is the same as "div#myID.class1 class2 class3"
		/// </remarks>
		/// <example>
		/// div.myclass#myid
		/// </example>
		/// <param name="tagNameExpression">Name of the tag, possibly with \#id and .classnames appended</param>
		public virtual void ParseTag(string tagNameExpression)
		{
			int iPos = tagNameExpression.IndexOfAny(TAG_DELIMS);
			if (iPos == -1)
			{
				// Use the simple tag name
				TagName = tagNameExpression;
				return;
			}

			int iStart = 0; string sValue;
			// Isolate the tag name. may not have one if called from Create
			if (iPos > 0)
			{
				TagName = tagNameExpression.Substring(0, iPos);
			}

			// Parse out the remaining items
			for (iStart = iPos; iPos != -1; iStart = iPos)
			{
				iPos = tagNameExpression.IndexOfAny(TAG_DELIMS, iStart + 1);
				sValue = iPos != -1
					? tagNameExpression.Substring(iStart + 1, iPos - iStart - 1)
					: tagNameExpression.Substring(iStart + 1)
				;
				switch (tagNameExpression[iStart])
				{
					case '.': AddClassInternal(sValue); break;
					case '#': Attributes["id"] = sValue; break;
				}
			}
		}

		private void AddClassInternal(string newClass)
		{
			string current;
			if (!Attributes.TryGetValue("class",out current)) {
				current = "";
			} else {
				current = current.Trim();
			}

			current +=
				(String.IsNullOrEmpty(current) ? "" : " ")
				+ newClass.Trim();

			if (!String.IsNullOrEmpty(current)) {
				Attributes["class"] = current;
			}
		}

		/// <summary>
		/// Returns a flag that specifies if this tag needs special handling.
		/// </summary>
		protected virtual SpecialHandling GetSpecialHandlingFlag(string tag)
		{
			if (tag != null)
			{
				switch(tag.ToLowerInvariant())
				{
				case "area":		return SpecialHandling.SelfClosing;
				case "br":			return SpecialHandling.SelfClosing;
				case "col":			return SpecialHandling.SelfClosing;
				case "img":			return SpecialHandling.SelfClosing;
				case "input":		return SpecialHandling.SelfClosing;
				case "hr":			return SpecialHandling.SelfClosing;
				case "link":		return SpecialHandling.SelfClosing;
				case "meta":		return SpecialHandling.SelfClosing;
				case "param":		return SpecialHandling.SelfClosing;
				}
			}
			return SpecialHandling.None;
		}

		/// <summary>
		/// Builds the html output writting to the given TextWritter
		/// </summary>
		public override TextWriter Write(TextWriter tw)
		{
			bool selfClosing = 0 != (GetSpecialHandlingFlag(TagName) & SpecialHandling.SelfClosing);
			tw.Write('<'); tw.Write(TagName);
			WriteAttributes(tw);
			if (selfClosing) {
				tw.Write('/'); tw.Write('>');
			} else {
				tw.Write('>');
				base.Write(tw); //children
				tw.Write('<'); tw.Write('/'); tw.Write(TagName); tw.Write('>');
			}
			return tw;
		}

		/// <summary>
		/// Generates the attributes text for the html element
		/// </summary>
		protected virtual void WriteAttributes(TextWriter writer)
		{
			if (null != _attributes)
			{
				foreach (KeyValuePair<string,string> kvp in _attributes)
				{
					string val = HttpUtility.HtmlEncode(kvp.Value);
					writer.Write(' '); writer.Write(kvp.Key);
					writer.Write('='); writer.Write('"');
					writer.Write(val); writer.Write('"');
				}
			}
		}

		/// <summary>
		/// Returns the finished html as a string. You can use Write() instead if you have the page output textwriter available.
		/// </summary>
		public override string ToString()
		{
			StringWriter sw = new StringWriter();
			Write(sw);
			return sw.ToString();
		}
	}
}

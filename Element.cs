using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace MOD.Web.Element
{
	#region Instance Members
	/// <summary>
	/// Represents an Element in the DOM Tree.  This is either a Tag or Text.
	/// It has a type (the html tag as a string).  
	/// It can have a set of attributes represented as strings.
	/// It can have a list of child Elements.
	/// Extends Node and Implements the Renderable Interface.
	/// </summary>
	public partial class Element : Node, IRenderable
	{
		/// <summary>
		/// Construct the Element with the type specified by the string parameter.
		/// For example, new Element("a") will create an anchor tag.
		/// </summary>
		/// <param name="tag">The Tag type ("a","div","table","ul", etc) </param>
		public Element(string tag)
		{
			ParseTagName(tag);
		}

		/// <summary>
		/// Render this element as a Node.
		/// </summary>
		/// <returns>Returns this (as a Node)</returns>
		public Node Render()
		{
			return this;
		}

		/// <summary>
		/// Children contains all child nodes (is a Fragment)
		/// </summary>
		public Fragment Children
		{
			get
			{
				if (null == children)
				{
					children = new Fragment();
				}
				return children;
			}
			set
			{
				children = value;
			}
		}

		/// <summary>
		/// The attributes for this Element as a dictionary of strings.
		/// Attributes being everything specified in the tag such as inline style, href, width, height, src, etc.
		/// Note that this dictionary is case Insensitive.
		/// </summary>
		public Dictionary<string, string> Attributes
		{
			get
			{
				InitAttributes(); return attributes;
			}
		}

		/// <summary>
		/// Initializes the attributes member variable as an empty Dictionary, if it hasn't been initialized yet.
		/// </summary>
		protected virtual void InitAttributes()
		{
			if (null == attributes)
			{
				attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			}
		}

		/// <summary>
		/// The Name of the tag represented by this Element.
		/// </summary>
		public string TagName;

		/// <summary>
		/// Children of this Element.  Fragment is just a list of Nodes.
		/// </summary>
		protected Fragment children = null;

		/// <summary>
		/// The Attributes of this Element, represented as key/value pairs.
		/// Generally represents classes, ID, inline styles, or tag specific attributes like src for Images or href for anchors.
		/// Can be anything though.
		/// </summary>
		protected Dictionary<string, string> attributes = null;

		/// <summary>
		/// Add any supported type of object as a child of this Element.
		/// If the type is IEnumerable, each value in that set will be added individually.
		/// 
		/// Supported Types:
		/// string, IEnumerable&lt;string&gt;, 
		/// Node, IEnumerable&lt;Node&gt;, 
		/// IEnumerable&lt;Element&gt;, 
		/// IRenderable, IEnumerable&lt;IRenderable&gt;,
		/// IEnumerable&lt;object&gt;,
		/// long, int, double (will be converted to a string and added as text)
		/// </summary>
		/// <param name="objectList">An array of objects</param>
		/// <returns>This</returns>
		public Element Add(params object[] objectList)
		{
			return Add((IEnumerable<object>)objectList);
		}

		/// <summary>
		/// Add an enumerable collection of objects of any support type as child Nodes.
		/// This iterates over the collection and calls the appropriate Add function based on 
		/// the type of that item.
		/// 
		/// Supported Types:
		/// string, IEnumerable&lt;string&gt;, 
		/// Node, IEnumerable&lt;Node&gt;, 
		/// IEnumerable&lt;Element&gt;, 
		/// IRenderable, IEnumerable&lt;IRenderable&gt;,
		/// IEnumerable&lt;object&gt;,
		/// long, int, double (will be converted to a string and added as text)
		/// </summary>
		/// <param name="objectList">The Enumerable collection of objects</param>
		/// <returns>This</returns>
		public Element Add(IEnumerable<object> objectList)
		{
			if (objectList == null)
			{
				return this;
			}
			foreach (object obj in objectList)
			{
				if (obj == null)
				{
					continue;
				}

				if (obj is string)
				{
					AddText((string)obj);
				}
				else if (obj is IEnumerable<string>)
				{
					Add((IEnumerable<string>)obj);
				}
				else if (obj is Node)
				{
					Add((Node)obj);
				}
				else if (obj is IEnumerable<Node>)
				{
					Add((IEnumerable<Node>)obj);
				}
				else if (obj is IEnumerable<Element>)
				{
					foreach (Element child in (IEnumerable<Element>)obj)
					{
						Add(child);
					}
				}
				else if (obj is IEnumerable<IRenderable>)
				{
					foreach (IRenderable child in (IEnumerable<IRenderable>)obj)
					{
						Add(child);
					}
				}
				else if (obj is IEnumerable<object>)
				{
					Add((IEnumerable<object>)obj);
				}
				else if (obj is IRenderable)
				{
					Add((obj as IRenderable).Render());
				}
				else if (obj is long || obj is int || obj is double)
				{
					Add(obj.ToString());
				}
				else
				{
					throw new Exception(
						string.Format("Element: Unsupported type:{0}, type: {1}", obj.ToString(), obj.GetType().Name)
					);
				}
			}
			return this;
		}

		/// <summary>
		/// Adds a Fragment as child Nodes.
		/// </summary>
		/// <remarks>
		/// Because Fragment is both an IList and a Node, there is some Type
		/// ambiguity when it comes to which Add() method to use. This solves
		/// the Type ambiguity for adding an individual Fragment.
		/// </remarks>
		/// <param name="fragment"></param>
		/// <returns></returns>
		public Element Add(Fragment fragment)
		{
			return Add((Node)fragment);
		}

		/// <summary>
		/// Adds an array of Nodes.  This converts the array to IEnumerable&lt;Node&gt; and adds that.
		/// </summary>
		/// <param name="nodeList">The array of Nodes to add</param>
		/// <returns>This</returns>
		public Element Add(params Node[] nodeList)
		{
			return Add((IEnumerable<Node>)nodeList);
		}

		/// <summary>
		/// Adds an IENumerable collection.  Each item will have its parent set to this and will then be 
		/// added to this's set of children.
		/// </summary>
		/// <param name="nodeList">the Enumerable collection of Nodes to add</param>
		/// <returns>This</returns>
		public Element Add(IEnumerable<Node> nodeList)
		{
			if (nodeList == null)
			{
				//Debug.WriteLine("Add null == nodeList");
				return this;
			}
			foreach (Node node in nodeList)
			{
				if (null == node)
				{
					//Debug.WriteLine("Add null == node");
					continue;
				}
				node.Parent = this;
				Children.Add(node);
			}
			return this;
		}

		/// <summary>
		/// Adds text that will be htmlencoded. Use AddHtml() to leave text unencoded.
		/// </summary>
		/// <param name="textList">An Array of strings to add (each string will become a text node)</param>
		/// <returns>This</returns>
		public Element Add(params string[] textList)
		{
			return AddText(textList);
		}

		/// <summary>
		/// Add a collection of strings as text nodes.
		/// </summary>
		/// <param name="textList">The enumerable collection of strings to add (each string will become a text node).</param>
		/// <returns>This</returns>
		public Element Add(IEnumerable<string> textList)
		{
			return AddText(textList);
		}

		/// <summary>
		/// Add an array of strings as child text nodes of this Element.
		/// The array will be converted to IEnumerable first.
		/// </summary>
		/// <param name="textList">An array of strings to add</param>
		/// <returns>This</returns>
		public Element AddText(params string[] textList)
		{
			return AddText((IEnumerable<string>)textList);
		}

		/// <summary>
		/// Add an enumerable collection of strings
		/// Each string will become a separate child text node.
		/// </summary>
		/// <param name="textList">The enumerable collection of strings to add</param>
		/// <returns>This</returns>
		public Element AddText(IEnumerable<string> textList)
		{
			if (textList == null)
			{
				//Debug.WriteLine("AddText null == textList");
				return this;
			}
			foreach (String text in textList)
			{
				if (String.IsNullOrEmpty(text))
				{
					//Debug.WriteLine("AddText null = text");
				}
				Text node = new Text(text);
				node.Parent = this;
				this.Children.Add(node);
			}
			return this;
		}

		/// <summary>
		/// Adds text that will NOT be htmlencoded.
		/// Had to compromise on the name; you can't have both a static and instance version of the same method.
		/// </summary>
		///<param name="textList">Array of strings to be added as non-encoded text nodes </param>
		///<returns>this</returns>
		public Element AddHtml(params string[] textList)
		{
			return AddHtml((IEnumerable<string>)textList);
		}

		/// <summary>
		/// Add an enumerable collection of strings that will not be HTML encoded.
		/// Each string will become a separate child text node.
		/// </summary>
		/// <param name="textList">The enumerable collection of strings to add</param>
		///<returns>this</returns>
		public Element AddHtml(IEnumerable<string> textList)
		{
			if (textList == null)
			{
				//Debug.WriteLine("AddHtml null == textList");
				return this;
			}
			foreach (String text in textList)
			{
				if (String.IsNullOrEmpty(text))
				{
					//Debug.WriteLine("AddHtml null == text");
					continue;
				}
				Text node = new Text(text, true);
				node.Parent = this;
				this.Children.Add(node);
			}
			return this;
		}

		/// <summary>
		/// Adds a class or classes into the class list of a node.
		/// In the attribute dictionary there is only one entry for "class".
		/// This is a string that has all the class names separated by spaces.
		/// </summary>
		/// <param name="classList">Class names to add</param>
		/// <returns>this</returns>
		public Element AddClass(params string[] classList)
		{
			if (classList == null)
			{
				//Debug.WriteLine("AddClass null == classList");
				return this;
			}

			StringBuilder sb = new StringBuilder();
			bool isFirst = true;
			if (Attributes.ContainsKey("class"))
			{
				sb.Append(Attributes["class"]);
				isFirst = false;
			}

			foreach (string name in classList)
			{
				if (String.IsNullOrEmpty(name))
				{
					//Debug.WriteLine("AddClass null == name");
					continue;
				}
				if (!isFirst) { sb.Append(' '); }
				sb.Append(name);
				isFirst = false;
			}

			string final = sb.ToString();
			if (!String.IsNullOrEmpty(final))
			{
				Attributes["class"] = final;
			}
			return this;
		}

		/// <summary>
		/// Sets an attribute to the given value. If the value is null the attribute is not set.
		/// </summary>
		/// <remarks>
		/// Since the attribute will not be set if the value is null, you can conditionally apply attributes inline 
		/// with something like Element.Create("option").SetAttribute("selected", value == selectedValue ? "selected" : null).
		/// </remarks>
		/// <param name="name">The name of the attribute whose value is going to be set</param>
		/// <param name="value">The new value of the attribute to be set</param>
		/// <returns>this</returns>
		public Element SetAttribute(string name, string value)
		{
			if (value != null)
			{
				Attributes[name] = value;
			}
			return this;
		}

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
		protected void ParseTagName(string tagNameExpression)
		{
			//Debug.Assert(!String.IsNullOrEmpty(tagNameExpression)
			//    , "ParseTagName null == tagNameExpression");

			int iPos = tagNameExpression.IndexOfAny(tagDelims);
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
				iPos = tagNameExpression.IndexOfAny(tagDelims, iStart + 1);
				sValue = iPos != -1
					? tagNameExpression.Substring(iStart + 1, iPos - iStart - 1)
					: tagNameExpression.Substring(iStart + 1)
				;
				switch (tagNameExpression[iStart])
				{
					case '.': AddClass(sValue); break;
					case '#': Attributes["id"] = sValue; break;
				}
			}
		}

		/// <summary>
		///  Valid delimiters for ParseTagName.  '#' for ID, '.' for class.  It doesn't do anything special with white space.
		/// </summary>
		protected static char[] tagDelims = { '.', '#' };

		/// <summary>
		/// Render this Element into html (convert this Element and its children into a string).
		/// </summary>
		/// <returns>String in HTML format representing this element and its children</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			ToString(sb);
			return sb.ToString();
		}

		/// <summary>
		/// Render this Element into html.  Adds the resulting string to the StringBuilder.
		/// </summary>
		/// <param name="sb">Stringbuilder object used to process the rendered HTML string.</param>
		public override void ToString(StringBuilder sb)
		{
			//Debug.Assert(sb != null, "ToString null == sb");
			sb.Append('<').Append(TagName);
			if (null != attributes && attributes.Count > 0)
			{
				foreach (KeyValuePair<string, string> kvp in attributes)
				{
					string val = HttpUtility.HtmlEncode(kvp.Value);
					sb.Append(' ').Append(kvp.Key).Append('=').Append('"').Append(val).Append('"');
				}
			}

			bool isEmpty = false;
			if (SpecialHandlingMap.ContainsKey(TagName))
			{
				SpecialHandling sh = SpecialHandlingMap[TagName];
				isEmpty = SpecialHandling.SelfClosing == (sh & SpecialHandling.SelfClosing);
			}

			if (isEmpty)
			{
				sb.Append('/').Append('>');
			}
			else
			{
				sb.Append('>');
				if (null != children && children.Count > 0)
				{
					foreach (Node child in children)
					{
						child.ToString(sb);
					}
				}
				sb.Append('<').Append('/').Append(TagName).Append('>');
			}
		}

		/// <summary>
		/// Apply the attributes specified in attribList to this element
		/// </summary>
		/// <remarks>
		/// attribList should have an even number of strings, unless the first one is a tag specifier ([a-zA-Z#.]+)
		/// </remarks>
		/// <param name="attribList">Array of key/value pairs as strings that represent attribute name, value</param>
		/// <param name="sh">SpecialHandling flag.  Denotes special cases for this tag.</param>
		protected void ProcessAttributes(string[] attribList, SpecialHandling sh)
		{
			//Debug.Assert(attribList != null, "ProcessAtrributes null == attribList");
			int count = attribList.Length;
			int start = 0;
			string firstAtt = attribList[0];
			//Debug.Assert(!String.IsNullOrEmpty(firstAtt), "firstAtt == null");
			if (count > 0 && firstAtt.IndexOfAny(tagDelims) != -1)
			{
				start = 1;//remove unparied parsable
				this.ParseTagName(firstAtt);
			}

			//Debug.Assert(
			//    (count - start) % 2 == 0
			//    , "You must pass an even number of attribute parameters"
			//);

			bool handleSpecialLink = false;
			if (sh != SpecialHandling.None)
			{
				handleSpecialLink = SpecialHandling.Link == (sh & SpecialHandling.Link);
			}

			for (int a = start; a < count; a += 2)
			{
				string name = attribList[a];
				string value;
				if (handleSpecialLink && LinkAttributeMap.Contains(name))
				{
					value = ProcessUrl(attribList[a + 1]);
				}
				else
				{
					value = attribList[a + 1];
				}

				//Debug.Assert(!String.IsNullOrEmpty(name), "ProcessAtrributes attribute name is null");
				if (!String.IsNullOrEmpty(name))
				{
					//Debug.WriteLineIf(String.IsNullOrEmpty(value), "ProcessAttributes: value for \"" + name + "\" is null.");

					SetAttribute(name, value);
				}
			}
		}
	}

	#endregion

	/// <summary>
	/// Special Handling flag.
	/// </summary>
	/// <remarks>
	/// This has a bug.  The enum is supposed to be a set of flags which are handled by bitmasking operations (and/or).
	/// The problem is that Suppress = 1, Link = 2, SelfClosing = 3.  So instead of those three being distinct flags, SelfClosing = Suppress + Link.
	/// There is no way to specify self closing without also specifying Suppress and Link.
	/// Apparently this is not a problem as of now, but hopefully one day SelfClosing will be set to 0x4 or some other distinct value.
	/// 
	/// Suppress		//don't do special processing
	///	Link			//process "~/" urls
	///	SelfClosing		//doesn't have children so never print the end tag ( <br/> )
	/// </remarks>
	[Flags]
	public enum SpecialHandling
	{
		/// <summary>
		/// 
		/// </summary>
		None = 0x00,
		/// <summary>
		/// Don't do any special processing
		/// </summary>
		Suppress = 0x01,
		/// <summary>
		/// Process "~/" urls
		/// </summary>
		Link = 0x02,
		/// <summary>
		/// Doesn't have children so never print the end tag ( <br/> )
		/// </summary>
		SelfClosing = 0x03
	}

	#region Static Members
	public partial class Element
	{
		/// <summary>
		/// Provides the default value of ~/ as resolved by VirtualPathUtility.ToAbsolute, but
		/// isn't marked readonly so that an application can change this out on application start.
		/// This value should be set once for the life of the application -- doing otherwise is
		/// not defined and will likely not work the way you desire (don't change it while the
		/// web site is running -- it's intended to be inject with something suitable 
		/// during unit testing).
		/// </summary>
		public static Func<string, string> ResolveUrlProvider = VirtualPathUtility.ToAbsolute; 

		/// <summary>
		/// Html tags special handling lookup
		/// </summary>
		protected static Dictionary<string, SpecialHandling> SpecialHandlingMap = new Dictionary<string, SpecialHandling>(StringComparer.OrdinalIgnoreCase) 
		{
			{"a",						SpecialHandling.Link},
			{"area",				SpecialHandling.SelfClosing | SpecialHandling.Link},
			{"blockquote",	SpecialHandling.Link},
			{"br",					SpecialHandling.SelfClosing},
			{"col",					SpecialHandling.SelfClosing},
			{"del",					SpecialHandling.Link},
			{"form",				SpecialHandling.Link},
			{"iframe",			SpecialHandling.Link},
			{"img",					SpecialHandling.SelfClosing | SpecialHandling.Link},
			{"input",				SpecialHandling.SelfClosing | SpecialHandling.Link},
			{"ins",					SpecialHandling.Link},
			{"hr",					SpecialHandling.SelfClosing},
			{"link",				SpecialHandling.SelfClosing | SpecialHandling.Link},
			{"meta",				SpecialHandling.SelfClosing},
			{"param",				SpecialHandling.SelfClosing},
			{"q",						SpecialHandling.Link},
			{"script",			SpecialHandling.Link}
		};

		/// <summary>
		/// These attributes usually contain urls.
		/// </summary>
		protected static HashSet<string> LinkAttributeMap = new HashSet<string>() 
		{
			"action","cite","href","rel","rev","src"
		};

		/// <summary>
		/// The Create method. TagName, Parameters. Parameters are listed in pairs
		/// </summary>
		/// <example>
		/// Element.Create("div","class","myclass","id","myid");
		/// </example>
		public static Element Create(string tag, params string[] attribList)
		{
			//Debug.Assert(!String.IsNullOrEmpty(tag));
			return Create(tag, attribList, SpecialHandling.None);
		}

		/// <summary>
		/// The Create method with special handling. TagName, Parameters. Parameters are listed in pairs
		/// </summary>
		/// <example>
		/// Element.Create("div","class","myclass","id","myid", SpecialHandling.SelfClosing);
		/// </example>
		public static Element Create(string tag, string[] attribList, SpecialHandling sh)
		{
			//Debug.Assert(!String.IsNullOrEmpty(tag));
			Element el = new Element(tag);

			if ((sh & SpecialHandling.Suppress) == 0)
			{
				if (SpecialHandlingMap.ContainsKey(el.TagName))
				{
					sh = SpecialHandlingMap[el.TagName];
				}
			}

			if (attribList != null && attribList.Length > 0)
			{
				el.ProcessAttributes(attribList, sh);
			}
			return el;
		}

		/// <summary>
		/// Special handling for urls; converts "~/" urls ToAbsolute 
		/// </summary>
		/// <param name="url">string, the URL to process</param>
		/// <returns></returns>
		protected static string ProcessUrl(string url)
		{
			//Debug.Assert(!String.IsNullOrEmpty(url));
			if (url.StartsWith("~/"))
			{
				var fn = ResolveUrlProvider ?? VirtualPathUtility.ToAbsolute;
				// ToAbsolute chokes on QueryStrings, so we have to remove it first
				return fn("~/") + url.Substring(2);
			}
			return url;
		}

		/// <summary>
		/// Adds text that will be encoded.
		/// </summary>
		/// <param name="text">string to be added</param>
		public static Text Text(string text)
		{
			return new Text(text, false);
		}

		/// <summary>
		/// Converts an array of strings into one string and adds it as a new text Node.  It will be HTML encoded.
		/// </summary>
		/// <param name="text">array of strings to be added</param>
		/// <returns></returns>
		public static Text Text(params string[] text)
		{
			return new Text(String.Join("", text), false);
		}

		/// <summary>
		/// Adds text that will NOT be htmlencoded.
		/// </summary>
		/// <param name="text">string to be added</param>
		public static Text Html(string text)
		{
			return new Text(text, true);
		}

		/// <summary>
		/// Converts an array of strings into one string and adds it as a new text Node.  Will not be HTML encoded.
		/// </summary>
		/// <param name="text">array of strings to be added</param>
		/// <returns></returns>
		public static Text Html(params string[] text)
		{
			return new Text(String.Join("", text), true);
		}
	}
	#endregion
}

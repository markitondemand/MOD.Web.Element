using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MOD.Web.Element
{

		#region Instance Members
		public partial class Element : Node, IRenderable
		{
			public Element(string tag)
			{
				ParseTagName(tag);
			}

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
			/// Attributes Dictionary<string,string>; case insensitive
			/// </summary>
			public Dictionary<string, string> Attributes
			{
				get
				{
					InitAttributes(); return attributes;
				}
			}

			protected virtual void InitAttributes()
			{
				if (null == attributes)
				{
					attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}
			}

			public string TagName;

			protected Fragment children = null;
			protected Dictionary<string, string> attributes = null;

			/// <summary>
			/// Adds just about anything to a node
			/// </summary>
			public Element Add(params object[] objectList)
			{
				return Add((IEnumerable<object>)objectList);
			}
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
			/// Adds nodes to a node
			/// </summary>
			public Element Add(params Node[] nodeList)
			{
				return Add((IEnumerable<Node>)nodeList);
			}
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
			/// Adds text that will be htmlencoded. use AddHtml() to leave text unencoded.
			/// </summary>
			public Element Add(params string[] textList)
			{
				return AddText(textList);
			}
			public Element Add(IEnumerable<string> textList)
			{
				return AddText(textList);
			}
			public Element AddText(params string[] textList)
			{
				return AddText((IEnumerable<string>)textList);
			}
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
			/// Adds text that will NOT be htmlencoded
			/// </summary>
			//had to compromise on the name. you can't have both a static and instance version of the same method
			public Element AddHtml(params string[] textList)
			{
				return AddHtml((IEnumerable<string>)textList);
			}
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
			/// Adds a class into the class list of a node
			/// </summary>
			/// <param name="className"></param>
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
			/// <example>
			/// div.myclass#myid
			/// </example>
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

			// Valid delimiters for ParseTagName
			protected static char[] tagDelims = { '.', '#' };

			/// <summary>
			/// Render this Element into html
			/// </summary>
			public override string ToString()
			{
				StringBuilder sb = new StringBuilder();
				ToString(sb);
				return sb.ToString();
			}

			/// <summary>
			/// Render this Element into html
			/// </summary>
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

		[Flags]
		public enum SpecialHandling
		{
			None = 0x00
			,
			Suppress = 0x01		//don't do special processing
				,
			Link = 0x02			//process "~/" urls
				, SelfClosing = 0x03		//doesn't have children so never print the end tag ( <br/> )
		}


		#region Static Members
		public partial class Element
		{
			//Html tags special handling lookup
			protected static Dictionary<string, SpecialHandling> SpecialHandlingMap = new Dictionary<string, SpecialHandling>(StringComparer.OrdinalIgnoreCase) 
			{
				 {"a"			,SpecialHandling.Link}
				,{"area"		,SpecialHandling.SelfClosing | SpecialHandling.Link}
				,{"blockquote"	,SpecialHandling.Link}
				,{"br"			,SpecialHandling.SelfClosing}
				,{"col"			,SpecialHandling.SelfClosing}
				,{"del"			,SpecialHandling.Link}
				,{"form"		,SpecialHandling.Link}
				,{"iframe"		,SpecialHandling.Link}
				,{"img"			,SpecialHandling.SelfClosing | SpecialHandling.Link}
				,{"input"		,SpecialHandling.SelfClosing | SpecialHandling.Link}
				,{"ins"			,SpecialHandling.Link}
				,{"hr"			,SpecialHandling.SelfClosing}
				,{"link"		,SpecialHandling.SelfClosing | SpecialHandling.Link}
				,{"meta"		,SpecialHandling.SelfClosing}
				,{"param"		,SpecialHandling.SelfClosing}
				,{"q"			,SpecialHandling.Link}
				,{"script"		,SpecialHandling.Link}
			};

			//These attributes usually contain urls
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

			//special handling for urls. converts "~/" urls ToAbsolute
			protected static string ProcessUrl(string url)
			{
				//Debug.Assert(!String.IsNullOrEmpty(url));
				if (url.StartsWith("~/"))
				{
					// ToAbsolute chokes on QueryStrings, so we have to remove it first
					return VirtualPathUtility.ToAbsolute("~/") + url.Substring(2);
				}
				return url;
			}

			/// <summary>
			/// Adds text that will be htmlencoded
			/// </summary>
			public static Text Text(string text)
			{
				return new Text(text, false);
			}
			public static Text Text(params string[] text)
			{
				return new Text(String.Join("", text), false);
			}
			/// <summary>
			/// Adds text that will NOT be htmlencoded
			/// </summary>
			public static Text Html(string text)
			{
				return new Text(text, true);
			}
			public static Text Html(params string[] text)
			{
				return new Text(String.Join("", text), true);
			}
		}
		#endregion

}

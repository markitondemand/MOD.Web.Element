using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MOD.Web.Element
{
	/// <summary>
	/// This embodies all of the supplemental methods for Element. Methods allow chaining when appropriate.
	/// </summary>
	public static class ElementExtensions
	{
		/// <summary>
		/// Add a single Node to a fragment
		/// </summary>
		public static E Add<E>(this E self, INode node) where E : IFragment
		{
			if (node != null) {
				self.Children.Add(node);
			}
			return self;
		}

		/// <summary>
		/// Add multiple nodes to a fragment
		/// </summary>
		public static E Add<E>(this E self, params INode[] list) where E : IFragment
		{
			return Add<E>(self,(IEnumerable<INode>)list);
		}

		/// <summary>
		/// Add multiple nodes to a fragment
		/// </summary>
		public static E Add<E>(this E self, IEnumerable<INode> list) where E : IFragment
		{
			if (list != null)
			{
				foreach(INode node in list)
				{
					if (node != null) {
						self.Children.Add(node);
					}
				}
			}
			return self;
		}

		/// <summary>
		/// Adds text that will be htmlencoded. Use AddHtml() to leave text unencoded.
		/// </summary>
		public static E Add<E>(this E self, IConvertible text) where E : IFragment
		{
			return AddText<E>(self,text);
		}

		/// <summary>
		/// Add a collection of strings as text nodes that will be htmlencoded.
		/// </summary>
		public static E Add<E>(this E self, params IConvertible[] list) where E: IFragment
		{
			return Add<E>(self,(IEnumerable<IConvertible>)list);
		}

		/// <summary>
		/// Add a collection of strings as text nodes that will be htmlencoded.
		/// </summary>
		public static E Add<E>(this E self, IEnumerable<IConvertible> list) where E : IFragment
		{
			if (list != null)
			{
				foreach(IConvertible text in list)
				{
					if (text != null) {
						self.Children.Add(new TextNode(text));
					}
				}
			}
			return self;
		}

		/// <summary>
		/// Adds text that will be htmlencoded. Use AddHtml() to leave text unencoded.
		/// </summary>
		public static E AddText<E>(this E self, IConvertible text) where E : IFragment
		{
			self.Children.Add(new TextNode(text));
			return self;
		}

		/// <summary>
		/// Adds text that will NOT be htmlencoded.
		/// </summary>
		public static E AddHtml<E>(this E self, IConvertible text) where E : IFragment
		{
			self.Children.Add(new TextNode(true,text));
			return self;
		}

		/// <summary>
		/// Sets an attribute to the given value. If the value is null the attribute is not set.
		/// </summary>
		/// <remarks>
		/// Since the attribute will not be set if the value is null, you can conditionally apply attributes inline 
		/// with something like Element.Create("option").SetAttribute("selected", value == selectedValue ? "selected" : null).
		/// </remarks>
		public static E SetAttribute<E>(this E self, string name, IConvertible value) where E : IElement
		{
			if (!String.IsNullOrEmpty(name) && value != null) {
				self.Attributes[name] = value.ToString();
			}
			return self;
		}
		/// <summary>
		/// Adds/Sets multiple attributes on the this node. If the value is empty the attribute is not set. You must pass an event number of attribute/value pairs
		/// </summary>
		public static E SetAttribute<E>(this E self, params IConvertible[] attributes) where E : IElement
		{
			if (attributes != null)
			{
				if (attributes.Length % 2 != 0) {
					throw new ArgumentException("You must pass an even number of attributes");
				}
				for(int a=0; a<attributes.Length; a+=2)
				{
					IConvertible name = attributes[a];
					IConvertible val = attributes[a+1];
					SetAttribute(self,name.ToString(),val); //need the tostring so it doesn't infinite loop
				}
			}
			return self;
		}
		/// <summary>
		/// Returns the attribute value for a given key. Returns an empty string if the key does not exist.
		/// </summary>
		public static string GetAttribute(this IElement self, string key)
		{
			string val;
			if (self == null || self.Attributes == null || !self.Attributes.TryGetValue(key, out val)) {
				val = "";
			}
			return val;
		}
		/// <summary>
		/// Adds a class or classes into the class list of a node.
		/// In the attribute dictionary there is only one entry for "class".
		/// This is a string that has all the class names separated by spaces.
		/// </summary>
		public static E AddClass<E>(this E self, string @class) where E : IElement
		{
			if (!String.IsNullOrEmpty(@class)) {
				self.ParseTag("."+@class);
			}
			return self;
		}
		/// <summary>
		/// Adds a class or classes into the class list of a node.
		/// In the attribute dictionary there is only one entry for "class".
		/// This is a string that has all the class names separated by spaces.
		/// </summary>
		public static E AddClass<E>(this E self, params string[] @class) where E : IElement
		{
			if (@class != null) {
				foreach(string c in @class) {
					if (!String.IsNullOrEmpty(c)) {
						self.ParseTag("."+c);
					}
				}
			}
			return self;
		}
		/// <summary>
		/// Returns true if the class attribute contains a given class. Uses a case-sensitive compare by default.
		/// </summary>
		public static bool HasClass<E>(this E self, string @class, StringComparison compare = StringComparison.CurrentCulture) where E : IElement
		{
			string test = " "+(@class??"").Trim()+" ";
			if (self != null && !String.IsNullOrEmpty(test))
			{
				string present = self.GetAttribute("class") ?? "";
				bool good = (" "+present+" ").IndexOf(test,compare) != -1;
				return good;
			}
			return false;
		}
	}
}

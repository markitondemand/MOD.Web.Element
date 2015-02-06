using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MOD.Web.Element
{
	/// <summary>
	/// Element 'static' usage class. This is here to allow the Element.Create(...) and usage pattern.
	/// It's still an instance so that it can be extended, but all methods should be static
	/// </summary>
	public class Element
	{
		/// <summary>
		/// protected constructor so that it can't be instantiated publicly. Do the same for any extending classes
		/// </summary>
		protected Element() {}
		/// <summary>
		/// Shortcut for creating elements.
		/// </summary>
		public static IElement Create(string tag, params IConvertible[] attr)
		{
			return _provider.Create(tag,attr);
		}

		/// <summary>
		/// Shortcut for creating text nodes.
		/// </summary>
		public static IText Text(IConvertible text)
		{
			return _provider.Text(text);
		}

		/// <summary>
		/// Shortcut for creating text nodes with raw html.
		/// </summary>
		public static IText Html(IConvertible html)
		{
			return _provider.Html(html);
		}

		/// <summary>
		/// Shortcut for creating a fragment.
		/// </summary>
		public static IFragment Fragment()
		{
			return _provider.Fragment();
		}

		/// <summary>
		/// Shortcut for creating a StreamNode.
		/// </summary>
		public static INode Stream(TextReader tr)
		{
			return _provider.Stream(tr);
		}

		/// <summary>
		/// Shortcut for creating a StreamNode.
		/// </summary>
		public static INode Stream(StreamNode.WriterDelegate func)
		{
			return _provider.Stream(func);
		}

		//each AppDomain gets a copy of the static variables
		private static IElementStaticProvider _provider = new DefaultProvider();
		
		/// <summary>
		/// Use this to override the default IElementStaticProvider (it's static so probably should only be set once on app startup)
		/// </summary>
		public static IElementStaticProvider StaticInstance { get {
			return _provider; //leaving out null checks because failures need to be exposed
		} set {
			_provider = value;
		}}

		private sealed class DefaultProvider : IElementStaticProvider
		{
			public IElement Create(string tag, params IConvertible[] attr) {
				return new ElementNode(tag).SetAttribute(attr);
			}
			public IText Text(IConvertible text) {
				return new TextNode(text);
			}
			public IText Html(IConvertible html) {
				return new TextNode(true,html);
			}
			public INode Stream(TextReader tr) {
				return new StreamNode(tr);
			}
			public INode Stream(StreamNode.WriterDelegate func) {
				return new StreamNode(func);
			}
			public IFragment Fragment() {
				return new FragmentNode();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MOD.Web.Element
{
	/// <summary>
	/// An interface that captures the 'static' usage pattern.
	/// </summary>
	public interface IElementStaticProvider
	{
		/// <summary>
		/// Shortcut for creating elements.
		/// </summary>
		IElement Create(string tag, params IConvertible[] attr);

		/// <summary>
		/// Shortcut for creating text nodes.
		/// </summary>
		IText Text(IConvertible text);

		/// <summary>
		/// Shortcut for creating html nodes.
		/// </summary>
		IText Html(IConvertible html);

		/// <summary>
		/// Shortcut for creating a StreamNode.
		/// </summary>
		INode Stream(TextReader tr);

		/// <summary>
		/// Shortcut for creating a StreamNode.
		/// </summary>
		INode Stream(StreamNode.WriterDelegate func);

		/// <summary>
		/// Shortcut for createing a fragment.
		/// </summary>
		IFragment Fragment();
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MOD.Web.Element
{
	//.Net 4.0 added System.Web.IHtmlString to support razor views,
	//  but I want to support .Net 3.5 without having to put in tons of compiler directives 

	#if NET35
	// Summary:
	//     Represents an HTML-encoded string that should not be encoded again.
	public interface IHtmlString
	{
		// Summary:
		//     Returns an HTML-encoded string.
		//
		// Returns:
		//     An HTML-encoded string.
		string ToHtmlString();
	}
	#endif
}

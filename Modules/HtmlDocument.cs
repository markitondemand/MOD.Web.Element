using MOD.Web.Element.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOD.Web.Element.Modules
{
	public abstract class HtmlDocument : ViewModule
	{
		public virtual Element Body { get; set; }
		public virtual Element Head { get; set; }

		public virtual string Title { get; set; }
		public virtual string Language { get; set; }
		public virtual string Charset { get; set; }

		public virtual IRenderable PageView { get; set; }

		/// <summary>
		/// Setups the parts of a default page.
		/// </summary>
		public HtmlDocument()
		{
			Container = Element.Create("html");
			Head = Element.Create("head");
			Body = Element.Create("body");

			Container.Add(Head, Body);
		}
	}
}

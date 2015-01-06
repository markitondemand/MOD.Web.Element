
namespace MOD.Web.Element.Modules
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class HtmlDocument : ViewModule
	{
		/// <summary>
		/// 
		/// </summary>
		public virtual Element Body { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public virtual Element Head { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string Title { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string Language { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string Charset { get; set; }

		/// <summary>
		/// 
		/// </summary>
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

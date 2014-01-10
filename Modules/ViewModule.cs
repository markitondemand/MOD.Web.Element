using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOD.Web.Element.Module
{
	public class ViewModule : IRenderable
	{
		protected Element _Container = null;
		/// <summary>
		/// The Element that represents markup for the module.
		/// </summary>
		protected Element Container
		{
			get { return _Container = _Container ?? CreateContainer(); }
			set { _Container = value; }
		}
		public virtual Node Render() { return Container; }

		/// <summary>
		/// Implement this method to override control the Container Element that is created for each instance
		/// </summary>
		/// <returns>Container Element</returns>
		protected virtual Element CreateContainer()
		{
			return Element.Create("div");
		}
	}
}

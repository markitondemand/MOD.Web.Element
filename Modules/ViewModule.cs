
namespace MOD.Web.Element.Modules
{
	/// <summary>
	/// 
	/// </summary>
	public class ViewModule : IRenderable
	{
		/// <summary>
		/// 
		/// </summary>
		protected Element _Container = null;

		/// <summary>
		/// The Element that represents markup for the module.
		/// </summary>
		protected Element Container
		{
			get { return _Container = _Container ?? CreateContainer(); }
			set { _Container = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
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

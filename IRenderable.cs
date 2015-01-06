
namespace MOD.Web.Element
{
	/// <summary>
	/// This interface can be used by any class/object that can be rendered
	/// to a Node.  Element itself knows to call this function on an 
	/// instance if it implements this interface.
	/// </summary>
	public interface IRenderable
	{
		/// <summary>
		/// This interface can be used by any class/object that can be rendered
		/// to a Node.  Element itself knows to call this function on an 
		/// instance if it implements this interface.
		/// </summary>
		/// <returns>An instance of Node</returns>
		Node Render();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOD.Web.Element
{
	/// <summary>
	/// Fragment: a collection on nodes. Use this when you don't have a parent element
	/// </summary>
	public class Fragment : List<Node>, INode
	{
		new public Fragment Add(Node item)
		{
			base.Add(item);
			return this;
		}
		/// <summary>
		/// Adds just about anything to a node
		/// </summary>
		public Fragment Add(params object[] objectList)
		{
			if (objectList == null)
			{
				//Debug.WriteLine("Add null == objectList");
				return this;
			}
			foreach (object obj in objectList)
			{
				if (obj == null)
				{
					//Debug.WriteLine("Add null = obj");
					continue;
				}
				if (obj is string)
				{
					Add(Element.Text(obj as string));
				}
				else if (obj is IEnumerable<string>)
				{
					Add(obj);
				}
				else if (obj is Node)
				{
					Add(obj as Node);
				}
				else if (obj is IEnumerable<Node>)
				{
					Add(obj);
				}
				else if (obj is IEnumerable<Element>)
				{
					Add(obj);
				}
				else if (obj is IEnumerable<object>)
				{
					Add(obj);
				}
				else if (obj is long || obj is int || obj is double)
				{
					Add(obj.ToString());
				}
				else
				{
					throw new Exception(
						string.Format("Unsupported type yo! :{0}, type: {1}"
						, obj.ToString(), obj.GetType().Name));
				}
			}
			return this;
		}

		new public Fragment AddRange(IEnumerable<Node> collection)
		{
			base.AddRange(collection);
			return this;
		}

		public void ToString(StringBuilder sb)
		{
			foreach (Node node in this)
			{
				node.ToString(sb);
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			ToString(sb);
			return sb.ToString();
		}
	}
}

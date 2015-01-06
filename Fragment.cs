using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MOD.Web.Element
{
	/// <summary>
	/// Fragment: a collection on nodes. Use this when you don't have a parent element
	/// </summary>
	public class Fragment : Node, IList<Node>, IRenderable
	{
		#region New Fragment Members
		/// <summary>
		/// 
		/// </summary>
		public List<Node> Children
		{
			get { return _Children ?? (_Children = new List<Node>()); }
			set { _Children = value; }
		}
		private List<Node> _Children;
		#endregion

		#region Original Fragment Members
		/// <summary>
		/// Adds a Node to the fragment.
		/// </summary>
		/// <param name="item">The Node to Add</param>
		/// <returns></returns>
		public Fragment Add(Node item)
		{
			Children.Add(item);
			return this;
		}

		/// <summary>
		/// Add a list of objects of any type to the fragment
		/// Supported types are:
		/// string, long, int, double
		/// IEnumerable&lt;string&gt;
		/// Node
		/// IEnumerable&lt;Node&gt;
		/// IEnumerable&lt;Element&gt;
		/// IEnumerable&lt;object&gt;
		/// </summary>
		public Fragment Add(params object[] objectList)
		{
			if (objectList == null)
			{
				Debug.WriteLine("Add null == objectList");
				return this;
			}

			foreach (object obj in objectList)
			{
				if (obj == null)
				{
					Debug.WriteLine("Add null = obj");
					continue;
				}
				if (obj is string)
				{
					Add(Element.Text(obj as string));
				}
				else if (obj is IEnumerable)
				{
					foreach (var child in (IEnumerable)obj)
					{
						Add(child);
					}
				}
				else if (obj is Node)
				{
					Add(obj as Node);
				}
				else if (obj is long || obj is int || obj is double)
				{
					Add(obj.ToString());
				}
				else
				{
					throw new Exception(
						string.Format("Unsupported type bro! :{0}, type: {1}"
						, obj.ToString(), obj.GetType().Name));
				}
			}

			return this;
		}

		/// <summary>
		/// Adds the collection to the end of the Fragment
		/// </summary>
		/// <param name="collection">Enumerable set of nodes to add</param>
		/// <returns></returns>
		public Fragment AddRange(IEnumerable<Node> collection)
		{
			Children.AddRange(collection);
			return this;
		}

		/// <summary>
		/// Take each object in this node, convert it to a string, and add it to the StringBuilder
		/// </summary>
		/// <param name="sb">The StringBuilder to which all the stringified objects will be added</param>
		public override void ToString(StringBuilder sb)
		{
			foreach (Node node in Children)
			{
				node.ToString(sb);
			}
		}

		/// <summary>
		/// Create a StringBuilder, add all the contents of the Fragment to that StringBuilder, and retrieve the built string
		/// </summary>
		/// <returns>One string containing the string values of all the objects in the fragment</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			ToString(sb);
			return sb.ToString();
		}
		#endregion

		#region List<T> Members
		/// <summary>
		/// Inserts the elements of a collection into the List&lt;T&gt; at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the new elements should be inserted.</param>
		/// <param name="collection">The collection whose elements should be inserted into the List&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
		public void InsertRange(int index, IEnumerable<Node> collection)
		{
			Children.InsertRange(index, collection);
		}
		#endregion

		#region IList<T> Members
		/// <summary>
		/// Adds an item to the ICollection&lt;T&gt;.
		/// </summary>
		/// <param name="item">The object to add to the ICollection&lt;T&gt;.</param>
		void ICollection<Node>.Add(Node item)
		{
			Children.Add(item);
		}
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		public IEnumerator<Node> GetEnumerator()
		{
			return Children.GetEnumerator();
		}
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		/// <summary>
		/// Removes all items from the ICollection&lt;T&gt;.
		/// </summary>
		public void Clear()
		{
			Children.Clear();
		}
		/// <summary>
		/// Gets the number of elements contained in the ICollection&lt;T&gt;.
		/// </summary>
		/// <param name="item">The object to locate in the ICollection&lt;T&gt;.</param>
		/// <returns>true if item is found in the ICollection&lt;T&gt;; otherwise, false.</returns>
		public bool Contains(Node item)
		{
			return Children.Contains(item);
		}
		/// <summary>
		/// Gets the number of elements contained in the ICollection&lt;T&gt;.
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection&lt;T&gt;. The Array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		public void CopyTo(Node[] array, int arrayIndex)
		{
			Children.CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// Gets the number of elements contained in the ICollection&lt;T&gt;.
		/// </summary>
		public int Count
		{
			get { return Children.Count; }
		}
		/// <summary>
		/// Determines the index of a specific item in the IList&lt;T&gt;.
		/// </summary>
		/// <param name="item">The object to locate in the IList&lt;T&gt;.</param>
		/// <returns>The object to locate in the IList&lt;T&gt;.</returns>
		public int IndexOf(Node item)
		{
			return Children.IndexOf(item);
		}
		/// <summary>
		/// Inserts an item to the IList&lt;T&gt; at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert into the IList&lt;T&gt;.</param>
		public void Insert(int index, Node item)
		{
			Children.Insert(index, item);
		}
		/// <summary>
		/// Gets a value indicating whether the ICollection&lt;T&gt; is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the ICollection&lt;T&gt;.
		/// </summary>
		/// <param name="item">The object to remove from the ICollection&lt;T&gt;.</param>
		/// <returns>true if item was successfully removed from the ICollection&lt;T&gt;; otherwise, false. This method also returns false if item is not found in the original ICollection&lt;T&gt;.</returns>
		public bool Remove(Node item)
		{
			return Children.Remove(item);
		}
		/// <summary>
		/// Removes the IList&lt;T&gt; item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		public void RemoveAt(int index)
		{
			Children.RemoveAt(index);
		}
		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		public Node this[int index]
		{
			get { return Children[index]; }
			set { Children[index] = value; }
		}
		#endregion

		#region IRenderable Members
		/// <summary>
		/// Renders a Fragment to a Node.
		/// </summary>
		/// <returns>A Node instance.</returns>
		public Node Render()
		{
			return this;
		}
		#endregion
	}
}

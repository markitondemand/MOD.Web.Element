using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOD.Web.Element.Tests
{
	[TestFixture]
	[Category("Fragment")]
	public class FragmentTests
	{
		[Test]
		public void Add_Null_Objects()
		{
			var d = new FragmentNode();

			IEnumerable<INode> items = null;

			d.Add(items);

			Assert.AreEqual("", d.ToString());
		}

		[Test]
		public void Return_From_Add_Null_Objects()
		{
			var d = new FragmentNode();

			IEnumerable<INode> items = null;

			var a = d.Add(items);

			Assert.AreEqual(a, d);
		}

		[Test]
		public void Add_String_And_Null_Items()
		{
			var d = new FragmentNode();

			IEnumerable<string> items =
				new List<string>
				{
					null,
					"here",
					null
				};

			d.Add(items);

			Assert.AreEqual("here", d.ToString());
		}

		[Test]
		public void Add_Multi_Items()
		{
			var d = new FragmentNode();

			IEnumerable<INode> items =
				new List<INode>
				{
					null,
					Element.Text("here"),
					null,
					Element.Create("div").Add("my-div"),
					null,
					Element.Create("a").Add(Element.Create("span").Add("click me")),
					null
				};

			d.Add(items);

			Assert.AreEqual("here<div>my-div</div><a><span>click me</span></a>", d.ToString());
		}

		[Test]
		public void Add_Null_Object_List()
		{
			INode[] objs = null;

			var f = new FragmentNode();

			f.Add(objs);

			Assert.AreEqual(0, f.Children.Count);
		}

		[Test]
		public void Add_Primitives()
		{
			var f = new FragmentNode();

			//f.Add(1, 2.0, long.MaxValue, false, true, DateTime.MinValue);
			f.Add(1, 2.0, long.MaxValue);

			Assert.AreEqual(3, f.Children.Count);
		}

		[Test]
		public void Insert_Item()
		{
			var f = new FragmentNode().Add(
				"1",
				"2",
				"3"
			);
			f.Add(Element.Text("a"));

			Assert.AreEqual("123a", f.ToString());
		}

		[Test]
		public void Nested_Fragments()
		{
			var f = new FragmentNode().Add(
				Element.Text("a"),
				new FragmentNode().Add(
					"b",
					"c",
					"d"
				)
			);

			Assert.AreEqual("abcd", f.ToString());
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace MOD.Web.Element.Tests
{
	[TestFixture]
	[Category("Fragment")]
	public class FragmentTests
	{
		[Test]
		public void Add_Null_Objects()
		{
			var d = new Fragment();

			IEnumerable<object> items = null;

			d.Add(items);

			Assert.AreEqual("", d.ToString());
		}

		[Test]
		public void Return_From_Add_Null_Objects()
		{
			var d = new Fragment();

			IEnumerable<object> items = null;

			var a = d.Add(items);

			Assert.AreEqual(a, d);
		}

		[Test]
		public void Add_String_And_Null_Items()
		{
			var d = new Fragment();

			IEnumerable<object> items =
				new List<object>
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
			var d = new Fragment();

			IEnumerable<object> items =
				new List<object>
				{
					null,
					"here",
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
		[ExpectedException(typeof(Exception))]
		public void Add_Unsupported_Type()
		{
			var d = new Fragment();

			var buff = new StringBuilder();

			d.Add(buff);
		}

		[Test]
		public void Add_Null_Object_List()
		{
			object[] objs = null;

			var f = new Fragment();

			f.Add(objs);

			Assert.AreEqual(0, f.Count);
		}

		[Test]
		public void Add_Primitives()
		{
			var f = new Fragment();

			//f.Add(1, 2.0, long.MaxValue, false, true, DateTime.MinValue);
			f.Add(1, 2.0, long.MaxValue);

			Assert.AreEqual(3, f.Count);
		}

		[Test]
		public void Insert_Item()
		{
			var f = new Fragment()
			{
				"1",
				"2",
				"3"
			};
			f.Insert(1, Element.Text("a"));

			Assert.AreEqual("1a23", f.ToString());
		}

		[Test]
		public void Insert_Range()
		{
			var f = new Fragment()
			{
				"1",
				"2",
				"3"
			};
			f.InsertRange(1, new List<Node>
			{
				Element.Text("a"),
				Element.Text("b"),
				Element.Text("c")
			});

			Assert.AreEqual("1abc23", f.ToString());
		}

		[Test]
		public void Nested_Fragments()
		{
			var f = new Fragment
			{
				"a",
				new Fragment
				{
					"b",
					"c",
					"d"
				}
			};

			Assert.AreEqual("abcd", f.ToString());
		}

		[Test]
		[Ignore("this should be fixed in a separate branch")]
		public void Add_Null_Range()
		{
			var f = new Fragment();
			IEnumerable<Node> nodes = null;

			f.AddRange(nodes);

			//Expect(f.Count, Is.EqualTo(0));
			Assert.AreEqual(f.Count, 0);
		}

		[Test]
		[Ignore("this should be fixed in a separate branch")]
		public void Add_Null_Range_To_String()
		{
			var f = new Fragment();
			IEnumerable<Node> nodes = null;

			f.AddRange(nodes);

			//Expect(f.ToString(), Is.EqualTo(""));
			Assert.AreEqual(f.ToString(), "");
		}

		[Test]
		[Ignore("this should be fixed in a separate branch")]
		public void Add_Range_With_Null_Nodes()
		{
			var f = new Fragment();
			IEnumerable<Node> nodes =
				new List<Node>
				{
					Element.Create("div"),
					null,
					Element.Html("S & P"),
					null,
					Element.Text("S & P")
				};

			f.AddRange(nodes);

			//Expect(f.ToString(), Is.EqualTo("<div></div>S & PS &amp; P"));
			Assert.AreEqual(f.ToString(), "<div></div>S & PS &amp; P");
		}

		[Test]
		[Ignore("this should be fixed in a separate branch")]
		public void Set_Index_With_Null_Nodes()
		{
			var f = new Fragment();
			IEnumerable<Node> nodes = new List<Node>
			{
				Element.Create("div"),
				null,
				Element.Html("S & P"),
				null,
				Element.Text("S & P")
			};

			f.AddRange(nodes);

			f[1] = null;

			//Expect(f.ToString(), Is.EqualTo("<div></div>S &amp; P"));
			Assert.AreEqual(f.ToString(), "<div></div>S &amp; P");
		}
	}
}

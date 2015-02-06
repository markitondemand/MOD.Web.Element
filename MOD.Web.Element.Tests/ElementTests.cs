using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOD.Web.Element.Tests
{
	[TestFixture]
	[Category("Element")]
	public class ElementTests
	{
		#region Add
		[Test]
		public void Return_From_Add_Null_Objects()
		{
			var d = Element.Create("div");

			IEnumerable<IConvertible> items = null;

			var a = d.Add(items);

			Assert.AreEqual(a, d);
		}

		[Test]
		public void Add_String_And_Null_Items()
		{
			var d = Element.Create("div");

			IEnumerable<IConvertible> items =
				new List<IConvertible>
				{
					null,
					"here",
					null
				};

			d.Add(items);

			Assert.AreEqual("<div>here</div>", d.ToString());
		}

		[Test]
		public void Add_Multi_Items()
		{
			var d = Element.Create("div");

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

			Assert.AreEqual("<div>here<div>my-div</div><a><span>click me</span></a></div>", d.ToString());
		}

		[Test]
		public void Add_Enumerables_Inside_Of_Enumerables()
		{
			var d = Element.Create("div");

			var nodes =
				new List<INode>
				{
					Element.Create("div"),
					Element.Fragment().Add(
						Element.Create("a")
					)
				};

			d.Add(nodes);

			Assert.AreEqual("<div><div></div><a></a></div>", d.ToString());
		}

		[Test]
		public void Add_String_With_Html_Entities()
		{
			var d = Element.Create("div");

			d.Add("<&>\"");

			Assert.AreEqual("<div>&lt;&amp;&gt;&quot;</div>", d.ToString());
		}

		[Test]
		public void Add_Nullable_Int()
		{
			var d = new ElementNode("div");
			int? n = null;

			d.Add(n);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_Nullable_Double()
		{
			var d = new ElementNode("div");
			double? n = null;

			d.Add(n);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_Nullable_Long()
		{
			var d = new ElementNode("div");
			long? n = null;

			d.Add(n);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_Nullable_Boolean()
		{
			var d = new ElementNode("div");
			bool? n = null;

			d.Add(n);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_Int()
		{
			var d = new ElementNode("div");
			int? n = int.MaxValue;

			d.Add(n);

			Assert.AreEqual(string.Format("<div>{0}</div>", int.MaxValue), d.ToString());
		}

		[Test]
		public void Add_Double()
		{
			var d = new ElementNode("div");
			double? n = double.MaxValue;

			d.Add(n);

			Assert.AreEqual(string.Format("<div>{0}</div>", double.MaxValue), d.ToString());
		}

		[Test]
		public void Add_Long()
		{
			var d = new ElementNode("div");
			long? n = long.MaxValue;

			d.Add(n);

			Assert.AreEqual(string.Format("<div>{0}</div>", long.MaxValue), d.ToString());
		}

		[Test]
		
		public void Add_Boolean()
		{
			var d = new ElementNode("div");
			bool? n = true;

			d.Add(n);

			Assert.AreEqual(d.ToString(), "<div>True</div>");
		}

		[Test]
		public void Add_Nullable_DataTime()
		{
			var d = new ElementNode("div");
			DateTime? n = null;

			d.Add(n);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		
		public void Add_DataTime()
		{
			var d = new ElementNode("div");
			DateTime? n = DateTime.MinValue;

			d.Add(n);

			Assert.AreEqual(d.ToString(), string.Format("<div>{0}</div>", DateTime.MinValue));
		}

		[Test]
		public void Result_of_Add_Null_Nodes_IEnumerable()
		{
			var d = Element.Create("div");

			var b = d.Add((IEnumerable<INode>)null);

			Assert.AreEqual(d, b);
		}

		[Test]
		public void Add_Null_Nodes_IEnumerable()
		{
			var d = Element.Create("div");

			d.Add((IEnumerable<INode>)null);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_Empty_IEnumerable_Of_Nodes()
		{
			var d = Element.Create("div");

			var nodes = new List<INode>();

			d.Add(nodes);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_IEnumerable_Of_Tags_Nodes()
		{
			var d = Element.Create("div");

			var nodes = new List<INode> { Element.Create("a") };

			d.Add(nodes);

			Assert.AreEqual("<div><a></a></div>", d.ToString());
		}

		[Test]
		public void Add_IEnumerable_Of_Tags_And_Text_Nodes()
		{
			var d = Element.Create("div");

			var nodes = new List<INode> { Element.Create("br"), new TextNode("stuff") };

			d.Add(nodes);

			Assert.AreEqual("<div><br/>stuff</div>", d.ToString());
		}

		[Test]
		public void Add_IEnumerable_Nested_Nodes()
		{
			var d = Element.Create("div");

			var nodes =
				new List<INode>
				{
					Element.Create("br"),
					new TextNode("stuff"),
					null,
					Element.Create("div").Add("here"),
					null
				};

			d.Add(nodes);

			Assert.AreEqual("<div><br/>stuff<div>here</div></div>", d.ToString());
		}

		[Test]
		public void Add_Fragment()
		{
			var d = Element.Create("div").Add(
				new FragmentNode().Add(
					Element.Text("Test"),
					Element.Create("h1")
				)
			);

			Assert.AreEqual("<div>Test<h1></h1></div>", d.ToString());
		}

		[Test]
		public void Add_Fragment_And_Element()
		{
			var d = Element.Create("div").Add(
				new FragmentNode().Add(
					Element.Text("Test"),
					Element.Create("h1")
				),
				Element.Create("div")
			);

			Assert.AreEqual("<div>Test<h1></h1><div></div></div>", d.ToString());
		}

		[Test]
		public void Add_Nested_Fragments()
		{
			var d = Element.Create("div").Add(
				new FragmentNode().Add(
					Element.Text("Test"),
					Element.Create("h1"),
					new FragmentNode().Add(
						Element.Text("InnerTest"),
						Element.Create("h2")
					)
				)
			);

			Assert.AreEqual("<div>Test<h1></h1>InnerTest<h2></h2></div>", d.ToString());
		}

		[Test]
		public void Add_And_Then_Modify_Fragment()
		{
			var f = new FragmentNode().Add(
				"Test"
			);

			var d = Element.Create("div").Add(f);

			f.Add("ing");

			Assert.AreEqual("<div>Testing</div>", d.ToString());
		}

		[Test]
		public void Add_Multiple_And_Then_Modify_Fragment()
		{
			var f = new FragmentNode().Add(
				"Test"
			);

			var d = Element.Create("div").Add(
				f,
				Element.Create("h1")
			);

			f.Add("ing");

			Assert.AreEqual("<div>Testing<h1></h1></div>", d.ToString());
		}
		#endregion

		#region AddClass
		[Test]
		public void Add_A_Class()
		{
			var d = Element.Create("div");
			d.AddClass("a");

			Assert.AreEqual("<div class=\"a\"></div>", d.ToString());
		}

		[Test]
		public void Add_Classes()
		{
			var d = Element.Create("div");
			d.AddClass("a", "b", "c");

			Assert.AreEqual("<div class=\"a b c\"></div>", d.ToString());
		}

		[Test]
		public void Add_Classes_With_A_Null()
		{
			var d = Element.Create("div");
			d.AddClass("a", "b", null, "c");

			Assert.AreEqual("<div class=\"a b c\"></div>", d.ToString());
		}

		[Test]
		public void Add_Classes_Passing_Null()
		{
			var d = Element.Create("div");
			d.AddClass((string)null);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_A_Class_When_Classes_Already_Present()
		{
			var d = Element.Create("div.first-class");
			d.AddClass("a");

			Assert.AreEqual("<div class=\"first-class a\"></div>", d.ToString());
		}

		[Test]
		public void Add_Classes_When_Classes_Already_Present()
		{
			var d = Element.Create("div.first-class");
			d.AddClass("a", "b", "c");

			Assert.AreEqual("<div class=\"first-class a b c\"></div>", d.ToString());
		}

		[Test]
		public void Add_Classes_With_A_Null_When_Classes_Already_Present()
		{
			var d = Element.Create("div.first-class");
			d.AddClass("a", "b", null, "c");

			Assert.AreEqual("<div class=\"first-class a b c\"></div>", d.ToString());
		}

		[Test]
		public void Add_Classes_Passing_Null_When_Classes_Already_Present()
		{
			var d = Element.Create("div.first-class");
			d.AddClass((string)null);

			Assert.AreEqual("<div class=\"first-class\"></div>", d.ToString());
		}
		#endregion

		#region AddHtml
		[Test]
		public void AddHtml_Html_Entities()
		{
			var d = Element.Create("div");

			d.AddHtml("<&>\"");

			Assert.AreEqual("<div><&>\"</div>", d.ToString());
		}
		#endregion

		#region Create
		[Test]
		public void Create_Div()
		{
			var e = Element.Create("div");

			Assert.AreEqual("<div></div>", e.ToString());
		}

		[Test]
		public void Create_Nested_Div()
		{
			var e = Element.Create("div").Add(Element.Create("div"));

			Assert.AreEqual("<div><div></div></div>", e.ToString());
		}

		[Test]
		public void Create_Div_With_Attr()
		{
			var e = Element.Create("div", "data-name", "testing");

			//Assert.AreEqual("<div data-name=\"testing\"></div>", e.ToString());
		}

		//[Test]
		//public void Create_Div_With_Attr_And_Id()
		//{
		//	var e = Element.Create("div#my-div", "data-name", "testing");

		//	Assert.AreEqual("<div id=\"my-div\" data-name=\"testing\"></div>", e.ToString());
		//}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_Div_With_Unpaired_Name_Value()
		{
			var e = Element.Create("div", "data-name");
		}

		// For Chrome Bug: 
		[Test]
		public void Create_Empty_Source_Tag_Should_Close()
		{
			var a = Element.Create("source");

			Assert.AreEqual("<source></source>", a.ToString());
		}

		[Test]
		public void Create_Non_Empty_Source_Tag_Should_Close()
		{
			var a = Element.Create("source").Add("stuff");

			Assert.AreEqual("<source>stuff</source>", a.ToString());
		}

		[Test]
		public void Create_SelfClosing_Tags()
		{
			var tags = new List<string>
			{
				"area",
				"br",
				"col",
				"img",
				"input",
				"hr",
				"link",
				"meta",
				"param"
			};

			foreach (var tag in tags)
			{
				var a = Element.Create(tag);
				var html = a.ToString();

				Assert.AreEqual(string.Format("<{0}/>", tag), html);
			}
		}
		#endregion

		#region Oddities
		[Test]
		public void Oddity_Create_Then_Add()
		{
			var d = Element.Create("div");

			d.Add(
				Element.Create("div").Add(
					Element.Create("span").Add("Here we go")));

			Assert.AreEqual("<div><div><span>Here we go</span></div></div>", d.ToString());
		}

		[Test]
		public void Oddity_Create_Then_Add_Empty_Element()
		{
			var d = Element.Create("div");

			d.Add(
				Element.Create("h2"),
				Element.Create("div").Add(
					Element.Create("span").Add("Here we go")));

			Assert.AreEqual("<div><h2></h2><div><span>Here we go</span></div></div>", d.ToString());
		}

		[Test]
		public void Oddity_Add_Id_After_Create()
		{
			var d = Element.Create("div#my-id");

			d.SetAttribute("id", "new-id");

			Assert.AreEqual("<div id=\"new-id\"></div>", d.ToString());
		}
		#endregion
	}
}



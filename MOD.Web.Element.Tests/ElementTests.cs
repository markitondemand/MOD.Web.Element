using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace MOD.Web.Element.Tests
{
	[TestFixture]
	[Category("Element")]
	public class ElementTests
	{
		#region Add
		[Test]
		public void Add_Null_Objects()
		{
			var d = Element.Create("div");

			IEnumerable<object> items = null;

			d.Add(items);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Return_From_Add_Null_Objects()
		{
			var d = Element.Create("div");

			IEnumerable<object> items = null;

			var a = d.Add(items);

			Assert.AreEqual(a, d);
		}

		[Test]
		public void Add_String_And_Null_Items()
		{
			var d = Element.Create("div");

			IEnumerable<object> items =
				new List<object>
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

			Assert.AreEqual("<div>here<div>my-div</div><a><span>click me</span></a></div>", d.ToString());
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void Add_Unsupported_Type()
		{
			var d = Element.Create("div");

			var buff = new StringBuilder();

			d.Add(buff);
		}

		[Test]
		public void Add_Enumerables_Inside_Of_Enumerables()
		{
			var d = Element.Create("div");

			var nodes =
				new List<object>
				{
					Element.Create("div"),
					(System.Collections.IEnumerable)new List<Element>
					{
						Element.Create("a")
					}
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
			var d = new Element("div");
			int? n = null;

			d.Add(n);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_Nullable_Double()
		{
			var d = new Element("div");
			double? n = null;

			d.Add(n);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_Nullable_Long()
		{
			var d = new Element("div");
			long? n = null;

			d.Add(n);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_Nullable_Boolean()
		{
			var d = new Element("div");
			bool? n = null;

			d.Add(n);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_Int()
		{
			var d = new Element("div");
			int? n = int.MaxValue;

			d.Add(n);

			Assert.AreEqual(string.Format("<div>{0}</div>", int.MaxValue), d.ToString());
		}

		[Test]
		public void Add_Double()
		{
			var d = new Element("div");
			double? n = double.MaxValue;

			d.Add(n);

			Assert.AreEqual(string.Format("<div>{0}</div>", double.MaxValue), d.ToString());
		}

		[Test]
		public void Add_Long()
		{
			var d = new Element("div");
			long? n = long.MaxValue;

			d.Add(n);

			Assert.AreEqual(string.Format("<div>{0}</div>", long.MaxValue), d.ToString());
		}

		//[Test]
		//
		//public void Add_Boolean()
		//{
		//	var d = new Element("div");
		//	bool? n = true;

		//	d.Add(n);

		//	Assert.AreEqual(d.ToString(), "<div>True</div>");
		//}

		[Test]
		public void Add_Nullable_DataTime()
		{
			var d = new Element("div");
			DateTime? n = null;

			d.Add(n);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		//[Test]
		//
		//public void Add_DataTime()
		//{
		//	var d = new Element("div");
		//	DateTime? n = DateTime.MinValue;

		//	d.Add(n);

		//	Assert.AreEqual(d.ToString(), string.Format("<div>{0}</div>", DateTime.MinValue));
		//}

		[Test]
		public void Result_of_Add_Null_Nodes_IEnumerable()
		{
			var d = Element.Create("div");

			var b = d.Add((IEnumerable<Node>)null);

			Assert.AreEqual(d, b);
		}

		[Test]
		public void Add_Null_Nodes_IEnumerable()
		{
			var d = Element.Create("div");

			d.Add((IEnumerable<Node>)null);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_Empty_IEnumerable_Of_Nodes()
		{
			var d = Element.Create("div");

			var nodes = new List<Node>();

			d.Add(nodes);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void Add_IEnumerable_Of_Tags_Nodes()
		{
			var d = Element.Create("div");

			var nodes = new List<Node> { Element.Create("a") };

			d.Add(nodes);

			Assert.AreEqual("<div><a></a></div>", d.ToString());
		}

		[Test]
		public void Add_IEnumerable_Of_Tags_And_Text_Nodes()
		{
			var d = Element.Create("div");

			var nodes = new List<Node> { Element.Create("br"), new Text("stuff") };

			d.Add(nodes);

			Assert.AreEqual("<div><br/>stuff</div>", d.ToString());
		}

		[Test]
		public void Add_IEnumerable_Nested_Nodes()
		{
			var d = Element.Create("div");

			var nodes =
				new List<Node>
				{
					Element.Create("br"),
					new Text("stuff"),
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
				new Fragment
				{
					"Test",
					Element.Create("h1")
				}
			);

			Assert.AreEqual("<div>Test<h1></h1></div>", d.ToString());
		}

		[Test]
		public void Add_Fragment_And_Element()
		{
			var d = Element.Create("div").Add(
				new Fragment
				{
					"Test",
					Element.Create("h1")
				},
				Element.Create("div")
			);

			Assert.AreEqual("<div>Test<h1></h1><div></div></div>", d.ToString());
		}

		[Test]
		public void Add_Nested_Fragments()
		{
			var d = Element.Create("div").Add(
				new Fragment
				{
					"Test",
					Element.Create("h1"),
					new Fragment
					{
						"InnerTest",
						Element.Create("h2")
					}
				}
			);

			Assert.AreEqual("<div>Test<h1></h1>InnerTest<h2></h2></div>", d.ToString());
		}

		[Test]
		public void Add_And_Then_Modify_Fragment()
		{
			var f = new Fragment
			{
				"Test"
			};

			var d = Element.Create("div").Add(f);

			f.Add("ing");

			Assert.AreEqual("<div>Testing</div>", d.ToString());
		}

		[Test]
		public void Add_Multiple_And_Then_Modify_Fragment()
		{
			var f = new Fragment
			{
				"Test"
			};

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
			d.AddClass(null);

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
			d.AddClass(null);

			Assert.AreEqual("<div class=\"first-class\"></div>", d.ToString());
		}
		#endregion

		#region AddHtml
		[Test]
		public void AddHtml_Null_Strings_Of_Html()
		{
			var d = Element.Create("div");

			IEnumerable<string> items = null;

			d.AddHtml(items);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void AddHtml_Returns_Div_When_Add_Null_Strings_Of_Html()
		{
			var d = Element.Create("div");

			IEnumerable<string> items = null;

			var a = d.AddHtml(items);

			Assert.AreEqual(a, d);
		}

		[Test]
		public void AddHtml_Return_Add_Null_Strings()
		{
			var d = Element.Create("div");

			d.AddHtml((string)null, (string)null, (string)null);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void AddHtml_Null_Strings()
		{
			var d = Element.Create("div");

			var a = d.AddHtml((string)null, (string)null, (string)null);

			Assert.AreEqual("<div></div>", d.ToString());
		}

		[Test]
		public void AddHtml_Nulls_And_Strings()
		{
			var d = Element.Create("div");

			var a = d.AddHtml((string)null, "Here", (string)null, " There", (string)null, " Everywhere");

			Assert.AreEqual("<div>Here There Everywhere</div>", d.ToString());
		}

		[Test]
		public void AddHtml_Nulls_And_Strings_From_List()
		{
			var d = Element.Create("div");

			IEnumerable<string> items =
				new List<string>
				{
					(string)null,
					"Here",
					(string)null,
					" There",
					(string)null,
					" Everywhere"
				};

			var a = d.AddHtml(items);

			Assert.AreEqual("<div>Here There Everywhere</div>", d.ToString());
		}

		[Test]
		public void AddHtml_Html_Entities()
		{
			var d = Element.Create("div");

			d.AddHtml("<&>\"");

			Assert.AreEqual("<div><&>\"</div>", d.ToString());
		}
		#endregion

		//#region Attributes
		//[Test]
		//public void AddAttribute_SpecialHandlingLink_Anchor()
		//{
		//	Element.ResolveUrlProvider = url => "/Testing";

		//	var e = Element.Create("a", "href", "~/");
		//	Assert.AreEqual("<a href=\"/Testing\"></a>", e.ToString());
		//}

		//[Test]
		//public void AddAttribute_SpecialHandlingLink_Blockquote()
		//{
		//	Element.ResolveUrlProvider = url => "/Testing";

		//	var e = Element.Create("blockquote", "cite", "~/");
		//	Assert.AreEqual("<blockquote cite=\"/Testing\"></blockquote>", e.ToString());
		//}

		//[Test]
		//public void AddAttribute_SpecialHandlingLink_Del()
		//{
		//	Element.ResolveUrlProvider = url => "/Testing";

		//	var e = Element.Create("del", "cite", "~/");
		//	Assert.AreEqual("<del cite=\"/Testing\"></del>", e.ToString());
		//}

		//[Test]
		//public void AddAttribute_SpecialHandlingLink_Form()
		//{
		//	Element.ResolveUrlProvider = url => "/Testing";

		//	var e = Element.Create("form", "action", "~/");
		//	Assert.AreEqual("<form action=\"/Testing\"></form>", e.ToString());
		//}

		//[Test]
		//public void AddAttribute_SpecialHandlingLink_Iframe()
		//{
		//	Element.ResolveUrlProvider = url => "/Testing";

		//	var e = Element.Create("iframe", "action", "~/");
		//	Assert.AreEqual("<iframe action=\"/Testing\"></iframe>", e.ToString());
		//}

		//[Test]
		//public void AddAttribute_SpecialHandlingLink_Img()
		//{
		//	Element.ResolveUrlProvider = url => "/Testing";

		//	var e = Element.Create("img", "src", "~/");
		//	Assert.AreEqual("<img src=\"/Testing\"/>", e.ToString());
		//}

		//[Test]
		//public void AddAttribute_SpecialHandlingLink_Input()
		//{
		//	Element.ResolveUrlProvider = url => "/Testing";

		//	var e = Element.Create("input", "src", "~/");
		//	Assert.AreEqual("<input src=\"/Testing\"/>", e.ToString());
		//}

		//[Test]
		//public void AddAttribute_SpecialHandlingLink_Ins()
		//{
		//	Element.ResolveUrlProvider = url => "/Testing";

		//	var e = Element.Create("ins", "cite", "~/");
		//	Assert.AreEqual("<ins cite=\"/Testing\"></ins>", e.ToString());
		//}

		//[Test]
		//public void AddAttribute_SpecialHandlingLink_Link()
		//{
		//	Element.ResolveUrlProvider = url => "/Testing";

		//	var e = Element.Create("link", "href", "~/");
		//	Assert.AreEqual("<link href=\"/Testing\"/>", e.ToString());
		//}
		//#endregion

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

			Assert.AreEqual("<div data-name=\"testing\"></div>", e.ToString());
		}

		[Test]
		public void Create_Div_With_Attr_And_Id()
		{
			var e = Element.Create("div#my-div", "data-name", "testing");

			Assert.AreEqual("<div id=\"my-div\" data-name=\"testing\"></div>", e.ToString());
		}

		[Test]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void Create_Div_With_Unpaired_Name_Value()
		{
			var e = Element.Create("div", "data-name");
		}

		[Test]
		public void Create_Render_On_Element_Returns_Itself()
		{
			var a = Element.Create("div");
			var e = a.Render();

			Assert.AreEqual(e, a);
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

		//[Test]
		//
		//public void Oddity_Href_After_Create()
		//{
		//	using (ShimsContext.Create())
		//	{
		//		Element.ResolveUrlProvider = url => url.Replace("~", "");

		//		var d = Element.Create("div", "href", "Home/Tests");

		//		d.AddAttribute("href", "~/MyController/MyAction");

		//		Assert.AreEqual("<div href=\"/MyController/MyAction\"></div>", d.ToString());
		//	}
		//}

		[Test]
		[Ignore("this should be fixed in a separate branch")]
		public void Oddity_New_Button_No_Added_Type_Attribute()
		{
			var d = new Element("button");
			d.SetAttribute("id", "button");

			Assert.AreEqual("<button id=\"button\" type=\"button\"></button>", d.ToString());
		}
		#endregion
	}
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOD.Web.Element.Tests
{
	[TestFixture]
	[Category("Text")]
	public class TextTests
	{
		[Test]
		public void Is_Encoding_Default_Text()
		{
			var text = new TextNode("here");

			Assert.IsNotNull(text);
			Assert.IsTrue(text.IsHtmlEncoded);
		}

		[Test]
		public void Has_Encoded_Default_Text()
		{
			var text = new TextNode("S & P");

			//Expect(text, Is.Not.Null);
			//Expect(text.ToString(), Is.EqualTo("S &amp; P"));
			Assert.IsNotNull(text);
			Assert.AreEqual("S &amp; P", text.ToString());
		}

		[Test]
		public void Is_Not_Encoding_Default_Text()
		{
			var text = new TextNode(true, "S & P");

			Assert.IsNotNull(text);
			Assert.IsFalse(text.IsHtmlEncoded);
		}

		[Test]
		public void Has_Not_Encoded_Default_Text()
		{
			var text = new TextNode(true, "S & P").ToString();

			Assert.IsNotNull(text);
			Assert.AreEqual("S & P", text);
		}

		[Test]
		public void Creating_Text_Passing_Nulls()
		{
			var html = new TextNode(null);
			var text = html.ToString();

			Assert.IsNotNull(text);
			Assert.AreEqual("", text);
		}
	}
}

using NUnit.Framework;

namespace MOD.Web.Element.Tests
{
	[TestFixture]
	[Category("Text")]
	public class TextTests
	{
		//[Test]
		//[Category("Text")]
		//public void Default_Text_Is_Empty_String()
		//{
		//	var text = new Text();

		//	//Expect(text.IsEncoded);
		//	//Expect(text.ToString(), Is.EqualTo(""));
		//	Assert.IsTrue(text.IsEncoded);
		//	Assert.AreEqual(text.ToString(), "");
		//}

		[Test]
		public void Is_Encoding_Default_Text()
		{
			var text = new Text("here");

			//Expect(text, Is.Not.Null);
			//Expect(text.IsEncoded);
			Assert.IsNotNull(text);
			Assert.IsTrue(text.IsEncoded);
		}

		[Test]
		public void Has_Encoded_Default_Text()
		{
			var text = new Text("S & P");

			//Expect(text, Is.Not.Null);
			//Expect(text.ToString(), Is.EqualTo("S &amp; P"));
			Assert.IsNotNull(text);
			Assert.AreEqual("S &amp; P", text.ToString());
		}

		[Test]
		public void Is_Not_Encoding_Default_Text()
		{
			var text = new Text("S & P", true);

			//Expect(text, Is.Not.Null);
			//Expect(!text.IsEncoded);
			Assert.IsNotNull(text);
			Assert.IsFalse(text.IsEncoded);
		}

		[Test]
		public void Has_Not_Encoded_Default_Text()
		{
			var text = new Text("S & P", true).ToString();

			//Expect(text, Is.Not.Null);
			//Expect(text, Is.EqualTo("S & P"));
			Assert.IsNotNull(text);
			Assert.AreEqual("S & P", text);
		}

		//[Test]
		//[Category("Text")]
		//public void Creating_Html_Passing_Null()
		//{
		//	var html = new Text((string)null, true);
		//	var text = html.ToString();

		//	//Expect(text, Is.Not.Null);
		//	//Expect(text, Is.EqualTo(""));
		//	Assert.IsNotNull(text);
		//	Assert.AreEqual(text, "");
		//}

		[Test]
		public void Creating_Text_Passing_Nulls()
		{
			var html = new Text((string)null, (string)null);
			var text = html.ToString();

			//Expect(text, Is.Not.Null);
			//Expect(text, Is.EqualTo(""));
			Assert.IsNotNull(text);
			Assert.AreEqual("", text);
		}

		[Test]
		public void Creating_Text_From_Lines()
		{
			var text = new Text("S & P", " and ", "Dow").ToString();

			//Expect(text, Is.Not.Null);
			//Expect(text, Is.EqualTo("S &amp; P and Dow"));
			Assert.IsNotNull(text);
			Assert.AreEqual("S &amp; P and Dow", text);
		}
	}
}

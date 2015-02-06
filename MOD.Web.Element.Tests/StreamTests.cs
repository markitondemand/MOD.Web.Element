using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOD.Web.Element.Tests
{
	[TestFixture]
	[Category("Stream")]
	public class StreamTests
	{
		[Test]
		public void StreamNode_Matches_TextReader()
		{
			string text = "this is a test";
			StringReader tr = new StringReader(text);
			var node = new StreamNode(tr);

			string html = node.ToString();
			Assert.AreEqual(html,text);
		}

		[Test]
		public void StreamNode_Matches_TextWriter()
		{
			string text = "this is test 2";
			var node = new StreamNode(w => w.Write(text));

			string html = node.ToString();
			Assert.AreEqual(html,text);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void StreamNode_Matches_Null_TextReader()
		{
			var node = new StreamNode((TextReader)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void StreamNode_Matches_Null_TextWriter()
		{
			var node = new StreamNode((MOD.Web.Element.StreamNode.WriterDelegate)null);
		}
	}
}

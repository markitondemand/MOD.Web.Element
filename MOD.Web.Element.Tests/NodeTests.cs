using System.Text;
using NUnit.Framework;

namespace MOD.Web.Element.Tests
{
	[TestFixture]
	[Category("Node")]
	public class NodeTests
	{
		[Test]
		public void Default_Item()
		{
			var item = new Node();

			//Expect(item.Parent, Is.Null);
			Assert.IsNull(item.Parent);
		}

		[Test]
		public void ToString_Passing_Null_StringBuilder()
		{
			var item = new Node();

			item.ToString(null);
		}

		[Test]
		public void ToString_Passing_Non_Null_StringBuilder()
		{
			var item = new Node();
			var sb = new StringBuilder();

			item.ToString(sb);

			//Expect(sb.Length, Is.EqualTo(0));
			Assert.AreEqual(0, sb.Length);
		}
	}
}

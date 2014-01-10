using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MOD.Web.Element
{
	public class Text : Node
	{
		public Text() { }
		public Text(params string[] text) : this(String.Join("", text)) { }
		public Text(string text) : this(text, false) { }
		public Text(string text, bool leaveRaw)
		{
			Value = text;
			IsEncoded = !leaveRaw;
		}

		public override void ToString(StringBuilder sb)
		{
			sb.Append(ToString());
		}

		public override string ToString()
		{
			if (IsEncoded)
			{
				return HttpUtility.HtmlEncode(Value); //This doesn't encode unicode characters!!??
			}
			else
			{
				return Value;
			}
		}

		public bool IsEncoded;
		public string Value;
	}
}

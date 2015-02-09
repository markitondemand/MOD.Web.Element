using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MOD.Web.Element
{
	/// <summary>
	/// Similar to a TextNode, this represents a Node that is only text, no children and not a tag,
	///  but where the contents are provided by a TextReader/TextWriter
	/// </summary>
	public sealed class StreamNode : INode
	{
		/// <summary>
		/// Constructor for providing a reader to be read as text/html
		/// </summary>
		public StreamNode(TextReader reader): this(reader,defaultBufferSize) {}
		
		/// <summary>
		/// Constructor for providing a reader to be read as text/html and customize the buffersize
		/// </summary>
		public StreamNode(TextReader reader, int bufferSize)
		{
			if (reader == null) {
				throw new ArgumentNullException("reader must not be null");
			}
			if (bufferSize < 1) {
				throw new ArgumentOutOfRangeException("bufferSize must be greater than 0");
			}
			_reader = reader;
			BufferSize = bufferSize;
			_buffer = new char[bufferSize];
		}
		/// <summary>
		/// Constructor for providing a writer method that accepts a TextWriter
		/// </summary>
		public StreamNode(WriterDelegate writerMethod)
		{
			if (writerMethod == null) {
				throw new ArgumentNullException("writerMethod must no be null");
			}
			_writer = writerMethod;
		}
		
		/// <summary>
		/// Delegate that can be used to provide the textwriter content
		/// </summary>
		public delegate void WriterDelegate(TextWriter w);
		
		private TextReader _reader = null;
		private WriterDelegate _writer = null;
		private const int defaultBufferSize = 32*1024;
		private char[] _buffer;

		/// <summary>
		/// Returns the output as a string. Use Write() instead if possible
		/// </summary>
		public override string ToString()
		{
			StringWriter sw = new StringWriter();
			Write(sw);
			return sw.ToString();
		}

		/// <summary>
		/// Returns the output as a string. Same as calling ToString();
		/// </summary>
		public string ToHtmlString()
		{
			return this.ToString();
		}
		
		/// <summary>
		/// Buffer size in bytes. Only used with a TextReader
		/// </summary>
		public int BufferSize { get; set; }

		public TextWriter Write(TextWriter writer)
		{
			if (_writer != null) {
				_writer.Invoke(writer);
			}
			else
			{
				int count = -1;
				do {
					count = _reader.ReadBlock(_buffer,0,BufferSize);
					if (count > 0) { writer.Write(_buffer,0,count); }
				} while(count > 0);
			}
			return writer;
		}
	}
}

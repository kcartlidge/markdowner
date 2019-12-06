using Markdowner.Enumerations;
using System.Collections.Generic;
using System.Text;

namespace Markdowner.Models
{
    public class Line
    {
        /// <summary>
        /// The type of line.
        /// </summary>
        public LineType LineType { get; internal set; }

        /// <summary>
        /// The location in the original source text.
        /// </summary>
        public int OriginalLineNumber { get; }

        /// <summary>
        /// The location in the compressed source.
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// The content for the line.
        /// </summary>
        public StringBuilder Text { get; internal set; }

        /// <summary>
        /// The length of the line.
        /// </summary>
        public int Length { get; internal set; }

        /// <summary>
        /// The tokenised content, for use in output generation.
        /// </summary>
        public List<Token> Tokens { get; set; }

        /// <summary>
        /// The location and contents of a line of source.
        /// </summary>
        public Line(int originalLineNumber, int lineNumber, string text)
        {
            text = string.IsNullOrEmpty(text) ? string.Empty : text;
            OriginalLineNumber = originalLineNumber;
            LineNumber = lineNumber;
            Text = new StringBuilder(text);
            LineType = LineType.Empty;
            Length = text.Length;
        }

        public override string ToString()
        {
            return Text.ToString();
        }
    }
}

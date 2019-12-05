using System.Text;

namespace Markdowner.Models
{
    public class Line
    {
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
        public StringBuilder Text { get; }

        /// <summary>
        /// The length of the line.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// The location and contents of a line of source.
        /// </summary>
        public Line(int originalLineNumber, int lineNumber, string text)
        {
            OriginalLineNumber = originalLineNumber;
            LineNumber = lineNumber;
            Text = new StringBuilder(text);
            Length = text.Length;
        }

        public override string ToString()
        {
            return Text.ToString();
        }
    }
}

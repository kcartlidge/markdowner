using Markdowner.Enumerations;
using System;
using System.Text;

namespace Markdowner.Models
{
    public class Line
    {
        /// <summary>
        /// The type of line.
        /// </summary>
        public LineType LineType { get; private set; }

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
        public StringBuilder Text { get; private set; }

        /// <summary>
        /// The length of the line.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// The location and contents of a line of source.
        /// </summary>
        public Line(int originalLineNumber, int lineNumber, string text)
        {
            text = string.IsNullOrEmpty(text) ? string.Empty : text;

            LineType = LineType.Empty;
            OriginalLineNumber = originalLineNumber;
            LineNumber = lineNumber;
            Text = new StringBuilder(text);
            Length = text.Length;

            if (Length > 0)
            {
                LineType = LineType.Paragraph;
                CheckType("# ", LineType.Header1, defaultAmender);
                CheckType("## ", LineType.Header2, defaultAmender);
                CheckType("### ", LineType.Header3, defaultAmender);
                CheckType("#### ", LineType.Header4, defaultAmender);
                CheckType("##### ", LineType.Header5, defaultAmender);
                CheckType("###### ", LineType.Header6, defaultAmender);
                CheckType("- ", LineType.UnorderedList, defaultAmender);
                CheckType("* ", LineType.UnorderedList, defaultAmender);
                CheckType("1. ", LineType.OrderedList, defaultAmender);
                CheckType(" ", LineType.Quote, whitespaceAmender);
                CheckType("\t", LineType.Quote, whitespaceAmender);
                CheckType("```", LineType.Pre, runAmender);
                CheckType("---", LineType.Rule, runAmender);
            }
        }

        public override string ToString()
        {
            return Text.ToString();
        }

        /// <summary>Returns minus the prefix and trimmed of leading whitespace.</summary>
        private string defaultAmender(string prefix)
        {
            return Text.Remove(0, prefix.Length).ToString().TrimStart();
        }

        /// <summary>Returns trimmed of leading whitespace.</summary>
        private string whitespaceAmender(string prefix)
        {
            return Text.ToString().TrimStart();
        }

        /// <summary>
        /// Removes the matched prefix from the original, and
        /// returns it with the leading whitespace trimmed.
        /// </summary>
        private string runAmender(string prefix)
        {
            Text.Remove(0, prefix.Length);
            return Text.ToString().TrimStart();
        }

        /// <summary>Set the type by prefix, and run the amendment func to update the source.</summary>
        private void CheckType(string prefix,
                               LineType becomesType,
                               Func<string, string> amendFunc)
        {
            if (StartsWith(Text, prefix))
            {
                LineType = becomesType;
                Text = new StringBuilder(amendFunc(prefix));
                Length = Text.Length;
            }
        }

        /// <summary>
        /// Checks if the a StringBuilder's text starts with a given string.
        /// Provided to avoid ToString on the StringBuilder (to access
        /// StartsWith()) as that overhead is otherwise unnecessary.
        /// </summary>
        private bool StartsWith(StringBuilder content, string startText)
        {
            var contentLength = content.Length;
            for (int i = 0; i < startText.Length; i++)
            {
                if (i >= contentLength) return false;
                if (startText[i] != content[i]) return false;
            }
            return true;
        }
    }
}

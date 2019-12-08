using Markdowner.Enumerations;
using Markdowner.Models;
using System;
using System.Text;

namespace Markdowner.Parsers
{
    internal class LineParser
    {
        Line lineOut;

        /// <summary>Update the line with it's LineType.</summary>
        public void Parse(Line lineIn)
        {
            lineOut = lineIn;

            if (lineOut.Length > 0)
            {
                lineOut.LineType = LineType.Paragraph;
                CheckType("# ", LineType.Header1, defaultAmender);
                CheckType("## ", LineType.Header2, defaultAmender);
                CheckType("### ", LineType.Header3, defaultAmender);
                CheckType("#### ", LineType.Header4, defaultAmender);
                CheckType("##### ", LineType.Header5, defaultAmender);
                CheckType("###### ", LineType.Header6, defaultAmender);
                CheckType("- ", LineType.UnorderedList, defaultAmender);
                CheckType("* ", LineType.UnorderedList, defaultAmender);
                CheckType("1. ", LineType.OrderedList, defaultAmender);
                CheckType(" ", LineType.Pre, whitespaceAmender);
                CheckType("\t", LineType.Pre, whitespaceAmender);
                CheckType("> ", LineType.Quote, defaultAmender);
                CheckType("---", LineType.Rule, runAmender);
            }
        }

        /// <summary>Set the type by prefix, and run the amendment func to update the source.</summary>
        private void CheckType(string prefix,
                               LineType becomesType,
                               Func<string, string> amendFunc)
        {
            if (StartsWith(lineOut.Text, prefix))
            {
                lineOut.LineType = becomesType;
                lineOut.Text = new StringBuilder(amendFunc(prefix));
                lineOut.Length = lineOut.Text.Length;
            }
        }

        /// <summary>Returns minus the prefix and trimmed of leading whitespace.</summary>
        private string defaultAmender(string prefix)
        {
            return lineOut.Text.Remove(0, prefix.Length).ToString().TrimStart();
        }

        /// <summary>Returns trimmed of leading whitespace.</summary>
        private string whitespaceAmender(string prefix)
        {
            return lineOut.Text.ToString().TrimStart();
        }

        /// <summary>
        /// Removes the matched prefix from the original, and
        /// returns it with the leading whitespace trimmed.
        /// </summary>
        private string runAmender(string prefix)
        {
            lineOut.Text.Remove(0, prefix.Length);
            return lineOut.Text.ToString().TrimStart();
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

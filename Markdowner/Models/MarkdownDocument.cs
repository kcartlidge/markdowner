using Markdowner.Services;
using System;
using System.Collections.Generic;

namespace Markdowner.Models
{
    public class MarkdownDocument
    {
        private readonly char[] EOL = "\n".ToCharArray();
        private readonly char[] TRIM = " \t\r\n".ToCharArray();

        /// <summary>
        /// Original source after being compressed.
        /// Trailing spaces, tabs, and carriage returns are removed.
        /// Leading and trailing empty lines are removed.
        /// Runs of empty lines are reduced to a single one.
        /// Runs of content lines without empty lines between are combined.
        /// </summary>
        public List<Line> CompressedSource { get; private set; }

        /// <summary>
        /// The complete definition of a loaded collection of source.
        /// </summary>
        public MarkdownDocument(string text)
        {
            CompressedSource = new List<Line>();
            if (text == null) return;

            // Get separate lines (EOL) with no trailing whitespace (TRIM).
            var lines = text.Split(EOL, StringSplitOptions.None);
            var lastLineWasEmpty = true;
            var lineNum = 0;
            foreach (var line in lines)
            {
                lineNum++;
                var trimmed = line.TrimEnd(TRIM);

                // Skip leading and/or multiple empty lines.
                if (trimmed.Length == 0)
                {
                    if (lastLineWasEmpty) continue;
                    lastLineWasEmpty = true;
                    continue;
                }

                // Only start a new line if there was a preceeding empty one.
                // This compresses runs of lines into single paragraphs.
                if (lastLineWasEmpty)
                {
                    CompressedSource.Add(new Line(lineNum,
                                                  CompressedSource.Count + 1,
                                                  trimmed));
                }
                else
                {
                    CompressedSource[CompressedSource.Count - 1].Text.Append(" ");
                    CompressedSource[CompressedSource.Count - 1].Text.Append(trimmed);
                }
                lastLineWasEmpty = false;
            }

            MarkdownService.GetTokens(this);
        }
    }
}

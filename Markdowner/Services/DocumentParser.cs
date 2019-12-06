using Markdowner.Models;
using System;
using System.Collections.Generic;

namespace Markdowner.Services
{
    internal class DocumentParser
    {
        private readonly char[] EOL = "\n".ToCharArray();
        private readonly char[] TRIM = " \t\r\n".ToCharArray();

        private MarkdownDocument documentOut;

        public MarkdownDocument Parse(MarkdownDocument documentIn)
        {
            documentOut = documentIn;

            // Compress the document text into resulting lines.
            documentOut.CompressedText = new List<Line>();
            if (documentOut.text == null) return documentOut;

            // Get separate lines (EOL) with no trailing whitespace (TRIM).
            var lines = documentOut.text.Split(EOL, StringSplitOptions.None);
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
                    documentOut.CompressedText.Add(new Line(lineNum,
                                                  documentOut.CompressedText.Count + 1,
                                                  trimmed));
                }
                else
                {
                    documentOut.CompressedText[documentOut.CompressedText.Count - 1]
                        .Text.Append(" ");
                    documentOut.CompressedText[documentOut.CompressedText.Count - 1]
                        .Text.Append(trimmed);
                }
                lastLineWasEmpty = false;
            }

            // Parse the line types.
            var lineParser = new LineParser();
            foreach (var line in documentOut.CompressedText)
            {
                lineParser.Parse(line);
            }

            // Tokenise.
            var contentParser = new ContentParser();
            foreach (var line in documentOut.CompressedText)
            {
                line.Tokens = contentParser.Parse(line.Text.ToString());
            }

            return documentOut;
        }
    }
}

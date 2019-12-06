using Markdowner.Enumerations;
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
            var lineParser = new LineParser();
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

                // Create a representative line and derive it's line type.
                var newLine = new Line(lineNum, documentOut.CompressedText.Count + 1, trimmed);
                lineParser.Parse(newLine);

                // Only start a new line if there was a preceeding empty one.
                // This compresses runs of lines into single paragraphs.
                // However if the line type is not Paragraph it's new regardless.
                if (lastLineWasEmpty || newLine.LineType != LineType.Paragraph)
                {
                    documentOut.CompressedText.Add(newLine);
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

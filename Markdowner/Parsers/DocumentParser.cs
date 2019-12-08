using Markdowner.Enumerations;
using Markdowner.Models;
using System;
using System.Collections.Generic;

namespace Markdowner.Parsers
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
            var lastLineType = LineType.Empty;
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
                    lastLineType = LineType.Empty;
                    continue;
                }

                // Create a representative line and derive it's line type.
                var newLine = new Line(lineNum, documentOut.CompressedText.Count + 1, trimmed);
                lineParser.Parse(newLine);

                // Compress paragraphs, unless:
                // (a) the preceeding line was empty,
                // (b) the preceeding line wasn't a paragraph.
                if (newLine.LineType == LineType.Paragraph)
                {
                    if (lastLineWasEmpty || lastLineType != LineType.Paragraph)
                    {
                        documentOut.CompressedText.Add(newLine);
                    }
                    else
                    {
                        documentOut
                            .CompressedText[documentOut.CompressedText.Count - 1]
                            .Text.Append(" ");
                        documentOut
                            .CompressedText[documentOut.CompressedText.Count - 1]
                            .Text.Append(trimmed);
                    }
                }
                else
                {
                    documentOut.CompressedText.Add(newLine);
                }

                lastLineWasEmpty = false;
                lastLineType = newLine.LineType;
            }

            // Tokenise and set multi-line block Start/Stop flags.
            var contentParser = new ContentParser();
            var max = documentOut.CompressedText.Count;
            for (int i = 0; i < max; i++)
            {
                var line = documentOut.CompressedText[i];
                line.Tokens = contentParser.Parse(line.Text.ToString());

                var prevType = (i > 0 ? documentOut.CompressedText[i - 1].LineType : LineType.Empty);
                var nextType = (i < (max - 1) ? documentOut.CompressedText[i + 1].LineType : LineType.Empty);
                line.IsBlockStart = (line.LineType != prevType);
                line.IsBlockEnd = (line.LineType != nextType);
            }

            return documentOut;
        }
    }
}

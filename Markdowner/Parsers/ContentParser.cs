using Markdowner.Enumerations;
using Markdowner.Models;
using System.Collections.Generic;
using System.Linq;

namespace Markdowner.Parsers
{
    internal class ContentParser
    {
        private Formatting[] Formats = new Formatting[]
        {
            new Formatting{ On = "**", Off = "**", TokenOn = TokenType.BoldOn, TokenOff = TokenType.BoldOff },
            new Formatting{ On = "*", Off = "*", TokenOn = TokenType.ItalicOn, TokenOff = TokenType.ItalicOff },
            new Formatting{ On = "_", Off = "_", TokenOn = TokenType.UnderlineOn, TokenOff = TokenType.UnderlineOff },
            new Formatting{ On = "`", Off = "`", TokenOn = TokenType.CodeOn, TokenOff = TokenType.CodeOff },
        };

        public List<Token> Parse(string textIn)
        {
            var linesOut = new List<Token> { };
            var flags = new SortedList<string, bool>();

            // Populate tokens.
            var isContent = false;
            for (int i = 0; i < textIn.Length; i++)
            {
                // Consume all the formats (nesting support handled elsewhere).
                // Skip remains 0 if none were found.
                var skip = 0;
                foreach (var format in Formats)
                {
                    var flag = flags.ContainsKey(format.On) && flags[format.On];
                    skip = Consume(linesOut, textIn, i, format.On, flag ? format.TokenOff : format.TokenOn);
                    if (skip > 0)
                    {
                        isContent = false;
                        flags[format.On] = !flag;
                        i += skip - 1;  // The -1 compensates for the loop increment.
                        break;
                    }
                }
                if (skip > 0) continue;

                // No skips, so we are dealing with content.
                // Accumulate it, or start a new token if the last wasn't content.
                var ch = textIn[i];
                if (isContent)
                {
                    linesOut.Last().Text += ch;
                }
                else
                {
                    isContent = true;
                    linesOut.Add(new Token { TokenType = TokenType.Normal, Offset = i, Text = ch.ToString() });
                }
            }

            // Close any still-open formats.
            foreach (var format in Formats)
            {
                if (flags.ContainsKey(format.On) && flags[format.On])
                {
                    linesOut.Add(new Token { TokenType = format.TokenOff, Offset = textIn.Length, Text = format.Off });
                }
            }

            return linesOut;
        }

        private int Consume(List<Token> result, string text, int idx, string marker, TokenType tokenType)
        {
            for (int i = 0; i < marker.Length; i++)
            {
                if (Getchar(text, idx + i) != marker[i]) return 0;
            }
            result.Add(new Token { TokenType = tokenType, Offset = idx, Text = marker });
            return marker.Length;
        }

        private char Getchar(string text, int pos)
        {
            if (pos > text.Length) return (char)0;
            return text[pos];
        }
    }
}

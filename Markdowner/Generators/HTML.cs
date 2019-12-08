using Markdowner.Enumerations;
using Markdowner.Models;
using System.Collections.Generic;
using System.Text;

namespace Markdowner.Generators
{
    public class HTML : IGenerator
    {
        IParser parser = new Parser();

        public StringBuilder Generate(string text)
        {
            var document = parser.Parse(text);
            var html = new StringBuilder();

            foreach (var line in document.CompressedText)
            {
                switch (line.LineType)
                {
                    case LineType.Empty:
                        break;
                    case LineType.Header1:
                        html.Append(Element("h1", TokenText(line.Tokens)));
                        break;
                    case LineType.Header2:
                        html.Append(Element("h2", TokenText(line.Tokens)));
                        break;
                    case LineType.Header3:
                        html.Append(Element("h3", TokenText(line.Tokens)));
                        break;
                    case LineType.Header4:
                        html.Append(Element("h4", TokenText(line.Tokens)));
                        break;
                    case LineType.Header5:
                        html.Append(Element("h5", TokenText(line.Tokens)));
                        break;
                    case LineType.Header6:
                        html.Append(Element("h6", TokenText(line.Tokens)));
                        break;
                    case LineType.OrderedList:
                        if (line.IsBlockStart) html.Append("<ol>\n");
                        html.Append(Element("li", TokenText(line.Tokens), 2));
                        if (line.IsBlockEnd) html.Append("\n</ol>");
                        break;
                    case LineType.UnorderedList:
                        if (line.IsBlockStart) html.Append("<ul>\n");
                        html.Append(Element("li", TokenText(line.Tokens), 2));
                        if (line.IsBlockEnd) html.Append("\n</ul>");
                        break;
                    case LineType.Quote:
                        if (line.IsBlockStart) html.Append("<blockquote>\n");
                        html.Append(TokenText(line.Tokens));
                        html.Append("<br/>");
                        if (line.IsBlockEnd) html.Append("\n</blockquote>");
                        break;
                    case LineType.Pre:
                        // Deliberately no line breaks in pre tags.
                        if (line.IsBlockStart) html.Append("<pre>");
                        html.Append(TokenText(line.Tokens));
                        if (line.IsBlockEnd) html.Append("</pre>");
                        break;
                    case LineType.Rule:
                        html.Append("<hr/>");
                        break;
                    case LineType.Paragraph:
                    default:
                        html.Append(Element("p", TokenText(line.Tokens)));
                        break;
                }
                html.Append("\n");
            }

            return html;
        }

        private StringBuilder Element(string tag, StringBuilder content, int indent = 0)
        {
            var pad = "".PadLeft(indent);
            return new StringBuilder($"{pad}<{tag}>{content}</{tag}>");
        }

        private StringBuilder TokenText(List<Token> tokens)
        {
            var result = new StringBuilder();
            foreach (var token in tokens)
            {
                switch (token.TokenType)
                {
                    case TokenType.Normal:
                        result.Append(token.Text);
                        break;
                    case TokenType.BoldOn:
                        result.Append("<strong>");
                        break;
                    case TokenType.BoldOff:
                        result.Append("</strong>");
                        break;
                    case TokenType.ItalicOn:
                        result.Append("<em>");
                        break;
                    case TokenType.ItalicOff:
                        result.Append("</em>");
                        break;
                    case TokenType.UnderlineOn:
                        result.Append("<u>");
                        break;
                    case TokenType.UnderlineOff:
                        result.Append("</u>");
                        break;
                    case TokenType.CodeOn:
                        result.Append("<code>");
                        break;
                    case TokenType.CodeOff:
                        result.Append("</code>");
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}

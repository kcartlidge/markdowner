using Markdowner.Enumerations;
using Markdowner.Models;
using System.Collections.Generic;

namespace Markdowner.Services
{
    internal static class MarkdownService
    {
        public static void GetTokens(DocumentSource documentSource)
        {
            foreach (var line in documentSource.CompressedSource)
            {
                line.Tokens = new List<Token> {
                    new Token { TokenType = TokenType.Normal, Text = line.Text.ToString() }
                };
            }
        }
    }
}

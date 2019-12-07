using Markdowner.Models;
using Markdowner.Parsers;

namespace Markdowner
{
    /// See <see cref="IParser"/>
    public class Parser : IParser
    {
        /// See <see cref="IParser.Parse"/>
        public MarkdownDocument Parse(string textIn)
        {
            var documentOut = new MarkdownDocument(textIn);
            return new DocumentParser().Parse(documentOut);
        }
    }
}

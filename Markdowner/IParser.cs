using Markdowner.Models;

namespace Markdowner
{
    public interface IParser
    {
        MarkdownDocument Parse(string text);
    }
}

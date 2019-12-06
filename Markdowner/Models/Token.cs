using Markdowner.Enumerations;

namespace Markdowner.Models
{
    public class Token
    {
        public TokenType TokenType;
        public string Text;
        public int Offset;

        public override string ToString()
        {
            return $"{Offset} {TokenType} => {Text}";
        }
    }
}

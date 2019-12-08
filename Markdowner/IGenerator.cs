using System.Text;

namespace Markdowner
{
    public interface IGenerator
    {
        StringBuilder Generate(string text);
    }
}

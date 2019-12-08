using FluentAssertions;
using Markdowner;
using Markdowner.Enumerations;
using NUnit.Framework;

namespace Tests
{
    public class LineTests
    {
        IParser parser = new Parser();

        [Test]
        [TestCase("#", "H1", LineType.Header1)]
        [TestCase("##", "H2", LineType.Header2)]
        [TestCase("###", "H3", LineType.Header3)]
        [TestCase("####", "H4", LineType.Header4)]
        [TestCase("#####", "H5", LineType.Header5)]
        [TestCase("######", "H6", LineType.Header6)]
        [TestCase("Sample", "Paragraph", LineType.Paragraph)]
        [TestCase("1.", "OrderedList", LineType.OrderedList)]
        [TestCase("-", "UnorderedList", LineType.UnorderedList)]
        [TestCase("*", "UnorderedList", LineType.UnorderedList)]
        [TestCase(">", "Quote", LineType.Quote)]
        [TestCase(" ", "Pre", LineType.Pre)]
        [TestCase("---", "Rule", LineType.Rule)]
        public void WithContent_HasCorrectLineTypes_AndNormalisesText(
            string prefix,
            string text,
            LineType lineType)
        {
            var document = parser.Parse($"{prefix} {text}");

            document.CompressedText[0].LineType.Should().Be(lineType);
        }
    }
}

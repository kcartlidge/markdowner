using FluentAssertions;
using Markdowner.Enumerations;
using Markdowner.Models;
using NUnit.Framework;

namespace Tests
{
    public class LineTests
    {
        [Test]
        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase("   ", 0)]
        [TestCase("12345  890", 10)]
        [TestCase("   hello", 5)]
        [TestCase("hello   ", 8)]
        public void WithVariousText_HasCorrectLengths(string text, int length)
        {
            var line = new Line(1, 1, text);

            line.Length.Should().Be(length);
        }

        [Test]
        [TestCase("", "", LineType.Empty)]
        [TestCase("#", "H1", LineType.Header1)]
        [TestCase("##", "H2", LineType.Header2)]
        [TestCase("###", "H3", LineType.Header3)]
        [TestCase("####", "H4", LineType.Header4)]
        [TestCase("#####", "H5", LineType.Header5)]
        [TestCase("######", "H6", LineType.Header6)]
        [TestCase("", "Paragraph", LineType.Paragraph)]
        [TestCase("1.", "OrderedList", LineType.OrderedList)]
        [TestCase("-", "UnorderedList", LineType.UnorderedList)]
        [TestCase("*", "UnorderedList", LineType.UnorderedList)]
        [TestCase("\t", "Quote", LineType.Quote)]
        [TestCase("```", "Pre", LineType.Pre)]
        [TestCase("---", "Rule", LineType.Rule)]
        public void DetermineLineTypeInfo_SetsLineTypes(
            string prefix,
            string text,
            LineType lineType)
        {
            var content = $"{prefix} {text}".Trim(' ');

            var line = new Line(1, 1, content);

            line.LineType.Should().Be(lineType);
            line.Text.ToString().Should().Be(text);
        }
    }
}

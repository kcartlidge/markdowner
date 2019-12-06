using FluentAssertions;
using Markdowner;
using NUnit.Framework;

namespace Tests
{
    public class MarkdownDocumentTests
    {
        IParser parser = new Parser();

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void WithNoSource_HasEmptyLines(string source)
        {
            var document = parser.Parse(source);

            document.CompressedText.Count.Should().Be(0);
        }

        [Test]
        public void WithWhitespaceSource_HasEmptyLines()
        {
            var document = parser.Parse("  \t  \r  \t");

            document.CompressedText.Count.Should().Be(0);
        }

        [Test]
        public void RemovesLeadingEmptyLines()
        {
            var document = parser.Parse("  \n  \n  \nHello.");

            document.CompressedText.Count.Should().Be(1);
        }

        [Test]
        public void RemovesTrailingEmptyLines()
        {
            var document = parser.Parse("Hello.\n\n \t\n");

            document.CompressedText.Count.Should().Be(1);
        }

        [Test]
        public void CompressesRunsOfEmptyLines()
        {
            var document = parser.Parse("One\n\n\nTwo\n\nThree");

            document.CompressedText.Count.Should().Be(3);
        }

        [Test]
        public void JoinsRunsOfLinesWithoutEmptyLinesBetween()
        {
            var document = parser.Parse("One\nTwo\nThree\n\nFour");

            document.CompressedText.Count.Should().Be(2);
            document.CompressedText[0].ToString().Should().Be("One Two Three");
            document.CompressedText[1].ToString().Should().Be("Four");
        }

        [Test]
        public void WithSomeEmptyLines_SetsCorrectLineNumbers()
        {
            var document = parser.Parse("One\n\nTwo\n\n\nThree\nFour\n\nFive");

            document.CompressedText.Count.Should().Be(4);

            document.CompressedText[0].OriginalLineNumber.Should().Be(1); // one
            document.CompressedText[1].OriginalLineNumber.Should().Be(3); // Two
            document.CompressedText[2].OriginalLineNumber.Should().Be(6); // Three Four
            document.CompressedText[3].OriginalLineNumber.Should().Be(9); // Five

            document.CompressedText[0].LineNumber.Should().Be(1);
            document.CompressedText[1].LineNumber.Should().Be(2);
            document.CompressedText[2].LineNumber.Should().Be(3);
            document.CompressedText[3].LineNumber.Should().Be(4);
        }
    }
}

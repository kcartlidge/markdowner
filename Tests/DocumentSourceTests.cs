using FluentAssertions;
using Markdowner.Models;
using NUnit.Framework;

namespace Tests
{
    public class DocumentSourceTests
    {
        DocumentSource document;

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void WithNoSource_HasEmptyLines(string source)
        {
            document = new DocumentSource(source);

            document.CompressedSource.Count.Should().Be(0);
        }

        [Test]
        public void WithWhitespaceSource_HasEmptyLines()
        {
            document = new DocumentSource("  \t  \r  \t");

            document.CompressedSource.Count.Should().Be(0);
        }

        [Test]
        public void RemovesLeadingEmptyLines()
        {
            document = new DocumentSource("  \n  \n  \nHello.");

            document.CompressedSource.Count.Should().Be(1);
        }

        [Test]
        public void RemovesTrailingEmptyLines()
        {
            document = new DocumentSource("Hello.\n\n \t\n");

            document.CompressedSource.Count.Should().Be(1);
        }

        [Test]
        public void CompressesRunsOfEmptyLines()
        {
            document = new DocumentSource("One\n\n\nTwo\n\nThree");

            document.CompressedSource.Count.Should().Be(3);
        }

        [Test]
        public void JoinsRunsOfLinesWithoutEmptyLinesBetween()
        {
            document = new DocumentSource("One\nTwo\nThree\n\nFour");

            document.CompressedSource.Count.Should().Be(2);
            document.CompressedSource[0].ToString().Should().Be("One Two Three");
            document.CompressedSource[1].ToString().Should().Be("Four");
        }

        [Test]
        public void WithSomeEmptyLines_SetsCorrectLineNumbers()
        {
            document = new DocumentSource("One\n\nTwo\n\n\nThree\nFour\n\nFive");

            document.CompressedSource.Count.Should().Be(4);

            document.CompressedSource[0].OriginalLineNumber.Should().Be(1); // one
            document.CompressedSource[1].OriginalLineNumber.Should().Be(3); // Two
            document.CompressedSource[2].OriginalLineNumber.Should().Be(6); // Three Four
            document.CompressedSource[3].OriginalLineNumber.Should().Be(9); // Five

            document.CompressedSource[0].LineNumber.Should().Be(1);
            document.CompressedSource[1].LineNumber.Should().Be(2);
            document.CompressedSource[2].LineNumber.Should().Be(3);
            document.CompressedSource[3].LineNumber.Should().Be(4);
        }
    }
}

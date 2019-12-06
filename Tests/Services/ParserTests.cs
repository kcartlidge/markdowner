using FluentAssertions;
using Markdowner.Enumerations;
using Markdowner.Models;
using Markdowner.Services;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class ParserTests
    {
        Parser parser;

        [SetUp]
        public void Setup()
        {
            parser = new Parser();
        }

        [Test]
        public void PlainText()
        {
            var text = "plain text";
            var result = parser.Parse(text);

            result.Should().BeEquivalentTo(new List<Token>
            {
                new Token{ TokenType = TokenType.Normal, Text = text },
            });
        }

        [Test]
        public void Bold()
        {
            var text = "some **bold** text, and **yet more bold** text.";
            //         "01234567890123456789012345678901234567890123456";
            var result = parser.Parse(text);

            result.Should().BeEquivalentTo(new List<Token>
            {
                new Token{Offset = 0, TokenType = TokenType.Normal, Text = "some " },
                new Token{Offset = 5, TokenType = TokenType.BoldOn, Text = "**" },
                new Token{Offset = 7, TokenType = TokenType.Normal, Text = "bold" },
                new Token{Offset = 11, TokenType = TokenType.BoldOff, Text ="**"},
                new Token{Offset = 13, TokenType = TokenType.Normal, Text = " text, and " },
                new Token{Offset = 24, TokenType = TokenType.BoldOn, Text = "**" },
                new Token{Offset = 26, TokenType = TokenType.Normal, Text = "yet more bold" },
                new Token{Offset = 39, TokenType = TokenType.BoldOff, Text ="**"},
                new Token{Offset = 41, TokenType = TokenType.Normal, Text = " text." },
            });
        }

        [Test]
        public void Italic()
        {
            var text = "some *italic* text, and *yet more italic* text.";
            //         "01234567890123456789012345678901234567890123456";
            var result = parser.Parse(text);

            result.Should().BeEquivalentTo(new List<Token>
            {
                new Token{Offset = 0, TokenType = TokenType.Normal, Text = "some " },
                new Token{Offset = 5, TokenType = TokenType.ItalicOn, Text = "*" },
                new Token{Offset = 6, TokenType = TokenType.Normal, Text = "italic" },
                new Token{Offset = 12, TokenType = TokenType.ItalicOff, Text ="*"},
                new Token{Offset = 13, TokenType = TokenType.Normal, Text = " text, and " },
                new Token{Offset = 24, TokenType = TokenType.ItalicOn, Text = "*" },
                new Token{Offset = 25, TokenType = TokenType.Normal, Text = "yet more italic" },
                new Token{Offset = 40, TokenType = TokenType.ItalicOff, Text ="*"},
                new Token{Offset = 41, TokenType = TokenType.Normal, Text = " text." },
            });
        }

        [Test]
        public void Underline()
        {
            var text = "some _underline_ text, and _yet more underline_ text.";
            //         "01234567890123456789012345678901234567890123456789012";
            var result = parser.Parse(text);

            result.Should().BeEquivalentTo(new List<Token>
            {
                new Token{Offset = 0, TokenType = TokenType.Normal, Text = "some " },
                new Token{Offset = 5, TokenType = TokenType.UnderlineOn, Text = "_" },
                new Token{Offset = 6, TokenType = TokenType.Normal, Text = "underline" },
                new Token{Offset = 15, TokenType = TokenType.UnderlineOff, Text ="_"},
                new Token{Offset = 16, TokenType = TokenType.Normal, Text = " text, and " },
                new Token{Offset = 27, TokenType = TokenType.UnderlineOn, Text = "_" },
                new Token{Offset = 28, TokenType = TokenType.Normal, Text = "yet more underline" },
                new Token{Offset = 46, TokenType = TokenType.UnderlineOff, Text ="_"},
                new Token{Offset = 47, TokenType = TokenType.Normal, Text = " text." },
            });
        }

        [Test]
        public void Code()
        {
            var text = "some `code` text, and `yet more code` text.";
            //         "012345678901234567890123456789012345678901";
            var result = parser.Parse(text);

            result.Should().BeEquivalentTo(new List<Token>
            {
                new Token{Offset = 0, TokenType = TokenType.Normal, Text = "some " },
                new Token{Offset = 5, TokenType = TokenType.CodeOn, Text = "`" },
                new Token{Offset = 6, TokenType = TokenType.Normal, Text = "code" },
                new Token{Offset = 10, TokenType = TokenType.CodeOff, Text ="`"},
                new Token{Offset = 11, TokenType = TokenType.Normal, Text = " text, and " },
                new Token{Offset = 22, TokenType = TokenType.CodeOn, Text = "`" },
                new Token{Offset = 23, TokenType = TokenType.Normal, Text = "yet more code" },
                new Token{Offset = 36, TokenType = TokenType.CodeOff, Text ="`"},
                new Token{Offset = 37, TokenType = TokenType.Normal, Text = " text." },
            });
        }

        [Test]
        public void MultipleRunsWithoutNesting_ReturnsExpectedFormatTokens()
        {
            var text = "We have **bold**, *italic*, _underline_, and `code`.";
            //         "0123456789012345678901234567890123456789012345678901";
            var result = parser.Parse(text);

            result.Should().BeEquivalentTo(new List<Token>
            {
                new Token{Offset = 0, TokenType = TokenType.Normal, Text = "We have " },
                new Token{Offset = 8, TokenType = TokenType.BoldOn, Text = "**" },
                new Token{Offset = 10, TokenType = TokenType.Normal, Text = "bold" },
                new Token{Offset = 14, TokenType = TokenType.BoldOff, Text ="**"},
                new Token{Offset = 16, TokenType = TokenType.Normal, Text = ", " },
                new Token{Offset = 18, TokenType = TokenType.ItalicOn, Text = "*" },
                new Token{Offset = 19, TokenType = TokenType.Normal, Text = "italic" },
                new Token{Offset = 25, TokenType = TokenType.ItalicOff, Text ="*"},
                new Token{Offset = 26, TokenType = TokenType.Normal, Text = ", " },
                new Token{Offset = 28, TokenType = TokenType.UnderlineOn, Text = "_" },
                new Token{Offset = 29, TokenType = TokenType.Normal, Text = "underline" },
                new Token{Offset = 38, TokenType = TokenType.UnderlineOff, Text ="_"},
                new Token{Offset = 39, TokenType = TokenType.Normal, Text = ", and " },
                new Token{Offset = 45, TokenType = TokenType.CodeOn, Text = "`" },
                new Token{Offset = 46, TokenType = TokenType.Normal, Text = "code" },
                new Token{Offset = 50, TokenType = TokenType.CodeOff, Text ="`"},
                new Token{Offset = 51, TokenType = TokenType.Normal, Text = "." },
            });
        }

        [Test]
        public void MultipleRunsOfTheSameType_ReturnsMultipleFormatTokens()
        {
            var text = "We have **bold**, and then **more bold**.";
            //         "01234567890123456789012345678901234567890";
            var result = parser.Parse(text);

            result.Should().BeEquivalentTo(new List<Token>
            {
                new Token{Offset = 0, TokenType = TokenType.Normal, Text = "We have " },
                new Token{Offset = 8, TokenType = TokenType.BoldOn, Text = "**" },
                new Token{Offset = 10, TokenType = TokenType.Normal, Text = "bold" },
                new Token{Offset = 14, TokenType = TokenType.BoldOff, Text ="**"},
                new Token{Offset = 16, TokenType = TokenType.Normal, Text = ", and then " },
                new Token{Offset = 27, TokenType = TokenType.BoldOn, Text = "**" },
                new Token{Offset = 29, TokenType = TokenType.Normal, Text = "more bold" },
                new Token{Offset = 38, TokenType = TokenType.BoldOff, Text ="**"},
                new Token{Offset = 40, TokenType = TokenType.Normal, Text = "." },
            });
        }
    }
}

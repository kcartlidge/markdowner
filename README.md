# Markdowner

Fast-enough and efficient-enough Markdown parsing in pure C# for .Net Core and .Net Framework.

This is written as a personal challenge. I've done Markdown parsers before and have used parser generators. I've even written my own equivalents to yacc, bison, etc (not just the grammars). 

This was different, a challenge to do something by hand without using techniques such as recursive descent or compiler compilers.

It is not feature complete, but is perfectly fine for many simpler cases, supporting:

- Inline (nested) formatting
    - Bold, Italics, Underline, Code
- Headers from 1 to 6
- Paragraphs
    - Adjacent lines are merged into single paragraphs
- Ordered lists
- Unordered lists
- Preformatted blocks
- Block quotes
- Horizontal lines
- Tracking line numbers
    - Line number from the original Markdown
    - Line number in the parsed lines
- Unit tests

## Sample usage

Check out the [example project](./Example/Program.cs) in the `Example` folder.

## Progress

It's at an ALPHA stage and is a working proof of concept.

Missing:

- Packaging for Nuget
- Nested lists
- Tables
- Links
- Images

## Methodology

### There is a 2-step process

1. Convert the input text into a `MarkdownDocument` containing a collection of lines.
    - Each line is assigned a type, such as Paragraph or UnorderedList.
    - Each line has a `Token` collection holding all it's text and/or formatting.
1. Generate an output from that document.
    - An HTML output generator is included.

The caller does not need to know it is a 2-step process; they just feed the output generator the original source. The reason for splitting into 2 steps is that parsing into lines and tokens is the computationally expensive bit so doing that separately (a) provides a form of advance compilation and (b) means the document can be fed through multiple output generators without the overhead of parsing again each time.

### Parsing follows a simple flow

- Ignore leading empty lines
- Treat runs of empty lines as one single line
- Derive the line types from the line start characters
- Merge consecutive paragraphs (where not separated by empty lines)
- Generate a set of tokens to represent the line content
- Add line-level flags for start/stop markers in quote/pre blocks

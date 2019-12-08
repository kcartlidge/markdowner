

# Markdowner Sample File

This is a _sample_ file for **Markdowner**, the *C# Markdown parser*.
It's deliberately messy, for my manual testing.
It can be consumed _by any .Net Core or full *Framework code.

## Supported stuff

It supports the following:

- Headers using hashes (#,## etc)
- Embedded **bold, *italics*, and _underline_**
- Single-level lists (using dash or asterisk)
* Quotes, marked using an indent
-  Pre blocks, marked using `backtick` gates (x3 needed)
- Code, marked using inline `backticks`

1.  Numbered lists
1. Either CR/LF or LF for line endings

You'll see too (above) that lines of different types (header, text, list) do not need a separator, unlike paragraph sequences. Plus, multiple empty lines are compacted.

    This is a pre-block.
    It covers multiple lines, so the line outputs should be in one parent element.
    This is the only block type which is bracketed by stuff.
    In other cases the individual lines determine their block type.

Now for a quote:
> This is a **quote** block.
> It covers two lines, so the line outputs should be in one parent element.
We should now be back to a paragraph.

---

It also supports horizontal lines ('rules') using three dashes (*not* em-dashes!).



using System.Collections.Generic;

namespace Markdowner.Models
{
    public class MarkdownDocument
    {
        internal readonly string text = "";

        /// <summary>
        /// Original text after being compressed and tokenised.
        /// Trailing spaces, tabs, and carriage returns are removed.
        /// Leading and trailing empty lines are removed.
        /// Runs of empty lines are reduced to a single one.
        /// Runs of content lines without empty lines between are combined.
        /// Lines contain tokenised representations (for output generation).
        /// </summary>
        public List<Line> CompressedText { get; internal set; }

        /// <summary>
        /// The complete definition of a loaded collection of source.
        /// </summary>
        public MarkdownDocument(string text)
        {
            this.text = text;
        }
    }
}

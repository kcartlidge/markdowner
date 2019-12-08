namespace Markdowner.Enumerations
{
    /// <summary>
    /// The types of line understood by Markdowner.
    /// </summary>
    public enum LineType
    {
        Empty,

        Header1,
        Header2,
        Header3,
        Header4,
        Header5,
        Header6,

        OrderedList,
        UnorderedList,
        Pre,
        Quote,
        Rule,

        Paragraph,
    }
}

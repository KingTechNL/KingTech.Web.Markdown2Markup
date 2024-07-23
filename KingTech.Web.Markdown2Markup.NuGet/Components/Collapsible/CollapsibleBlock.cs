using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace KingTech.Web.Markdown2Markup.Components.Collapsible;

public class CollapsibleBlock : LeafBlock, IFencedBlock
{
    public CollapsibleBlock(BlockParser? parser) : base(parser)
    {
    }

    /// <summary>
    /// The title to render the current collapsible.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The content to render for the current TabPage.
    /// </summary>
    public string Content { get; set; }

    public int IndentCount { get; set; }

    public char FencedChar { get; set; }
    public int OpeningFencedCharCount { get; set; }
    public StringSlice TriviaAfterFencedChar { get; set; }
    public string? Info { get; set; }
    public StringSlice UnescapedInfo { get; set; }
    public StringSlice TriviaAfterInfo { get; set; }
    public string? Arguments { get; set; }
    public StringSlice UnescapedArguments { get; set; }
    public StringSlice TriviaAfterArguments { get; set; }
    public NewLine InfoNewLine { get; set; }
    public StringSlice TriviaBeforeClosingFence { get; set; }
    public int ClosingFencedCharCount { get; set; }
}
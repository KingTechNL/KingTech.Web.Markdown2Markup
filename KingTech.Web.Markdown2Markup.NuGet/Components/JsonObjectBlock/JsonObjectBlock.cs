using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Newtonsoft.Json.Linq;

namespace KingTech.Web.Markdown2Markup.Components.JsonObjectBlock;

public class JsonObjectBlock<TObject> : LeafBlock, IFencedBlock
    where TObject : IJsonComponent, new()
{
    public JsonObjectBlock(BlockParser? parser) : base(parser)
    {
    }

    /// <summary>
    /// The object parsed from Json.
    /// </summary>
    public TObject? Object { get; set; }

    /// <summary>
    /// The parsed <see cref="JObject"/>.
    /// </summary>
    public JObject JObject { get; set; }

    /// <summary>
    /// The raw json string.
    /// </summary>
    public string JsonString { get; set; }



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
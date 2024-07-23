using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;

namespace KingTech.Web.Markdown2Markup.Components.JsonObjectBlock;

public class JsonObjectBlockParser<TObject> : FencedBlockParserBase<JsonObjectBlock<TObject>>
    where TObject : IJsonComponent, new()
{
    public const string DefaultInfoPrefix = "language-";

    /// <summary>
    /// The closing characters for this block.
    /// </summary>
    public string Closing { get; set; }

    /// <summary>
    /// JsonObject block parser parses a <see cref="IComponent"/> object from json.
    /// </summary>
    /// <param name="openingCharacters">The opening characters for this block.</param>
    /// <param name="closingCharacters">The closing characters for this block.</param>
    public JsonObjectBlockParser(string openingCharacters = "[[[JO", string closingCharacters = "]]]")
    {
        OpeningCharacters = openingCharacters.ToCharArray();
        Closing = closingCharacters;
        InfoPrefix = DefaultInfoPrefix;
    }

    /// <summary>
    /// Open a new fenced block for image galleries.
    /// </summary>
    /// <param name="processor"></param>
    /// <returns></returns>
    protected override JsonObjectBlock<TObject> CreateFencedBlock(BlockProcessor processor)
    {
        var jObjectBlock = new JsonObjectBlock<TObject>(this)
        {
            IndentCount = processor.Indent,
        };

        if (processor.TrackTrivia)
        {
            jObjectBlock.LinesBefore = processor.LinesBefore;
            jObjectBlock.TriviaBefore = processor.UseTrivia(processor.Start - 1);
            jObjectBlock.NewLine = processor.Line.NewLine;
        }

        return jObjectBlock;
    }

    /// <summary>
    /// Look for the end of the fenced block.
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="block"></param>
    /// <returns></returns>
    public override BlockState TryContinue(BlockProcessor processor, Block block)
    {
        var jsonObjectBlock = (JsonObjectBlock<TObject>)block;

        var result = base.TryContinue(processor, block);

        if (result == BlockState.Continue && !processor.TrackTrivia)
        {
            // Remove any indent spaces
            var c = processor.CurrentChar;
            var indentCount = jsonObjectBlock.IndentCount;
            while (indentCount > 0 && c.IsSpace())
            {
                indentCount--;
                c = processor.NextChar();
            }

            //End block.
            var lastParsedLine = jsonObjectBlock.GetLastLine();
            if (lastParsedLine?.Equals(Closing) ?? false)
            {
                result = BlockState.BreakDiscard;
            }
            else if (lastParsedLine?.EndsWith(Closing) ?? false)
            {
                result = BlockState.Break;
            }
        }

        return result;
    }

    /// <summary>
    /// Called when block is closed.
    /// Process all parsed lines.
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="block"></param>
    /// <returns></returns>
    public override bool Close(BlockProcessor processor, Block block)
    {
        //Get all lines.
        var jsonObjectBlock = (JsonObjectBlock<TObject>)block;
        var lines = jsonObjectBlock.Lines.Lines
            .Where(l => !string.IsNullOrWhiteSpace(l.ToString()))
            .Select(l => l.ToString().Trim())
            .ToList();

        try
        {
            jsonObjectBlock.JsonString = "{" + string.Join("", lines.GetRange(0,lines.Count-1)) + "}";
            jsonObjectBlock.JObject = JObject.Parse(jsonObjectBlock.JsonString);
            jsonObjectBlock.Object =  jsonObjectBlock.JObject.ToObject<TObject>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to parse {typeof(TObject).Name}: {ex.Message}");
        }

        return base.Close(processor, block);
    }
}
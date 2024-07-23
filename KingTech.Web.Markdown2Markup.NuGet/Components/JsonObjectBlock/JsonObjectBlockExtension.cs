using Markdig;
using Markdig.Renderers;

namespace KingTech.Web.Markdown2Markup.Components.JsonObjectBlock;

public class JsonObjectBlockExtension<TObject> : IMarkdownExtension
    where TObject : IJsonComponent, new()
{
    private readonly string _openingCharacters;
    private readonly string _closingCharacters;

    public JsonObjectBlockExtension(string openingCharacters, string closingCharacters)
    {
        _openingCharacters = openingCharacters;
        _closingCharacters = closingCharacters;
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        var blockParsers = pipeline.BlockParsers;
        if (!blockParsers.Contains<JsonObjectBlockParser<TObject>>())
            blockParsers.Add(new JsonObjectBlockParser<TObject>(_openingCharacters, _closingCharacters));
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        var htmlRenderer = renderer as HtmlRenderer;
        var renderers = htmlRenderer?.ObjectRenderers;

        if (renderers != null && !renderers.Contains<JsonObjectBlockRenderer<TObject>>())
        {
            renderers.Add(new JsonObjectBlockRenderer<TObject>());
        }
    }
}
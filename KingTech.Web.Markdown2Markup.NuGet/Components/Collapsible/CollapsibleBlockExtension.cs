using Markdig;
using Markdig.Renderers;
using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup.Components.Collapsible;

public class CollapsibleBlockExtension : IMarkdownExtension
{
    private readonly IJSRuntime _jsRuntime;

    public CollapsibleBlockExtension(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        var blockParsers = pipeline.BlockParsers;
        if (!blockParsers.Contains<CollapsibleBlockParser>())
            blockParsers.Add(new CollapsibleBlockParser());
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        var htmlRenderer = renderer as HtmlRenderer;
        var renderers = htmlRenderer?.ObjectRenderers;

        if (renderers != null && !renderers.Contains<CollapsibleBlockRenderer>())
        {
            renderers.Add(new CollapsibleBlockRenderer(_jsRuntime));
        }
    }
}
using Markdig;
using Markdig.Renderers;
using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup.Components.ParallaxImage;

public class ParallaxImageBlockExtension : IMarkdownExtension
{
    private readonly IJSRuntime _jsRuntime;

    public ParallaxImageBlockExtension(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        var blockParsers = pipeline.BlockParsers;
        if (!blockParsers.Contains<ParallaxImageBlockParser>())
            blockParsers.Add(new ParallaxImageBlockParser());
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        var htmlRenderer = renderer as HtmlRenderer;
        var renderers = htmlRenderer?.ObjectRenderers;

        if (renderers != null && !renderers.Contains<ParallaxImageBlockRenderer>())
        {
            renderers.Add(new ParallaxImageBlockRenderer(_jsRuntime));
        }
    }
}
using Markdig;
using Markdig.Renderers;
using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup.Components.ImageGallery;

public class ImageGalleryExtension : IMarkdownExtension
{
    private readonly IJSRuntime _jsRuntime;

    public ImageGalleryExtension(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        var blockParsers = pipeline.BlockParsers;
        if (!blockParsers.Contains<ImageGalleryBlockParser>())
            blockParsers.Add(new ImageGalleryBlockParser());
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        var htmlRenderer = renderer as HtmlRenderer;
        var renderers = htmlRenderer?.ObjectRenderers;

        if (renderers != null && !renderers.Contains<ImageGalleryRenderer>())
        {
            renderers.Add(new ImageGalleryRenderer(_jsRuntime));
        }
    }
}
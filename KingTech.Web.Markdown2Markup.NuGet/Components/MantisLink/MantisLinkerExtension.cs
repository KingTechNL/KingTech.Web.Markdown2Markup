using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace KingTech.Web.Markdown2Markup.Components.MantisLink;

/// <summary>
/// Example class from:
/// https://www.codeproject.com/Articles/1200545/Writing-Custom-Markdig-Extensions
/// </summary>
public class MantisLinkerExtension : IMarkdownExtension
{
    private readonly MantisLinkOptions _options;

    public MantisLinkerExtension(MantisLinkOptions options)
    {
        _options = options;
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        OrderedList<InlineParser> parsers;

        parsers = pipeline.InlineParsers;

        if (!parsers.Contains<MantisLinkInlineParser>())
        {
            parsers.Add(new MantisLinkInlineParser());
        }
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        HtmlRenderer htmlRenderer;
        ObjectRendererCollection renderers;

        htmlRenderer = renderer as HtmlRenderer;
        renderers = htmlRenderer?.ObjectRenderers;

        if (renderers != null && !renderers.Contains<MantisLinkRenderer>())
        {
            renderers.Add(new MantisLinkRenderer(_options));
        }
    }
}
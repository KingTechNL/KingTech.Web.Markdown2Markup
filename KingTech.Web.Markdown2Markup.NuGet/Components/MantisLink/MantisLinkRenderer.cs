using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace KingTech.Web.Markdown2Markup.Components.MantisLink;

/// <summary>
/// Example class from:
/// https://www.codeproject.com/Articles/1200545/Writing-Custom-Markdig-Extensions
/// </summary>
public class MantisLinkRenderer : HtmlObjectRenderer<Markdown2Markup.Components.MantisLink.MantisLink>
{
    private MantisLinkOptions _options;

    public MantisLinkRenderer(MantisLinkOptions options)
    {
        _options = options;
    }

    protected override void Write(HtmlRenderer renderer, Markdown2Markup.Components.MantisLink.MantisLink obj)
    {
        StringSlice issueNumber;

        issueNumber = obj.IssueNumber;

        if (renderer.EnableHtmlForInline)
        {
            renderer.Write("<a href=\"").Write
                (_options.Url).Write(issueNumber).Write('"');

            if (_options.OpenInNewWindow)
            {
                renderer.Write(" target=\"blank\" rel=\"noopener noreferrer\"");
            }

            renderer.Write('>').Write('#').Write(issueNumber).Write("</a>");
        }
        else
        {
            renderer.Write('#').Write(obj.IssueNumber);
        }
    }
}
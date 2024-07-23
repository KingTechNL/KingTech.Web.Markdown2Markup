using BlazorTemplater;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup.Components.Collapsible;

public class CollapsibleBlockRenderer : HtmlObjectRenderer<CollapsibleBlock>
{
    private readonly IJSRuntime _jsRuntime;

    public CollapsibleBlockRenderer(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Create HTML image gallery based on the given <see cref="CollapsibleBlock"/>.
    /// </summary>
    /// <param name="renderer">The renderer to write the image gallery to.</param>
    /// <param name="collapsibleBlock">The <see cref="CollapsibleBlock"/> to render.</param>
    protected override void Write(HtmlRenderer renderer, CollapsibleBlock collapsibleBlock)
    {
        var content = MarkdownRenderer.RenderMarkdown(collapsibleBlock.Content.Trim(), _jsRuntime);
        
        var html = string.Empty;
        try
        {
            html = new ComponentRenderer<Component.Collapsible>()
                .AddService<IJSRuntime>(_jsRuntime)
                .Set(component => component.Title, collapsibleBlock.Title)
                .Set(component => component.RawChildContent, content)
                .Render();
        }
        catch (Exception e)
        {
            //_logger.LogError(e, "Failed to render image library.");
            Console.WriteLine(e);
        }
        renderer.Write(html);
    }
}
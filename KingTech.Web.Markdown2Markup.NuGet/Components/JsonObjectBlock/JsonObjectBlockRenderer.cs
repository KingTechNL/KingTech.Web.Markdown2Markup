using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace KingTech.Web.Markdown2Markup.Components.JsonObjectBlock;

public class JsonObjectBlockRenderer<TObject> : HtmlObjectRenderer<JsonObjectBlock<TObject>>
    where TObject : IJsonComponent, new()
{
    public JsonObjectBlockRenderer()
    {
        
    }

    /// <summary>
    /// Create an HTML object based on the given <see cref="JsonObjectBlock{TObject}"/>.
    /// </summary>
    /// <param name="renderer">The renderer to write the object to.</param>
    /// <param name="jsonObjectBlock">The <see cref="JsonObjectBlock{TObject}"/> to render.</param>
    protected override void Write(HtmlRenderer renderer, JsonObjectBlock<TObject> jsonObjectBlock)
    {
        var html = jsonObjectBlock.Object?.ToHtmlString();

        renderer.Write(html ?? string.Empty);
    }
}
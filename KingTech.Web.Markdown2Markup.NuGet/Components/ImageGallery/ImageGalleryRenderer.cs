using BlazorTemplater;
using KingTech.Web.Markdown2Markup.Components.ImageGallery.Component;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup.Components.ImageGallery;

public class ImageGalleryRenderer : HtmlObjectRenderer<ImageGalleryBlock>
{
    private readonly IJSRuntime _jsRuntime;

    public ImageGalleryRenderer(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Create HTML image gallery based on the given <see cref="ImageGalleryBlock"/>.
    /// </summary>
    /// <param name="renderer">The renderer to write the image gallery to.</param>
    /// <param name="imageGalleryBlock">The <see cref="ImageGalleryBlock"/> to render.</param>
    protected override void Write(HtmlRenderer renderer, ImageGalleryBlock imageGalleryBlock)
    {
        var items = imageGalleryBlock.Images.Select(image => new ECommerceImageGalleryItem()
        {
            Caption = image.Caption,
            ImageUrl = image.ImageUrl,
            ThumbnailUrl = image.ThumbnailUrl
        }).ToList();
        
        var html = string.Empty;
        try
        {
            html = new ComponentRenderer<ECommerceImageGallery>()
                .AddService<IJSRuntime>(_jsRuntime)
                .Set(component => component.Identifier, imageGalleryBlock.Identifier)
                .Set(component => component.Items, items)
                .Set(component => component.Loading, imageGalleryBlock.Loading)
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
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup.Components.ImageGallery.Component;

/// <summary>
/// This component displays a slide show of images.
/// </summary>
public partial class ECommerceImageGallery : ComponentBase
{
    /// <summary>
    /// Items shown in this image gallery.
    /// </summary>
    [Parameter]
    public List<ECommerceImageGalleryItem> Items { get; set; }

    /// <summary>
    /// Unique identifier for this image gallery.
    /// </summary>
    [Parameter]
    public string Identifier { get; set; }

    /// <summary>
    /// Loading attribute set for all images in this gallery.
    /// </summary>
    [Parameter]
    public string Loading { get; set; } = "lazy";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Task.Delay(1000);
            await JS.InvokeVoidAsync("currentSlide", Identifier, 1);
        }
        await base.OnAfterRenderAsync(firstRender);
    }
}
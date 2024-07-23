namespace KingTech.Web.Markdown2Markup.Components.ImageGallery.Component;

/// <summary>
/// Image with metadata to show in image gallery.
/// </summary>
public class ECommerceImageGalleryItem
{
    /// <summary>
    /// Url to the (high definition) image.
    /// </summary>
    public string ImageUrl { get; set; }
    /// <summary>
    /// Url to the (low definition) image thumbnail.
    /// </summary>
    public string ThumbnailUrl { get; set; }
    /// <summary>
    /// Caption to display under this image.
    /// </summary>
    public string Caption { get; set; }
}
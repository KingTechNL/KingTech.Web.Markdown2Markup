using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace KingTech.Web.Markdown2Markup.Components.ImageGallery;

public class ImageGalleryBlock : LeafBlock, IFencedBlock
{
    public ImageGalleryBlock(BlockParser? parser) : base(parser)
    {
    }

    /// <summary>
    /// Unique identifier for this image gallery.
    /// </summary>
    public string Identifier { get; set; }

    /// <summary>
    /// Caption of the image gallery.
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    /// List of image url's.
    /// </summary>
    public List<ImageGalleryImage> Images { get; set; } = new ();

    /// <summary>
    /// The loading attribute on the images for this gallery.
    /// </summary>
    /// <remarks>Default = lazy.</remarks>
    public string Loading { get; set; } = "lazy";

    public int IndentCount { get; set; }

    public char FencedChar { get; set; }
    public int OpeningFencedCharCount { get; set; }
    public StringSlice TriviaAfterFencedChar { get; set; }
    public string? Info { get; set; }
    public StringSlice UnescapedInfo { get; set; }
    public StringSlice TriviaAfterInfo { get; set; }
    public string? Arguments { get; set; }
    public StringSlice UnescapedArguments { get; set; }
    public StringSlice TriviaAfterArguments { get; set; }
    public NewLine InfoNewLine { get; set; }
    public StringSlice TriviaBeforeClosingFence { get; set; }
    public int ClosingFencedCharCount { get; set; }
}

/// <summary>
/// Container class for the image metadata.
/// </summary>
public class ImageGalleryImage
{
    /// <summary>
    /// Url for this image.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;
    /// <summary>
    /// Url for the thumbnail of this image.
    /// </summary>
    public string ThumbnailUrl { get; set; } = string.Empty;
    /// <summary>
    /// Caption for this image.
    /// </summary>
    public string Caption { get; set; } = string.Empty;
}
using KingTech.Web.Markdown2Markup.Components.JsonObjectBlock;
using Microsoft.AspNetCore.Components;

namespace KingTech.Web.Markdown2Markup.Components.ParallaxImage.Component;

/// <summary>
/// A parallax is a stack of images that scroll dynamically, creating a partial 3D effect.
/// Origin:
/// https://github.com/KiaanCastillo/Pure-CSS-Multilayer-Parallax
/// </summary>
public partial class Parallax : ComponentBase, IJsonComponent
{
    /// <summary>
    /// The hero content of this parallax.
    /// Shown over image.
    /// </summary>
    [Parameter]
    public RenderFragment? HeroContent { get; set; }

    /// <summary>
    /// The hero content of this parallax as a raw string.
    /// Shown over the image.
    /// </summary>
    public string RawHeroContent
    {
        set => HeroContent = CreateRenderFragment(value);
        get => HeroContent?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// All parallax css classes to overlay.
    /// These classes should contain the correct background, background-size: cover, transform: translateZ(...) scale(...) and z-index.
    /// </summary>
    /// <remarks>
    /// It is recommended to use the ParallaxLayers property for this.
    /// If the ParallaxLayers property is not empty, this property will be ignored.
    /// </remarks>
    [Parameter]
    public IEnumerable<string> ParallaxClasses { get; set; } = new List<string>();

    /// <summary>
    /// The different (image) layers that will be overlapped in order to create a smooth scrolling parallax.
    /// </summary>
    [Parameter] public IEnumerable<ParallaxLayer> ParallaxLayers { get; set; } = new List<ParallaxLayer>();

    /// <summary>
    /// The content to render for the current page.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The content to render for this page, as raw string.
    /// </summary>
    public string RawChildContent
    {
        set => ChildContent = CreateRenderFragment(value);
        get => ChildContent?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// The height of this parallex in vh (1vh = 1% of the current view height).
    /// </summary>
    [Parameter] 
    public int Height { get; set; } = 100;

    /// <summary>
    /// Create a <see cref="RenderFragment"/> from a raw content string.
    /// </summary>
    /// <param name="rawContent">The raw content string to create a <see cref="RenderFragment"/> for.</param>
    /// <returns>The created <see cref="RenderFragment"/>.</returns>
    private RenderFragment CreateRenderFragment(string rawContent) => builder =>
    {
        builder.AddContent(1, rawContent);
    };

    public string ToHtmlString() => "Implement parallax html.";
}

/// <summary>
/// A layer of a parallax that will be used to create a partial 3D effect.
/// </summary>
public class ParallaxLayer
{
    /// <summary>
    /// Background image for this layer.
    /// </summary>
    public string BackgroundImage { get; set; }

    /// <summary>
    /// Z offset for this layer.
    /// </summary>
    public int TranslateZ { get; set; }

    /// <summary>
    /// Scale for this layer.
    /// </summary>
    public double Scale { get; set; }

    /// <summary>
    /// Additional custom css can be set.
    /// This is added to the parralax layer as inline-css.
    /// </summary>
    public string CustomCss { get; set; } = string.Empty;

    /// <summary>
    /// Given TranslateZ converted to be used by the parallax.
    /// </summary>
    internal string TranslateZPixels => $"{TranslateZ}px";

    /// <summary>
    /// Given Scale converted to be used by the parallax.
    /// </summary>
    internal string ScaleString => Scale.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
}
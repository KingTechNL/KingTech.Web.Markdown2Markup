using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace KingTech.Web.Markdown2Markup.Components.ParallaxImage;

public class ParallaxImageBlock : LeafBlock, IFencedBlock
{
    public ParallaxImageBlock(BlockParser? parser) : base(parser)
    {
    }

    public string Identifier { get; set; }

    /// <summary>
    /// The hero content of this parallax.
    /// Shown over image.
    /// </summary>
    public MarkdownDocument? HeroContent { get; set; }

    /// <summary>
    /// All parallax css classes to overlay.
    /// These classes should contain the correct background, background-size: cover, transform: translateZ(...) scale(...) and z-index.
    /// </summary>
    /// <remarks>
    /// It is recommended to use the ParallaxLayers property for this.
    /// If the ParallaxLayers property is not empty, this property will be ignored.
    /// </remarks>
    public IEnumerable<string> ParallaxClasses { get; set; } = new List<string>();

    /// <summary>
    /// The different (image) layers that will be overlapped in order to create a smooth scrolling parallax.
    /// </summary>
    public IEnumerable<ParallaxImageBlockLayer> ParallaxLayers { get; set; } = new List<ParallaxImageBlockLayer>();

    /// <summary>
    /// The content to render for the current TabPage.
    /// </summary>
    public MarkdownDocument? ChildContent { get; set; }

    /// <summary>
    /// The height of this parallax in vh (1vh = 1% of the current view height).
    /// </summary>
    public int Height { get; set; }


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
/// A layer of a parallax that will be used to create a partial 3D effect.
/// </summary>
public class ParallaxImageBlockLayer
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
}
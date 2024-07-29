using BlazorTemplater;
using KingTech.Web.Markdown2Markup.Components.ParallaxImage.Component;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup.Components.ParallaxImage;

public class ParallaxImageBlockRenderer : HtmlObjectRenderer<ParallaxImageBlock>
{
    private readonly IJSRuntime _jsRuntime;

    public ParallaxImageBlockRenderer(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Create HTML image gallery based on the given <see cref="ParallaxImageBlock"/>.
    /// </summary>
    /// <param name="renderer">The renderer to write the image gallery to.</param>
    /// <param name="parallaxImageBlock">The <see cref="ParallaxImageBlock"/> to render.</param>
    protected override void Write(HtmlRenderer renderer, ParallaxImageBlock parallaxImageBlock)
    {
        var layers = parallaxImageBlock.ParallaxLayers.Select(image => new ParallaxLayer()
        {
            BackgroundImage = image.BackgroundImage,
            TranslateZ = image.TranslateZ,
            Scale = image.Scale,
            CustomCss = image.CustomCss,
        }).ToList();

        var html = string.Empty;
        try
        {
            html = new ComponentRenderer<Parallax>()
                .AddService<IJSRuntime>(_jsRuntime)
                .Set(component => component.Height, parallaxImageBlock.Height)
                .Set(component => component.RawHeroContent, parallaxImageBlock.HeroContent?.ToString())
                .Set(component => component.RawChildContent, parallaxImageBlock.ChildContent?.ToString())
                .Set(component => component.ParallaxClasses, parallaxImageBlock.ParallaxClasses)
                .Set(component => component.ParallaxLayers, layers)
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
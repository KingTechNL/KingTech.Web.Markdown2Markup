using KingTech.Web.Markdown2Markup.Components.Collapsible;
using KingTech.Web.Markdown2Markup.Components.ImageGallery;
using KingTech.Web.Markdown2Markup.Components.JsonObjectBlock;
using KingTech.Web.Markdown2Markup.Components.MantisLink;
using Markdig;
using Markdig.Syntax;
using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup;

/// <summary>
/// Extension methods to add custom parsers/renderers to the markdown rendering.
/// </summary>
public static class Markdown2MarkupExtensionMethods
{
    /// <summary>
    /// Use parsing/rendering of mantis links from markdown.
    /// </summary>
    /// <param name="pipeline">The builder to add the <see cref="MantisLinkerExtension"/> to.</param>
    /// <param name="options">The <see cref="MantisLinkOptions"/> to add.</param>
    /// <returns></returns>
    public static MarkdownPipelineBuilder UseMantisLinks(this MarkdownPipelineBuilder pipeline, MantisLinkOptions options)
    {
        var extensions = pipeline.Extensions;

        if (!extensions.Contains<MantisLinkerExtension>())
            extensions.Add(new MantisLinkerExtension(options));

        return pipeline;
    }

    /// <summary>
    /// Use parsing/rendering of <see cref="ImageGalleryBlock"/>s from markdown.
    /// </summary>
    /// <param name="pipeline">The builder to add the <see cref="ImageGalleryExtension"/> to.</param>
    /// <param name="jsRuntime">Runtime javascript to inject.</param>
    /// <returns></returns>
    public static MarkdownPipelineBuilder UseImageGallery(this MarkdownPipelineBuilder pipeline, IJSRuntime jsRuntime)
    {
        var extensions = pipeline.Extensions;

        if (!extensions.Contains<ImageGalleryExtension>())
            extensions.Add(new ImageGalleryExtension(jsRuntime));

        return pipeline;
    }

    /// <summary>
    /// Use parsing/rendering of <see cref="ImageGalleryBlock"/>s from markdown.
    /// </summary>
    /// <param name="pipeline">The builder to add the <see cref="ImageGalleryExtension"/> to.</param>
    /// <param name="jsRuntime">Runtime javascript to inject.</param>
    /// <returns></returns>
    public static MarkdownPipelineBuilder UseCollapsible(this MarkdownPipelineBuilder pipeline, IJSRuntime jsRuntime)
    {
        var extensions = pipeline.Extensions;

        if (!extensions.Contains<CollapsibleBlockExtension>())
            extensions.Add(new CollapsibleBlockExtension(jsRuntime));

        return pipeline;
    }

    /// <summary>
    /// Use parsing/rendering of <see cref="JsonObjectBlock{TObject}"/>s from markdown.
    /// </summary>
    /// <typeparam name="TObject">The actual <see cref="IJsonComponent"/> to parse.</typeparam>
    /// <param name="pipeline">The builder to add the <see cref="JsonObjectBlockExtension{TObject}"/> to.</param>
    /// <param name="openingCharacters">String denoting the start of the <see cref="JsonObjectBlock{TObject}"/>.</param>
    /// <param name="closingCharacters">String denoting the end of the <see cref="JsonObjectBlock{TObject}"/>.</param>
    /// <returns></returns>
    public static MarkdownPipelineBuilder UseJsonObjectParsing<TObject>(this MarkdownPipelineBuilder pipeline, string openingCharacters, string closingCharacters)
        where TObject : IJsonComponent, new()
    {
        var extensions = pipeline.Extensions;

        if (!extensions.Contains<JsonObjectBlockExtension<TObject>>())
            extensions.Add(new JsonObjectBlockExtension<TObject>(openingCharacters, closingCharacters));

        return pipeline;
    }

    /// <summary>
    /// Get the last line of a <see cref="LeafBlock"/>.
    /// </summary>
    /// <param name="block">The <see cref="LeafBlock"/> to get the last line for.</param>
    /// <returns>The last line of the given <see cref="LeafBlock"/>.</returns>
    public static string GetLastLine(this LeafBlock block) => block.Lines.Lines?.Where(l => !string.IsNullOrWhiteSpace(l.ToString())).LastOrDefault().ToString();
}
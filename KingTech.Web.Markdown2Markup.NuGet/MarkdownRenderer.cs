using Humanizer;
using KingTech.Web.Markdown2Markup.Components.MantisLink;
using KingTech.Web.Markdown2Markup.Models;
using Markdig;
using Markdig.Syntax;
using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup;

/// <summary>
/// Static markdown renderer.
/// This is the main class for the Markdown2Markup package.
/// </summary>
public static class MarkdownRenderer
{
    /// <summary>
    /// Internal pipeline builder.
    /// </summary>
    public static MarkdownPipelineBuilder? Builder { get; private set; }

    /// <summary>
    /// Set the default markdown pipeline. Enables all components available to this package.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> that will be used for rendering some dynamic components.</param>
    public static void SetDefaultMarkdownPipelineBuilder(IJSRuntime jsRuntime, string mantislinkBaseUrl = "www.kingtech.nl") => SetMarkdownPipelineBuilder(
        new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .ConfigureNewLine("\r\n")
            .UseNonAsciiNoEscape()
            .UseGenericAttributes()
            .UseEmojiAndSmiley() //https://gist.github.com/rxaviers/7360908
            .UseCitations()
            .UseDiagrams()
            .UseFigures()
            .UseFooters()
            .UseFootnotes()
            //.UseJsonObjectParsing<Parallax>("[[[PX", "]]]")
            .UseCollapsible(jsRuntime)
            //.UseBootstrap()
            .UseImageGallery(jsRuntime)
            .UseMantisLinks(new MantisLinkOptions(mantislinkBaseUrl))
        );

    /// <summary>
    /// Set a custom <see cref="MarkdownPipelineBuilder"/> that will be used for rendering markdown.
    /// This allows you to add custom components to the markdown rendering.
    /// </summary>
    /// <param name="builder">The <see cref="MarkdownPipelineBuilder"/> to use.</param>
    public static void SetMarkdownPipelineBuilder(MarkdownPipelineBuilder builder)
    {
        Builder = builder;
    }

    /// <summary>
    /// Render a markdown string into HTML.
    /// If no builder is set, this method will use the default markdown builder.
    /// </summary>
    /// <param name="markdown">The markdown string to render into HTML.</param>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> that will be used for rendering some dynamic components.</param>
    /// <returns>A HTML string rendered from the given markdown. Null if no HTML could be rendered.</returns>
    public static string RenderMarkdown(string markdown, IJSRuntime jsRuntime)
    {
        if (Builder == null)
            SetDefaultMarkdownPipelineBuilder(jsRuntime);
        return RenderMarkdown(markdown);
    }

    /// <summary>
    /// Render a markdown string into HTML.
    /// If no builder is set, this method will return null.
    /// </summary>
    /// <param name="markdown">The markdown string to render into HTML.</param>
    /// <returns>A HTML string rendered from the given markdown. Null if no HTML could be rendered.</returns>
    public static string RenderMarkdown(string markdown)
    {
        if (Builder == null)
            return null;
        
        var pipeline = Builder.Build();
        var document = Markdig.Markdown.Parse(markdown, pipeline);
        var html = document.ToHtml(pipeline);
        return html;
    }

    /// <summary>
    /// Get a recursive list of chapters from a markdown string.
    /// This list can be used by the <see cref="KingTech.Web.Markdown2Markup.Components.Navigation"/> component to render a table of contents.""/>
    /// </summary>
    /// <param name="markdown">The markdown to get the chapters from.</param>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> that will be used for rendering some dynamic components.</param>
    /// <returns>A recursive list of chapters.</returns>
    public static List<TableOfContentsNode> GetChapters(string markdown, IJSRuntime jsRuntime)
    {
        if (Builder == null)
            SetDefaultMarkdownPipelineBuilder(jsRuntime);

        return GetChapters(markdown);
    }

    /// <summary>
    /// Get a recursive list of chapters from a markdown string.
    /// This list can be used by the <see cref="KingTech.Web.Markdown2Markup.Components.Navigation"/> component to render a table of contents.""/>
    /// </summary>
    /// <param name="markdown">The markdown to get the chapters from.</param>
    /// <returns>A recursive list of chapters.</returns>
    public static List<TableOfContentsNode> GetChapters(string markdown) 
    {
        if (Builder == null)
            return null;

        var pipeline = Builder.Build();

        var document = Markdown.Parse(markdown, pipeline);

        var root = new TableOfContentsNode { Name = "Root", Level = 0 };
        var stack = new Stack<TableOfContentsNode>();
        stack.Push(root);

        foreach (var heading in document.Descendants<HeadingBlock>())
        {
            var title = string.Concat(heading.Inline);
            var node = new TableOfContentsNode { 
                Name = title.Humanize(), 
                HRef = GenerateAnchorId(title),
                Level = heading.Level 
            };

            while (stack.Peek().Level >= node.Level)
                stack.Pop();

            stack.Peek().SubChapters.Add(node);
            stack.Push(node);
        }

        return root.SubChapters;

    }

    /// <summary>
    /// Generates a URL-friendly anchor ID from heading text.
    /// </summary>
    /// <param name="headingText">The heading text to convert.</param>
    /// <returns>A URL-friendly anchor ID.</returns>
    private static string GenerateAnchorId(string headingText)
    {
        if (string.IsNullOrWhiteSpace(headingText))
            return string.Empty;

        return headingText
            .ToLowerInvariant()
            .Replace(" ", "-")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("!", "")
            .Replace("?", "")
            .Replace(":", "")
            .Replace(";", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("[", "")
            .Replace("]", "")
            .Replace("{", "")
            .Replace("}", "")
            .Replace("/", "")
            .Replace("\\", "")
            .Replace("\"", "")
            .Replace("'", "")
            .Replace("&", "and")
            .Replace("--", "-")
            .Trim('-');
    }
}
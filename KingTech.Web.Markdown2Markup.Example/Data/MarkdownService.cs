using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup.Example.Data;

public class MarkdownService
{
    private readonly ILogger<MarkdownService> _logger;
    private readonly IJSRuntime _jsRuntime;

    public MarkdownService(ILogger<MarkdownService> logger, IJSRuntime jsRuntime)
    {
        _logger = logger;
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Get the source markdown and fully rendered HTML (+js) for the given file.
    /// </summary>
    /// <param name="fileName">The file to get the content and rendered HTML for.</param>
    /// <returns>source markdown and fully rendered HTML (+js) from the given file, null if file could not be found.</returns>
    public (string Markdown, string Html)? GetRenderedFile(string fileName)
    {
        var content = ReadContentFromFile(fileName);
        if (content == null)
            return null;

        var html = RenderMarkdown(content);
        return (content, html);
    }

    /// <summary>
    /// Render HTML from a markdown string.
    /// </summary>
    /// <param name="markdown">The markdown string to render.</param>
    /// <returns>An HTML string, if one could be rendered. Null otherwise.</returns>
    public string? RenderMarkdown(string markdown) => MarkdownRenderer.RenderMarkdown(markdown, _jsRuntime);

    /// <summary>
    /// Read the content from the given file.
    /// </summary>
    /// <param name="fileName">The name of the file to parse.</param>
    /// <returns>The parsed content from the file (as a RAW string).</returns>
    public string? ReadContentFromFile(string fileName)
    {
        try
        {
            using var r = new StreamReader(fileName);
            return r.ReadToEnd();
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Failed to parse content from {fileName}", fileName);
            return null;
        }
    }
}
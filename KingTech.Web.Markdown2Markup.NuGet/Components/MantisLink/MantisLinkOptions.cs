namespace KingTech.Web.Markdown2Markup.Components.MantisLink;

/// <summary>
/// Example class from:
/// https://www.codeproject.com/Articles/1200545/Writing-Custom-Markdig-Extensions
/// </summary>
public class MantisLinkOptions
{
    public MantisLinkOptions()
    {
        this.OpenInNewWindow = true;
    }

    public MantisLinkOptions(string url)
        : this()
    {
        this.Url = url;
    }

    public MantisLinkOptions(Uri uri)
        : this()
    {
        this.Url = uri.OriginalString;
    }

    public bool OpenInNewWindow { get; set; }

    public string Url { get; set; }
}
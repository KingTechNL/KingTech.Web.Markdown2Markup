using System.Diagnostics;
using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace KingTech.Web.Markdown2Markup.Components.MantisLink;

/// <summary>
/// Example class from:
/// https://www.codeproject.com/Articles/1200545/Writing-Custom-Markdig-Extensions
/// </summary>
[DebuggerDisplay("#{" + nameof(IssueNumber) + "}")]
public class MantisLink : LeafInline
{
    public StringSlice IssueNumber { get; set; }
}
using Microsoft.AspNetCore.Components;

namespace KingTech.Web.Markdown2Markup.Components.JsonObjectBlock;

/// <summary>
/// Interface for components that can be rendered from json in markdown.
/// </summary>
public interface IJsonComponent : IComponent
{
    /// <summary>
    /// Get the raw HTML for this aspnet core component.
    /// </summary>
    /// <returns>The raw HTML string for this component.</returns>
    public string ToHtmlString();
}
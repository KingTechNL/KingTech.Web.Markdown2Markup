using Microsoft.AspNetCore.Components;

namespace KingTech.Web.Markdown2Markup.Components.Collapsible.Component;

public partial class Collapsible : ComponentBase
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? ChildContent { get; set; } = null!;

    [Parameter]
    public string? RawChildContent { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        
    }
}
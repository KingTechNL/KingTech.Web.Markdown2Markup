namespace KingTech.Web.Markdown2Markup.Models;

/// <summary>
/// A node in the chapter tree.
/// </summary>
public class TableOfContentsNode
{
    /// <summary>
    /// The name of the chapter.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The file or repository entry to fetch this chapter.
    /// </summary>
    public string? HRef { get; init; }

    /// <summary>
    /// A list of sub-chapters.
    /// </summary>
    public List<TableOfContentsNode> SubChapters { get; set; } = new List<TableOfContentsNode>();

    /// <summary>
    /// The index of this node in respect to its siblings.
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// The level of the chapter in the hierarchy.
    /// </summary>
    public int Level { get; init; }

    public override string ToString()
    {
        var str = $"{new string(' ', Level * 2)}{Name}\n";
        foreach (var subChapter in SubChapters)
        {
            str += subChapter.ToString();
        }
        return str;
    }
}

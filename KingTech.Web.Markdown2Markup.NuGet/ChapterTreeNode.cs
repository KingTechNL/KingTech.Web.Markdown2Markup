namespace KingTech.Web.Markdown2Markup;

/// <summary>
/// A node in the chapter tree.
/// </summary>
public class ChapterTreeNode
{
    /// <summary>
    /// The name of the chapter.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// A list of sub-chapters.
    /// </summary>
    public List<ChapterTreeNode> SubChapters { get; init; } = new List<ChapterTreeNode>();

    /// <summary>
    /// The level of the chapter in the hierarchy.
    /// </summary>
    public int Level { get; init; }
}

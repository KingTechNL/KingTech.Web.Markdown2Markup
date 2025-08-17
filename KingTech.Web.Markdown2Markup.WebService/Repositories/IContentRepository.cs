using KingTech.Web.Markdown2Markup.Models;

namespace KingTech.Web.Markdown2Markup.WebService.Repositories
{
    /// <summary>
    /// Repository from fetching content and metadata from a source system (e.g. a database or file system)
    /// </summary>
    public interface IContentRepository
    {
        /// <summary>
        /// Get the global table of contents.
        /// </summary>
        /// <returns>A list of <see cref="TableOfContentsNode"/>s.</returns>
        List<TableOfContentsNode> GetTableOfContents();

        /// <summary>
        /// Retrieves the reference point of the application's main page.
        /// </summary>
        /// <returns>A string containing the reference of the main page.</returns>
        string GetMainPage();

        /// <summary>
        /// Retrieves the (markdown) content of a specific page based on its reference.
        /// </summary>
        /// <param name="reference">The reference string to fetch the content by.</param>
        /// <returns>The (markdown) content for the given reference.</returns>
        string GetContent(string reference);
    }
}
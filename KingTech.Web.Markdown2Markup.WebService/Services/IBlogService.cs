using KingTech.Web.Markdown2Markup.Models;

namespace KingTech.Web.Markdown2Markup.WebService.Services
{
    /// <summary>
    /// Service for providing the blogs and metadata to the frontend.
    /// </summary>
    public interface IBlogService
    {
        /// <summary>
        /// Get the global table of contents.
        /// This is used to build the navigation menu.
        /// </summary>
        /// <returns>A list of <see cref="TableOfContentsNode"/>s.</returns>
        List<TableOfContentsNode> GetTableOfContents();

        /// <summary>
        /// Retrieve a reference to the main (home) page for this blog.
        /// </summary>
        /// <returns>A reference to the main (home) page for this blog.</returns>
        string GetMainPage();

        /// <summary>
        /// Get the (html) content of a specific page based on its reference.
        /// </summary>
        /// <param name="reference">The reference string to fetch the content for.</param>
        /// <returns>The (html) content for the given reference.</returns>
        string GetContent(string reference);

        /// <summary>
        /// Get all the chapters for the given reference.
        /// This is used to build the small TableOfContents element.
        /// </summary>
        /// <param name="reference">The reference string to fetch the content for.</param>
        /// <returns>A list of <see cref="TableOfContentsNode"/>s representing each chapter for the given reference.</returns>
        public List<TableOfContentsNode>? GetChapters(string reference);
    }
}
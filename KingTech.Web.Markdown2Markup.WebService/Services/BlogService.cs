using KingTech.Web.Markdown2Markup.Models;
using KingTech.Web.Markdown2Markup.WebService.Repositories;
using Microsoft.JSInterop;

namespace KingTech.Web.Markdown2Markup.WebService.Services
{
    /// <inheritdoc/>
    public class BlogService : IBlogService
    {
        private readonly ILogger<BlogService> logger;
        private readonly IContentRepository repository;
        private readonly IJSRuntime jsRuntime;

        /// <summary>
        /// This class is responsible for providing the blogs and metadata to the frontend.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> for logging from this class.</param>
        /// <param name="repository">The <see cref="IContentRepository"/> used for fetching the data.</param>
        public BlogService(ILogger<BlogService> logger, IContentRepository repository, IJSRuntime jsRuntime)
        {
            this.logger = logger;
            this.repository = repository;
            this.jsRuntime = jsRuntime;
        }

        /// <inheritdoc/>
        public string GetContent(string reference)
        {
            var markdown = repository.GetContent(reference);
            var html = MarkdownRenderer.RenderMarkdown(markdown, jsRuntime);
            return html;
        }

        /// <inheritdoc/>
        public string GetMainPage()
        {
            logger.LogTrace("Fetching main page reference from repository.");
            return repository.GetMainPage();
        }

        /// <inheritdoc/>
        public List<TableOfContentsNode> GetTableOfContents()
        {
            logger.LogTrace("Fetching table of contents from repository.");
            return repository.GetTableOfContents();
        }
    }
}

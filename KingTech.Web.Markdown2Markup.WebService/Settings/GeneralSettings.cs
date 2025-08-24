using System.Reflection;

namespace KingTech.Web.Markdown2Markup.WebService.Settings
{
    /// <summary>
    /// General settings that apply to this application.
    /// </summary>
    public class GeneralSettings
    {
        /// <summary>
        /// Enable or disable the table of contents view on each page.
        /// </summary>
        public bool ShowTableOfContents { get; set; } = true;

        /// <summary>
        /// The title for your blog.
        /// This is shown at the top of the navigation menu.
        /// </summary>
        public string Title { get; set; } = "MyBlog";

        /// <summary>
        /// An optional banner that can be shown in the top of each page.
        /// Plain HTML is allowed here.
        /// </summary>
        public string Banner { get; set; } = string.Empty;

        /// <summary>
        /// An optional logo can be shown at the top of the navigation menu.
        /// Plain HTML is allowed.
        /// 
        /// If no logo is given, the <see cref="Title"/> is used.
        /// </summary>
        public string Logo { get; set; } = string.Empty;

        /// <summary>
        /// Insert a 'Home' item in the NavBar
        /// </summary>
        public bool InsertHomeItem { get; set; } = false;
    }
}

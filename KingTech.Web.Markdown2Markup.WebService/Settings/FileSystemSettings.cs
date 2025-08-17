namespace KingTech.Web.Markdown2Markup.WebService.Settings
{
    /// <summary>
    /// Settings for using the file system as a source for blogs.
    /// </summary>
    public class FileSystemSettings
    {
        /// <summary>
        /// The root directory the blogs are stored in.
        /// </summary>
        public string RootDirectory { get; set; } = string.Empty;
    }
}

using Humanizer;
using KingTech.Web.Markdown2Markup.Models;
using KingTech.Web.Markdown2Markup.WebService.Settings;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.RegularExpressions;

namespace KingTech.Web.Markdown2Markup.WebService.Repositories;

public class FileSystemContentRepository : IContentRepository
{
    private static readonly string[] allowedExtensions = { ".md", ".mdx" };
    private static readonly Regex chapterNameRegex = new Regex(@"^(?:(?<index>\d+)_)?(?<name>[^.]+)", RegexOptions.IgnoreCase);
    private static readonly Regex mainPageRegex = new Regex(@"^index(" + string.Join("|", allowedExtensions.Select(ext => Regex.Escape(ext))) + ")$", RegexOptions.IgnoreCase);
    private readonly FileSystemSettings settings;
    private readonly ILogger<FileSystemContentRepository> logger;

    public FileSystemContentRepository(ILogger<FileSystemContentRepository> logger, IOptions<FileSystemSettings> settings)
    {
        this.settings = settings.Value;
        this.logger = logger;

        this.logger.LogDebug("Starting file system content repository with settings: {@Settings}", settings);
    }

    /// <summary>
    /// Get the index of the main page based on 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string GetMainPage() => GetMainPage(settings.RootDirectory);

    /// <summary>
    /// Get chapters from the root directory (specified in settings) based on the names of sub-directories and files recursively.
    /// </summary>
    /// <returns>A list of <see cref="TableOfContentsNode"/>s describing the chapters and sub-chapters.</returns>
    /// <exception cref="DirectoryNotFoundException">This is thrown when the set root directory doesnt exist.</exception>
    public List<TableOfContentsNode> GetTableOfContents() => GetTableOfContents(settings.RootDirectory);

    /// <summary>
    /// Get the content from the given file.
    /// </summary>
    /// <param name="reference">The path/name of the file (without extension)</param>
    /// <returns>The content of the given file reference.</returns>
    public string GetContent(string reference)
    {
        // Find the file based on the reference, searching for any allowed extension
        foreach (var ext in allowedExtensions)
        {
            string filePath = Path.Combine(settings.RootDirectory, reference + ext);
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
        }

        // If not found, return empty string or throw exception as needed
        return string.Empty;
    }

    private string GetMainPage(string directory)
    {
        // Ensure the directory exists
        if (!Directory.Exists(directory))
        {
            logger.LogError("The directory '{directory}' does not exist.", directory);
            throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
        }

        var files = Directory.GetFiles(directory).Select(f => Path.GetFileName(f));

        // Find file that matches index regex.
        var mainPageFile = files.FirstOrDefault(f => mainPageRegex.IsMatch(f));
        if(mainPageFile != null)
            return ToReference(mainPageFile);

        // Or else get the first file in alphabetic order.
        return ToReference(files.Order().First()); //TODO: This might return 10_xxxxx.md instead of 2_xxxxx.md
    }

    /// <summary>
    /// Get chapters from a directory based on the names of sub-directories and files recursively.
    /// </summary>
    /// <param name="directory">The directory to get the chapters from.</param>
    /// <returns>A list of <see cref="TableOfContentsNode"/>s describing the chapters and sub-chapters.</returns>
    /// <exception cref="DirectoryNotFoundException">This is thrown when the given currentDirectory doesnt exist.</exception>
    private List<TableOfContentsNode> GetTableOfContents(string directory)
    {
        // Ensure the directory exists
        if (!Directory.Exists(directory))
        {
            logger.LogError("The directory '{directory}' does not exist.", directory);
            throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
        }

        // Initialize the root node of the chapter tree
        var root = new TableOfContentsNode
        {
            Name = Path.GetFileName(directory),
            Level = 0,
            SubChapters = new List<TableOfContentsNode>()
        };

        // Get all markdown files in the directory
        GetChaptersFromDirectoryRecursively(directory, root);
        return root.SubChapters;
    }

    /// <summary>
    /// Get chapters from a directory based on the names of sub-directories and files recursively.
    /// </summary>
    /// <param name="currentDirectory">The directory to get the chapters from.</param>
    /// <param name="currentNode">The <see cref="TableOfContentsNode"/> to add the chapters to.</param>
    /// <exception cref="DirectoryNotFoundException">This is thrown when the given currentDirectory doesnt exist.</exception>
    private void GetChaptersFromDirectoryRecursively(string currentDirectory, TableOfContentsNode currentNode)
    {
        if (!Directory.Exists(currentDirectory))
        {
            logger.LogError("Could not find directory '{directory}'.", currentDirectory);
            throw new DirectoryNotFoundException($"Could not find directory '{currentDirectory}'");
        }

        var childNodes = new List<TableOfContentsNode>();

        //Get all chapters from (sub)directories recursively.
        var directories = Directory.GetDirectories(currentDirectory).Select(d => Path.GetFileName(d));
        var index = 0;
        var level = currentNode.Level + 1;

        foreach (var d in directories)
        {
            //Check if name of directory can be used to define index.
            var match = chapterNameRegex.Match(d);
            if (match.Success)
            {
                //Isolate chapter name and index from directory name.
                var node = new TableOfContentsNode()
                {
                    Name = match.Groups["name"].Value.Humanize(),
                    HRef = null, //Directories cant have content (yet)
                    Index = int.Parse(match.Groups["index"].Value),
                    Level = level
                };
                GetChaptersFromDirectoryRecursively(Path.Combine(currentDirectory, d), node);
                childNodes.Add(node);
            }
            else
            {
                //Use full directory name and increment index.
                var node = new TableOfContentsNode()
                {
                    Name = d,
                    HRef = null, //Directories cant have content (yet)
                    Index = index,
                    Level = level
                };
                GetChaptersFromDirectoryRecursively(Path.Combine(currentDirectory, d), node);
                childNodes.Add(node);
            }

            index++;
        }

        //Get all chapters based on files (leave nodes).
        childNodes.AddRange(GetChaptersFromFileNames(currentDirectory, level));
        currentNode.SubChapters = childNodes.OrderBy(x => x.Index).ToList();
    }

    /// <summary>
    /// Get chapters from file names in a directory.
    /// </summary>
    /// <param name="directory">The directory to get the chapters from.</param>
    /// <param name="level">The level these chapters are in.</param>
    private List<TableOfContentsNode> GetChaptersFromFileNames(string directory, int level)
    {
        var childNodes = new List<TableOfContentsNode>();

        // Retrieve all files matching the chapter naming convention
        var files = Directory.GetFiles(directory).Select(f => Path.GetFileName(f));
        var index = 0;

        foreach (string file in files)
        {
            //Check if name of directory can be used to define index.
            var match = chapterNameRegex.Match(file);
            if (match.Success)
            {
                //Isolate chapter name and index from directory name.
                var node = new TableOfContentsNode()
                {
                    Name = match.Groups["name"].Value.Humanize(),
                    HRef = ToReference(Path.Combine(directory, file)),
                    Index = int.Parse(match.Groups["index"].Value),
                    Level = level
                };
                childNodes.Add(node);
            }
            else
            {
                //Use full file name and increment index.
                var node = new TableOfContentsNode()
                {
                    Name = Path.GetFileNameWithoutExtension(file).Humanize(),
                    HRef = ToReference(Path.Combine(directory, file)),
                    Index = index,
                    Level = level
                };
                childNodes.Add(node);
            }

            index++;
        }

        return childNodes;
    }

    /// <summary>
    /// Convert the file path to a reference that can be used in the browser, without the file extension.
    /// </summary>
    /// <param name="filePath">The path to the file to reference.</param>
    /// <returns>A referencable string without extension.</returns>
    private string ToReference(string filePath)
    {
        string? path = Path.GetDirectoryName(filePath);
        path = path.Replace(settings.RootDirectory, "");

        string fileName = Path.GetFileNameWithoutExtension(filePath);
        
        // Combine directory and filename (without extension) for reference
        string reference = path == null ? fileName : Path.Combine(path, fileName);
        return reference.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }
}
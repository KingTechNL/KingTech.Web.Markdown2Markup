using System.Text.RegularExpressions;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace KingTech.Web.Markdown2Markup.Components.ImageGallery;

public class ImageGalleryBlockParser : FencedBlockParserBase<ImageGalleryBlock>
{
    public const string DefaultInfoPrefix = "language-";
    public const string Opening = "[[[IG";
    public const string Closing = "]]]";


    public ImageGalleryBlockParser()
    {
        //Image gallery block starts with "[[[IG"
        OpeningCharacters = Opening.ToCharArray(); //new char[] {'[', '['};
        InfoPrefix = DefaultInfoPrefix;
    }

    /// <summary>
    /// Open a new fenced block for image galleries.
    /// </summary>
    /// <param name="processor"></param>
    /// <returns></returns>
    protected override ImageGalleryBlock CreateFencedBlock(BlockProcessor processor)
    {
        var imageGalleryBlock = new ImageGalleryBlock(this)
        {
            IndentCount = processor.Indent,
        };

        if (processor.TrackTrivia)
        {
            imageGalleryBlock.LinesBefore = processor.LinesBefore;
            imageGalleryBlock.TriviaBefore = processor.UseTrivia(processor.Start - 1);
            imageGalleryBlock.NewLine = processor.Line.NewLine;
        }
        
        return imageGalleryBlock;
    }

    /// <summary>
    /// Look for the end of the fenced block.
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="block"></param>
    /// <returns></returns>
    public override BlockState TryContinue(BlockProcessor processor, Block block)
    {
        var imageGallery = (ImageGalleryBlock)block;

        var result = base.TryContinue(processor, block);
        
        if (result == BlockState.Continue && !processor.TrackTrivia)
        {
            // Remove any indent spaces
            var c = processor.CurrentChar;
            var indentCount = imageGallery.IndentCount;
            while (indentCount > 0 && c.IsSpace())
            {
                indentCount--;
                c = processor.NextChar();
            }

            //End block.
            var lastParsedLine = imageGallery.GetLastLine();
            if (lastParsedLine?.Equals(Closing) ?? false)
            {
                result = BlockState.BreakDiscard;
            }
            else if (lastParsedLine?.EndsWith(Closing) ?? false)
            {
                result = BlockState.Break;
            }
        }

        return result;
    }

    /// <summary>
    /// Called when block is closed.
    /// Process all parsed lines.
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="block"></param>
    /// <returns></returns>
    public override bool Close(BlockProcessor processor, Block block)
    {
        //Process all lines.
        var imageGallery = (ImageGalleryBlock)block;
        var lines = imageGallery.Lines.Lines.Select(l => l.ToString().Trim());
        foreach (var line in lines)
        {
            //Set identifier
            if (line.StartsWith("#"))
            {
                imageGallery.Identifier = line.Replace("#", "");
            }
            //Custom attributes
            else if (line.StartsWith("$"))
            {
                ParseAttribute(line.Replace("$", ""), imageGallery);
            }
            //Get images
            else if(line.Contains("["))
            {
                var image = ParseImage(line);
                if(image != null)
                    imageGallery.Images.Add(image);
            }
            //Process imagegallery caption
            else
            {
                if (imageGallery.Caption == null)
                {
                    imageGallery.Caption = line;
                }
                else
                {
                    imageGallery.Caption += line;
                }

            }
        }

        //Set random identifier if none was specified.
        if (string.IsNullOrWhiteSpace(imageGallery.Identifier))
            imageGallery.Identifier = Random(7);

        return base.Close(processor, block);
    }

    /// <summary>
    /// Try to parse custom attributes from the given line.
    /// </summary>
    /// <param name="line"></param>
    /// <param name="imageGallery"></param>
    private void ParseAttribute(string line, ImageGalleryBlock imageGallery)
    {
        var split = line.Replace("$", "").Split('=');
        var attribute = split[0]?.ToLower();
        var value = split[1]?.ToLower();

        if (string.IsNullOrWhiteSpace(attribute) || string.IsNullOrWhiteSpace(value))
            return;

        switch (attribute)
        {
            case "loading":
                imageGallery.Loading = value;
                break;
        }
    }

    /// <summary>
    /// Parse an <see cref="ImageGalleryImage"/> from the given line.
    /// </summary>
    /// <param name="line">The line to parse.</param>
    /// <returns>The parsed <see cref="ImageGalleryImage"/></returns>
    private ImageGalleryImage? ParseImage(string line)
    {
        var imageCaption = Regex.Match(line, @"\(([^)]*)\)").Groups[1].Value;
        var imageUrl = Regex.Match(line, @"\[([^)]*)\]").Groups[1].Value;
        if (string.IsNullOrWhiteSpace(imageUrl))
            return null;
        return new ImageGalleryImage()
        {
            Caption = imageCaption,
            ImageUrl = imageUrl,
            ThumbnailUrl = imageUrl //TODO
        };
    }

    private static string Random(int amountOfCharacters)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, amountOfCharacters)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
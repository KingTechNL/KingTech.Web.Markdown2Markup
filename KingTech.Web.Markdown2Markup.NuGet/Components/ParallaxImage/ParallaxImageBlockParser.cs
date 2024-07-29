using KingTech.Web.Markdown2Markup.Components.ImageGallery;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace KingTech.Web.Markdown2Markup.Components.ParallaxImage;

public class ParallaxImageBlockParser : FencedBlockParserBase<ImageGalleryBlock>
{
    public const string DefaultInfoPrefix = "language-";
    public const string Opening = "[[[PX";
    public const string Closing = "]]]";


    public ParallaxImageBlockParser()
    {
        //Image gallery block starts with "[[["
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
        var parallaxImage = (ParallaxImageBlock)block;
        var lines = parallaxImage.Lines.Lines.Select(l => l.ToString().Trim());
        foreach (var line in lines)
        {
            //Set identifier
            if (line.StartsWith("#"))
            {
                parallaxImage.Identifier = line.Replace("#", "");
            }
        }

        return base.Close(processor, block);
    }
}
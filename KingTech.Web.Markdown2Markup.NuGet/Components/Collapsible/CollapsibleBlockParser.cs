using System.Text;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace KingTech.Web.Markdown2Markup.Components.Collapsible;

public class CollapsibleBlockParser : FencedBlockParserBase<CollapsibleBlock>
{
    public const string DefaultInfoPrefix = "language-";
    public const string Opening = "<collapsible>"; //DOESNT WORK YET (for some reason)
    public const string Closing = "</collapsible>";


    public CollapsibleBlockParser()
    {
        OpeningCharacters = Opening.ToCharArray(); //new char[] {'[', '['};
        InfoPrefix = DefaultInfoPrefix;
    }

    /// <summary>
    /// Open a new fenced block for image galleries.
    /// </summary>
    /// <param name="processor"></param>
    /// <returns></returns>
    protected override CollapsibleBlock CreateFencedBlock(BlockProcessor processor)
    {
        var imageGalleryBlock = new CollapsibleBlock(this)
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
        var collapsibleBlock = (CollapsibleBlock)block;
        if(processor.LineIndex == 0 && processor.Line.Text.Trim() != Opening)
            return BlockState.BreakDiscard;

        var result = base.TryContinue(processor, block);

        if (result == BlockState.Continue && !processor.TrackTrivia)
        {
            // Remove any indent spaces
            var c = processor.CurrentChar;
            var indentCount = collapsibleBlock.IndentCount;
            while (indentCount > 0 && c.IsSpace())
            {
                indentCount--;
                c = processor.NextChar();
            }

            //End block.
            var lastParsedLine = collapsibleBlock.GetLastLine();
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
        var collapsibleImage = (CollapsibleBlock)block;
        var lines = collapsibleImage.Lines.Lines.Select(l => l.ToString()).ToList();
        
        //Find title.
        var sb = new StringBuilder();
        foreach (var line in lines)
        {
            //Set title
            if (collapsibleImage.Title == null && line.Trim().StartsWith("#"))
            {
                collapsibleImage.Title = line.Replace("#", "").Trim();
                continue;
            }
            sb.Append(line);
        }

        //Set content
        collapsibleImage.Content = sb.ToString();

        return base.Close(processor, block);
    }
}
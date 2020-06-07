using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using ShineSpike.Utils;
using System.IO;
using System.Linq;

namespace ShineSpike.Extensions
{
    public static class MarkdownDocumentExtensions
    {
        public static string ToHtml(this MarkdownDocument document, MarkdownPipeline pipeline)
        {
            var writer = new StringWriter();
            // Create a HTML Renderer and setup it with the pipeline
            var renderer = new HtmlRenderer(writer);
            pipeline.Setup(renderer);
            // Renders markdown to HTML (to the writer)
            renderer.Render(document);
            // Gets the rendered string
            return writer.ToString();
        }

        public static string GetFirstImgUrl(this MarkdownDocument document)
        {
            // Select the first image
            var paragraphs = document.Descendants<ParagraphBlock>();
            var links = paragraphs.SelectMany(p => p.Inline.Descendants<LinkInline>());
            var firstImage = links.FirstOrDefault(l => l.IsImage);

            return firstImage?.Url ?? string.Empty;
        }

        public static string ExtractExcerpt(this MarkdownDocument document)
        {
            // Select the first image
            var paragraphs = document.Descendants<ParagraphBlock>();
            var firstTextParagraph = paragraphs.SelectMany(p => p.Inline.Descendants<LiteralInline>()).FirstOrDefault();
            var text = firstTextParagraph.Content.Text;

            if (firstTextParagraph == null)
                return string.Empty;

            // Truncate if needed
            if (text.Length > Constants.ExcerptLength)
            {
                return $"{text.Substring(text.Length - (Constants.ExcerptLength - 3))}...";
            }

            return text;
        }
    }
}

using CommonMark;
using System.Text.RegularExpressions;

namespace ShineSpike.Utils
{
    public class HtmlUtils
    {
        public static string MarkdownToHtml(string markdown)
        {
            if (string.IsNullOrEmpty(markdown))
            {
                return string.Empty;
            }

            var htmlContent = CommonMarkConverter.Convert(markdown);

            // Add lazy loading for post images
            return AddAttributeToTags(htmlContent, Constants.HtmlImgTag, Constants.LazyLoadingHtmlAttribute);
        }

        public static string AddAttributeToTags(string source, string tagName, string attribute)
        {
            var pattern = $"<{tagName} [^>]+>";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.Replace(source, m => ProcessMatch(m, attribute));
        }

        private static string ProcessMatch(Match m, string attribute)
        {
            string tag = m.Value;
            if (tag.IndexOf(attribute) == -1)
            {
                tag = tag.Replace(">", $" {attribute}>");
            }

            return tag;
        }
    }
}

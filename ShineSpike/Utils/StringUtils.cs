using ShineSpike.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ShineSpike.Utils
{
    public static class StringUtils
    {
        private static HashSet<char> DisallowedCharacters = new HashSet<char> { '!', '#', '$', '&', '\'', '(', ')', '*', ',', '/', ':', ';', '=', '?', '@', '[', ']', '\'', '%', '.', '<', '>', '\\', '^', '_', '\'', '{', '}', '|', '~', '`', '+' };

        public static string CreatePermalink(Post post)
        {
            var title = post.Title.ToLowerInvariant().Replace(Constants.Space, Constants.Dash, StringComparison.OrdinalIgnoreCase);
            return CleanString(title).ToLowerInvariant();
        }

        private static bool IsAllowedChar(char c)
        {
            return !DisallowedCharacters.Contains(c) && CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark;
        }

        private static string CleanString(string text)
        {
            var cleanedString = text.Normalize(NormalizationForm.FormD).Aggregate(
                new StringBuilder(),
                (sb, c) => IsAllowedChar(c) ? sb.Append(c) : sb,
                sb => sb.ToString()
            );
            return cleanedString.Normalize(NormalizationForm.FormC);
        }
    }
}

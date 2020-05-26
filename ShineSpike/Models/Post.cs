using ShineSpike.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShineSpike.Models
{
    public class Post
    {
        private string htmlContent = string.Empty;

        [Required]
        public string Id { get; set; } = DateTime.UtcNow.ToString("yyyyMMddHmmss");
        public string Permalink { get; set; } = string.Empty;

        public IList<string> Categories { get; } = new List<string>();

        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;

        public string Cover { get; set; } = string.Empty;

        [Required]
        public string Excerpt { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; } = true;
        public DateTime LastModified { get; internal set; } = DateTime.UtcNow;
        
        [NotMapped]
        public string Link => $"/blog/{Permalink}";

        [NotMapped]
        public bool HasCover => !string.IsNullOrEmpty(Cover);

        public string GetHtmlContent()
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                htmlContent = HtmlUtils.MarkdownToHtml(Content);
            }

            return htmlContent;
        }
    }
}

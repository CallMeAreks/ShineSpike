using Markdig;
using ShineSpike.Extensions;
using ShineSpike.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShineSpike.Models
{
    public class Post
    {
        [Required]
        public string Id { get; set; } = DateTime.UtcNow.ToString("yyyyMMddHmmss");
        public string Permalink { get; set; } = string.Empty;

        public IList<string> Categories { get; } = new List<string>();

        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;

        public string Cover { get; set; }

        public string Excerpt { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; } = true;
        public DateTime LastModified { get; internal set; } = DateTime.UtcNow;
        
        [NotMapped]
        public string Link => $"/{Constants.BlogControllerAction}/{Permalink}";

        [NotMapped]
        public bool HasCover => !string.IsNullOrEmpty(Cover);

        [NotMapped]
        public string HtmlContent { get; set; }

        public void ParseContent()
        {
            var pipeline = new MarkdownPipelineBuilder().Use<LazyImagesExtension>().Build();
            var doc = Markdown.Parse(Content, pipeline);

            Cover = doc.GetFirstImgUrl();
            Excerpt = Excerpt ?? doc.ExtractExcerpt();
            HtmlContent = doc.ToHtml(pipeline);
        }
    }
}

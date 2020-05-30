using Markdig;
using ShineSpike.Extensions;
using ShineSpike.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShineSpike.Models
{
    public class Post
    {
        [Required]
        public long Id { get; set; } = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHmmss"));
        public string Permalink { get; set; } = string.Empty;
        public IList<string> Categories { get; set; } = new List<string>();

        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;

        public string Cover { get; set; }

        public string Excerpt { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; } = true;
        public DateTime LastModified { get; internal set; } = DateTime.UtcNow;
        public string HtmlContent { get; set; }

        [JsonIgnore]
        public string Link => $"/{Constants.BlogControllerAction}/{Permalink}";

        [JsonIgnore]
        public bool HasCover => !string.IsNullOrEmpty(Cover);

        [JsonIgnore]
        public string SerializedCategories => string.Join(",", Categories);

        public void LoadCategoriesFromString(string serializedCategories)
        {
            Categories.Clear();

            var categories = serializedCategories.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var category in categories)
            {
                Categories.Add(category.ToLower().Trim());
            }
        }

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

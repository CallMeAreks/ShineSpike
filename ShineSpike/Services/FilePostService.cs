using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ShineSpike.Models;
using ShineSpike.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShineSpike.Services
{
    public class FilePostService : IPostService
    {
        private const string Extension = ".json";
        private string PostsFolder;
        private string ImagesFolder;
        private readonly IHttpContextAccessor ContextAccessor;
        private readonly List<Post> Cache = new List<Post>();

        public bool IsUserLoggedIn => ContextAccessor.HttpContext?.User?.Identity.IsAuthenticated == true;

        public FilePostService(IWebHostEnvironment env, IHttpContextAccessor contextAccessor)
        {
            if (env is null)
            {
                throw new ArgumentNullException(nameof(env));
            }

            PostsFolder = Path.Combine(env.WebRootPath, Constants.PostsFolderPath);
            ImagesFolder = Path.Combine(env.WebRootPath, Constants.ImagesFolderPath);

            ContextAccessor = contextAccessor;
            
            Initialize();
        }

        public Task<Post> GetById(string id)
        {
            return FetchPost(post => post.Id == id);
        }

        public Task<Post> GetByPermalink(string permalink)
        {
            return FetchPost(post => post.Permalink == permalink);
        }

        public IEnumerable<Post> GetPublishedPosts(Func<Post, bool> condition = null)
        {
            if(condition == null)
            {
                return Cache.Where(IsPublished);
            }

            return Cache.Where(post => IsPublished(post) && condition(post));
        }

        public IEnumerable<Post> GetPostsByCategory(string category)
        {
            return GetPublishedPosts(post => post.Categories.Contains(category, StringComparer.OrdinalIgnoreCase));
        }

        public async Task Publish(Post post)
        {
            if (post is null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            
            post.LastModified = DateTime.UtcNow;
            post.Permalink = string.IsNullOrEmpty(post.Permalink) ? StringUtils.CreatePermalink(post) : post.Permalink;
            post.ParseContent();

            var filePath = GetPostFilePath(post);
            var json = JsonSerializer.Serialize(post);

            await File.WriteAllTextAsync(filePath, json);

            if (!Cache.Contains(post))
            {
                Cache.Add(post);
                SortPostsByPublishedDate();
            }
        }

        public Task Delete(Post post)
        {
            if (post is null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            var filePath = GetPostFilePath(post);

            File.Delete(filePath);
            Cache.Remove(post);

            return Task.CompletedTask;
        }

        protected Task<Post> FetchPost(Func<Post, bool> predicate)
        {
            var post = Cache.FirstOrDefault(predicate);

            var isPublished = IsPublished(post);
            return Task.FromResult(isPublished ? post : null);
        }

        protected void Initialize()
        {
            // Ensure posts and images folders are created
            Directory.CreateDirectory(PostsFolder);
            Directory.CreateDirectory(ImagesFolder);

            LoadCache();
            SortPostsByPublishedDate();
        }

        protected void LoadCache()
        {
            foreach (var file in Directory.EnumerateFiles(PostsFolder, $"*{Extension}", SearchOption.TopDirectoryOnly))
            {
                var content = File.ReadAllText(file);
                var post = JsonSerializer.Deserialize<Post>(content);
                Cache.Add(post);
            }
        }

        protected bool IsPublished(Post post)
        {
            return post != null && post.PublishedAt < DateTime.UtcNow && post.IsPublished;
        }

        protected void SortPostsByPublishedDate()
        {
            Cache.Sort(
                (post1, post2) => post2.PublishedAt.CompareTo(post1.PublishedAt)
            );
        }

        protected string GetPostFilePath(Post post) => Path.Combine(PostsFolder, $"{post.Id}{Extension}");
    }
}

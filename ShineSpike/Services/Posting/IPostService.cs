using ShineSpike.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShineSpike.Services
{
    public interface IPostService
    {
        Task<Post> GetById(long id);
        Task<Post> GetByPermalink(string permalink);
        IEnumerable<Post> GetPublishedPosts(Func<Post, bool> condition = null);
        IEnumerable<Post> GetPublishedPages();
        IEnumerable<Post> GetPostsByCategory(string category);
        IEnumerable<string> GetCategories();
        Task Publish(Post post);
        Task Delete(Post post);
    }
}

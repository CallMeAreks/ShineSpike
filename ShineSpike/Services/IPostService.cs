using ShineSpike.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShineSpike.Services
{
    public interface IPostService
    {
        Task<Post> GetById(string id);
        Task<Post> GetByPermalink(string permalink);
        IEnumerable<Post> GetPublishedPosts(Func<Post, bool> condition = null);
        IEnumerable<Post> GetPostsByCategory(string category);
        Task Publish(Post post);
        Task Delete(Post post);
    }
}

using Microsoft.AspNetCore.Mvc;
using ShineSpike.Extensions;
using ShineSpike.Models;
using ShineSpike.Services;
using ShineSpike.Utils;
using ShineSpike.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShineSpike.Controllers
{
    [Route("blog")]
    public sealed class BlogController : Controller
    {
        private readonly IPostService Service;

        public BlogController(IPostService service)
        {
            Service = service;
        }

        [Route("{page:int?}")]
        [ResponseCache(CacheProfileName = Constants.CacheProfile)]
        public IActionResult Index([FromRoute]int page = 0)
        {
            var posts = Service.GetPublishedPosts().Page(Constants.PostsPerPage, page).ToList();
            return OutputPosts(posts, page, nameof(Index));
        }


        [Route("category/{category}/{page:int?}")]
        [ResponseCache(CacheProfileName = Constants.CacheProfile)]
        public IActionResult Category(string category, int page = 0)
        {
            if (category == null)
                return NotFound();

            var posts = Service.GetPostsByCategory(category).Page(Constants.PostsPerPage, page).ToList();
            return OutputPosts(posts, page, nameof(Category));
        }

        [Route("{permalink?}")]
        [ResponseCache(CacheProfileName = Constants.CacheProfile)]
        public async Task<IActionResult> ViewPost(string permalink)
        {
            var post = await Service.GetByPermalink(permalink);
            return post == null ? (IActionResult)NotFound() : View(post);
        }

        private IActionResult OutputPosts(IEnumerable<Post> posts, int page, string actionName)
        {
            // If it isn't the index page and there are no posts to show, return 404
            if (!posts.Any() && page > 0)
                return NotFound();

            var model = GetPageModel(page, actionName);
            model.Posts = posts;

            return View("Index", model);
        }

        private PostPageViewModel GetPageModel(int page, string actionName)
        {
            var previousPageUrl = Url.Action(actionName, new { page = page + 1 });
            var nextPageUrl = Url.Action(actionName, new { page = page > 1 ? (int?)page - 1 : null });

            return new PostPageViewModel
            {
                Page = new PageViewModel
                {
                    HasNextPage = page > 0,
                    PreviousPageUrl = previousPageUrl,
                    NextPageUrl = nextPageUrl
                }
            };
        }
    }
}

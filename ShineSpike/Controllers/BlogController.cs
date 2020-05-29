using Microsoft.AspNetCore.Mvc;
using ShineSpike.Extensions;
using ShineSpike.Services;
using ShineSpike.Utils;
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
;
            if (posts.Any() || page == 0)
            {
                return View(posts);
            }

            // Return 404 since the page is not the main and it's empty 
            return NotFound();
        }

        [Route("{permalink?}")]
        [ResponseCache(CacheProfileName = Constants.CacheProfile)]
        public async Task<IActionResult> ViewPost(string permalink)
        {
            var post = await Service.GetByPermalink(permalink);
            return post == null ? (IActionResult)NotFound() : View(post);
        }
    }
}

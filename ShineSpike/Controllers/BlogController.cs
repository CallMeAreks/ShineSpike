using Microsoft.AspNetCore.Mvc;
using ShineSpike.Services;
using ShineSpike.Utils;
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

        [ResponseCache(CacheProfileName = Constants.CacheProfile)]
        public IActionResult Index()
        {
            var posts = Service.GetPublishedPosts();
            return View(posts);
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

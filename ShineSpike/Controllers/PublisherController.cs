using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShineSpike.Models;
using ShineSpike.Services;
using System;
using System.Threading.Tasks;

namespace ShineSpike.Controllers
{
    public sealed class PublisherController : Controller
    {
        private readonly IPostService Service;

        public PublisherController(IPostService service)
        {
            Service = service;
        }

        [Route("blog/editor/{id?}")]
        [Authorize]
        public async Task<IActionResult> Editor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new Post());
            }

            var post = await Service.GetById(id);
            return post == null ? (IActionResult)NotFound() : View(post);
        }

        [Route("blog/editor")]
        [HttpPost, Authorize, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PublishPost(Post post)
        {
            if (post is null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            
            await Service.Publish(post);
            return RedirectToAction("ViewPost", "Blog", new { post.Permalink });
        }

        [Route("blog/{id}/delete")]
        [HttpPost, Authorize, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(string id)
        {
            var post = await Service.GetById(id);

            if (post is null)
            {
                return NotFound();
            }

            await Service.Delete(post);
            return Redirect("/");
        }
    }
}

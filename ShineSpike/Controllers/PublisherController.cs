using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShineSpike.Models;
using ShineSpike.Services;
using ShineSpike.ViewModels;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Editor(long? id)
        {
            var allCategories = Service.GetCategories();

            if (id == null)
                return View(GetEditPostViewModel(allCategories));

            var post = await Service.GetById(id.Value);

            if(post == null)
                return NotFound();
            
            return View(GetEditPostViewModel(allCategories, post));
        }

        [Route("blog/editor")]
        [HttpPost, Authorize, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PublishPost(EditPostViewModel model)
        {
            if (model?.Post == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            model.Post.LoadCategoriesFromString(Request.Form["Categories"]);

            await Service.Publish(model.Post);
            return RedirectToAction("ViewPost", "Blog", new { model.Post.Permalink });
        }

        [Route("blog/{id}/delete")]
        [HttpPost, Authorize, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(long id)
        {
            var post = await Service.GetById(id);

            if (post is null)
            {
                return NotFound();
            }

            await Service.Delete(post);
            return Redirect("/");
        }

        private EditPostViewModel GetEditPostViewModel(IEnumerable<string> categories, Post post = null)
        {
            var model =new EditPostViewModel
            {
                AllCategories = categories
            };

            if(post != null)
            {
                model.Post = post;
            }

            return model;
        }
    }
}

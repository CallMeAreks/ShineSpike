using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShineSpike.Extensions;
using ShineSpike.Services;
using System.Linq;

namespace ShineSpike.Controllers
{
    public class UploadController : Controller
    {
        private readonly IFormFileUploadService Service;

        public UploadController(IFormFileUploadService service)
        {
            Service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public JsonResult Image()
        {
            var image = Request.Form.Files.FirstOrDefault(file => file.IsValidImage());
            var response = Service.UploadFormFile(image);
            return Json(response);
        }
    }
}

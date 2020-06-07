using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShineSpike.Extensions;
using ShineSpike.Services;
using ShineSpike.Services.Backup;
using ShineSpike.Utils;
using System.IO;
using System.Linq;

namespace ShineSpike.Controllers
{
    [Route("panel")]
    [Authorize]
    public class PanelController : Controller
    {
        private readonly IPostService PostService;
        private readonly IBackupService BackupService;
        private readonly IFormFileUploadService UploadService;

        public PanelController(IPostService postService, IBackupService backupService, IFormFileUploadService uploadService)
        {
            PostService = postService;
            BackupService = backupService;
            UploadService = uploadService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(ViewPostList));
        }

        [Route("posts")]
        public IActionResult ViewPostList()
        {
            var posts = PostService.GetPublishedPosts().ToList();
            return View(nameof(ViewPostList), posts);
        }

        [Route("pages")]
        public IActionResult ViewPageList()
        {
            var pages = PostService.GetPublishedPages().ToList();
            return View(nameof(ViewPostList), pages);
        }

        [Route("backups")]
        public IActionResult ViewBackups()
        {
            var backups = BackupService.GetBackups().ToList();
            return View(backups);
        }

        [Route("backups/new")]
        public IActionResult CreateBackup()
        {
            BackupService.BackupData();
            return RedirectToAction(nameof(ViewBackups));
        }

        [Route("backups/download/{backupFile}")]
        public IActionResult DownloadBackup(string backupFile)
        {
            try
            {
                var file = BackupService.GetBackup(backupFile);
                return PhysicalFile(file.FullName, Constants.BackupFileMimeType, file.Name);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        [Route("backups/upload")]
        public IActionResult UploadBackupForm()
        {
            return View();
        }

        [Route("backups/upload")]
        [HttpPost]
        public IActionResult UploadBackup()
        {
            var backup = Request.Form.Files.FirstOrDefault(file => file.IsValidZipFile());
            UploadService.UploadFormFile(backup, Constants.BackupFolderPath);
            return RedirectToAction(nameof(ViewBackups));
        }

        [Route("backups/{backupFile}/restore")]
        public IActionResult Restore(string backupFile)
        {
            var result = BackupService.RestoreData(backupFile);
            return RedirectToAction("Index", "Blog");
        }
    }
}

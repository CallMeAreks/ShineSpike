using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShineSpike.Services.Backup;
using ShineSpike.Utils;
using System.IO;
using System.Linq;

namespace ShineSpike.Controllers
{
    [Route("settings")]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IBackupService Service;

        public SettingsController(IBackupService service)
        {
            Service = service;
        }

        public IActionResult Index()
        {
            var backups = Service.GetBackups().ToList();
            return View(backups);
        }

        [Route("/backups/new")]
        public IActionResult CreateBackup()
        {
            Service.BackupData();
            return RedirectToAction("Index");
        }

        [Route("/backups/download/{backupFile}")]
        public IActionResult DownloadBackup(string backupFile)
        {
            try
            {
                var file = Service.GetBackup(backupFile);
                return PhysicalFile(file.FullName, Constants.BackupFileMimeType, file.Name);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        [Route("/backups/{backupFile}/restore")]
        public IActionResult Restore(string backupFile)
        {
            var result = Service.RestoreData(backupFile);
            return RedirectToAction("Index", "Blog");
        }
    }
}

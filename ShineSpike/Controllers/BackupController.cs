using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShineSpike.Services.Backup;
using ShineSpike.Utils;

namespace ShineSpike.Controllers
{
    [Authorize]
    public class BackupController : Controller
    {
        private readonly IBackupService Service;

        public BackupController(IBackupService service)
        {
            Service = service;
        }

        public IActionResult Index()
        {
            var file = Service.BackupData();
            return PhysicalFile(file.FullName, Constants.BackupFileMimeType, file.Name);
        }

        [Route("/backup/{backupFile}/restore")]
        public IActionResult Restore(string backupFile)
        {
            var result = Service.RestoreData(backupFile);
            return Ok(result);
        }
    }
}

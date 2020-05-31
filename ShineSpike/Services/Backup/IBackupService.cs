using System.Collections.Generic;
using System.IO;

namespace ShineSpike.Services.Backup
{
    public interface IBackupService
    {
        FileInfo BackupData();
        IEnumerable<FileInfo> GetBackups();
        FileInfo GetBackup(string backupFileName);
        bool RestoreData(string backupFileName);
    }
}

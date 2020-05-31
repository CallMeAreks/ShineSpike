using System.IO;

namespace ShineSpike.Services.Backup
{
    public interface IBackupService
    {
        FileInfo BackupData();
        bool RestoreData(string backupFileName);
    }
}

using Microsoft.AspNetCore.Hosting;
using ShineSpike.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ShineSpike.Services.Backup
{
    public class ZipFileBackupService : IBackupService
    {
        protected string BlogFilesPath { get; set; }
        protected string BackupFolderPath { get; set; }

        public ZipFileBackupService(IWebHostEnvironment env)
        {
            if (env is null)
            {
                throw new ArgumentNullException(nameof(env));
            }

            BlogFilesPath    = Path.Combine(env.WebRootPath, Constants.BlogFilesFolderPath);
            BackupFolderPath = Path.Combine(env.WebRootPath, Constants.BackupFolderPath);

            // Ensure backup post is created
            Directory.CreateDirectory(BackupFolderPath);
        }

        public FileInfo BackupData()
        {
            var backupName = DateTime.UtcNow.ToString("yyyyMMddHmmss");
            var destination = $"{BackupFolderPath}/{$"{backupName}.zip"}";

            ZipFile.CreateFromDirectory(BlogFilesPath, destination);
            return new FileInfo(destination);
        }

        public bool RestoreData(string backupFileName)
        {
            try
            {
                var backupPath = $"{BackupFolderPath}/{backupFileName}.zip";
                ZipFile.ExtractToDirectory(backupPath, BlogFilesPath, true);
                return true;
            }
            catch(FileNotFoundException)
            {
                return false;
            }
        }

        public FileInfo GetBackup(string backupFile)
        {
            var destination = $"{BackupFolderPath}/{$"{backupFile}.zip"}";
            return new FileInfo(destination);
        }

        public IEnumerable<FileInfo> GetBackups()
        {
            var files = Directory.EnumerateFiles(BackupFolderPath, $"*zip", SearchOption.TopDirectoryOnly);
            return files.Select(file => new FileInfo(file)).OrderByDescending(file => file.Name);
        }
    }
}

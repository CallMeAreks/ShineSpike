using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ShineSpike.Models;
using System;
using System.IO;
using System.Net.Http.Headers;

namespace ShineSpike.Services
{
    public class FormFileUploadService : IFormFileUploadService
    {
        private string WebRootPath { get; set; }

        public FormFileUploadService(IWebHostEnvironment env)
        {
            if (env is null)
            {
                throw new ArgumentNullException(nameof(env));
            }

            WebRootPath = env.WebRootPath;
        }

        public FormUploadResponse UploadFormFile(IFormFile file, string destinationFolder)
        {
            try
            {
                //Getting FileName
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                if(File.Exists(Path.Combine(WebRootPath, destinationFolder, fileName)))
                {
                    // Assigning Unique Filename
                    fileName = $"{DateTime.UtcNow:yyyyMMddHmmss}-{fileName}";
                }

                // Get the final path
                var filePath = Path.Combine(WebRootPath, destinationFolder, fileName);

                using (FileStream fs = File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                return new FormUploadResponse($"/{destinationFolder}/{fileName}");
            }
            catch
            {
                return new FormUploadResponse(false, null);
            }
        }
    }
}

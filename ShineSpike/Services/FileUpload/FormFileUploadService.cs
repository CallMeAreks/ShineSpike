using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ShineSpike.Models;
using ShineSpike.Utils;
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

        public FormUploadResponse UploadFormFile(IFormFile file)
        {
            try
            {
                //Getting FileName
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                // Assigning Unique Filename
                var uniqueFileName = $"{DateTime.UtcNow:yyyyMMddHmmss}-{fileName}";

                // Get the final path
                var filePath = Path.Combine(WebRootPath, Constants.ImagesFolderPath, uniqueFileName);

                using (FileStream fs = File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                return new FormUploadResponse($"/{Constants.ImagesFolderPath}/{uniqueFileName}");
            }
            catch
            {
                return new FormUploadResponse(false, null);
            }
        }
    }
}

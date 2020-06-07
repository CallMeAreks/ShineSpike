using Microsoft.AspNetCore.Http;
using ShineSpike.Models;

namespace ShineSpike.Services
{
    public interface IFormFileUploadService
    {
        public FormUploadResponse UploadFormFile(IFormFile file, string destinationFolder);
    }
}

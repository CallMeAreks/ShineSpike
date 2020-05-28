﻿using Microsoft.AspNetCore.Http;

namespace ShineSpike.Extensions
{
    public static class IFormFileExtensions
    {
        public static bool IsValidImage(this IFormFile file)
        {
            return file.Length > 0 && file.ContentType.Contains("image");
        }
    }
}

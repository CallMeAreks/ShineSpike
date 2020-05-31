using System.IO;

namespace ShineSpike.Extensions
{
    public static class FileInfoExtensions
    {
        public static string GetNameWithoutExtension(this FileInfo file)
        {
            return Path.GetFileNameWithoutExtension(file.Name);
        }
    }
}

using ShineSpike.Services;
using System.Threading.Tasks;

namespace ShineSpike.Extensions
{
    public static class PostServiceExtensions
    {
        public static async Task<string> GetPostAsHtmlByPermalink(this IPostService service, string permalink)
        {
            var post = await service.GetByPermalink(permalink);
            return post?.HtmlContent ?? string.Empty;
        }
    }
}

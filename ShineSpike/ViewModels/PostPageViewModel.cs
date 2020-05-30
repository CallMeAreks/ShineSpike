using ShineSpike.Models;
using System.Collections.Generic;

namespace ShineSpike.ViewModels
{
    public class PostPageViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public PageViewModel Page { get; set; }
    }
}

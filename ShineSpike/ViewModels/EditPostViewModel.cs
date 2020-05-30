using ShineSpike.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShineSpike.ViewModels
{
    public class EditPostViewModel
    {
        public Post Post { get; set; } = new Post();
        public IEnumerable<string> AllCategories { get; set; } = new List<string>();
        public string AllCategoriesSerialized => string.Join(",", AllCategories.Select(cat => $"'{cat}'"));
    }
}

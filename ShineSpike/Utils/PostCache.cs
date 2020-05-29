using ShineSpike.Models;
using System.Collections.Generic;

namespace ShineSpike.Utils
{
    public class PostCache
    {
        private DualKeyDictionary<long, string, Post> Dict = new DualKeyDictionary<long, string, Post>();
        private List<long> Index = new List<long>();

        public IEnumerable<Post> Posts
        {
            get
            {
                foreach (var id in Index)
                {
                    yield return Get(id);
                }
            }
        }

        public void Add(Post post)
        {
            AddOrReplace(post);
            SortIndex();
        }

        public void AddRange(IEnumerable<Post> posts)
        {
            foreach(var post in posts)
            {
                AddOrReplace(post);
            }

            SortIndex();
        }

        public Post Get(long key)
        {
            return Dict.TryGetValue(key, out Post post) ? post : null;
        }

        public Post Get(string key)
        {
            return Dict.TryGetValue(key, out Post post) ? post : null;
        }

        public void Remove(Post post)
        {
            RemoveFromCaches(post);
            SortIndex();
        }


        private void AddOrReplace(Post post)
        {
            if (Dict.ContainsKey(post.Id))
            {
                RemoveFromCaches(post);
            }

            Dict.Add(post.Id, post.Permalink, post);
            Index.Add(post.Id);
        }
        private void RemoveFromCaches(Post post)
        {
            Dict.Remove(post.Id);
            Index.Remove(post.Id);
        }

        private void SortIndex()
        {
            Index.Sort((a, b) => b.CompareTo(a));
        }
    }
}

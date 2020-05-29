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
                // Retrieving them using the Index to preserve order
                foreach (var id in Index)
                {
                    yield return Get(id);
                }
            }
        }

        /// <summary>
        /// Adds the item to the cache and sorts the collection in descending order
        /// </summary>
        /// <param name="post"></param>
        public void Add(Post post)
        {
            AddOrReplace(post);
            SortIndex();
        }

        /// <summary>
        /// Adds the items to the cache and sorts the collection
        /// </summary>
        /// <param name="posts"></param>
        public void AddRange(IEnumerable<Post> posts)
        {
            foreach(var post in posts)
            {
                AddOrReplace(post);
            }

            SortIndex();
        }

        /// <summary>
        /// Gets the item by its primary key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Post Get(long key)
        {
            return Dict.TryGetValue(key, out Post post) ? post : null;
        }

        /// <summary>
        /// Gets the item by its secondary key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Post Get(string key)
        {
            return Dict.TryGetValue(key, out Post post) ? post : null;
        }

        /// <summary>
        /// Removes the item from the cache and sorts the collection
        /// </summary>
        /// <param name="post"></param>
        public void Remove(Post post)
        {
            RemoveFromCaches(post);
            SortIndex();
        }

        /// <summary>
        /// Adds or replaces the item from the cache and the index
        /// </summary>
        /// <param name="post"></param>
        private void AddOrReplace(Post post)
        {
            if (Dict.ContainsKey(post.Id))
            {
                RemoveFromCaches(post);
            }

            Dict.Add(post.Id, post.Permalink, post);
            Index.Add(post.Id);
        }

        /// <summary>
        /// Removes the item from the cache and the index
        /// </summary>
        /// <param name="post"></param>
        private void RemoveFromCaches(Post post)
        {
            Dict.Remove(post.Id);
            Index.Remove(post.Id);
        }

        /// <summary>
        /// Sorts the index in descending order
        /// </summary>
        private void SortIndex()
        {
            Index.Sort((a, b) => b.CompareTo(a));
        }
    }
}

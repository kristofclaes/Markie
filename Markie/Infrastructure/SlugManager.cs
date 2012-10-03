using System;
using System.Collections.Generic;
using System.Linq;
using Simple.Data;

namespace Markie.Infrastructure
{
    public interface ISlugManager
    {
        string GetUniqueSlug(string postSlug);
    }

    public class SlugManager : ISlugManager
    {
        public string GetUniqueSlug(string postSlug)
        {
            var db = Database.Open();
            var numberOfPostsWithSlug = db.Posts.FindAllByUrlSlug(postSlug).Count();

            if (numberOfPostsWithSlug == 0)
            {
                return postSlug;
            }

            List<string> slugs = GetUrlSlugStartingWith(db, postSlug);

            int counter = 2;
            string slugCandidate = String.Format("{0}-{1}", postSlug, counter);

            while (slugs.Any(x => x.Equals(slugCandidate, StringComparison.OrdinalIgnoreCase)))
            {
                counter = counter + 1;
                slugCandidate = String.Format("{0}-{1}", postSlug, counter);
            }

            return String.Format("{0}-{1}", postSlug, counter);
        }

        private List<string> GetUrlSlugStartingWith(dynamic db, string slug)
        {
            var postsStartingWithSlug = db.Posts.FindAll(db.Posts.UrlSlug.Like(slug + "%")).Select(db.Posts.UrlSlug).ToList();

            var slugs = new List<string>();
            foreach (var slugObject in postsStartingWithSlug)
            {
                slugs.Add(slugObject.UrlSlug);
            }

            return slugs;
        }
    }
}
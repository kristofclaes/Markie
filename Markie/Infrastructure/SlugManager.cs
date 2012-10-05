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
            var numberOfPostsWithSlug = db.Posts.GetCount(db.Posts.UrlSlug == postSlug);

            if (numberOfPostsWithSlug == 0)
            {
                return postSlug;
            }

            List<string> slugs = db.Posts.FindAll(db.Posts.UrlSlug.Like(postSlug + "%")).Select(db.Posts.UrlSlug).ToScalarList<string>();

            int counter = 2;
            string slugCandidate = String.Format("{0}-{1}", postSlug, counter);

            while (slugs.Any(x => x.Equals(slugCandidate, StringComparison.OrdinalIgnoreCase)))
            {
                counter = counter + 1;
                slugCandidate = String.Format("{0}-{1}", postSlug, counter);
            }

            return String.Format("{0}-{1}", postSlug, counter);
        }
    }
}
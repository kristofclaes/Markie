using System;
using Simple.Data;

namespace Markie.Infrastructure
{
    public interface IPostStore
    {
        AddDraftResult AddDraft(string title);
    }

    public class PostStore : IPostStore
    {
        private readonly ISlugifier slugifier;
        private readonly ISlugManager slugManager;

        public PostStore(ISlugifier slugifier, ISlugManager slugManager)
        {
            this.slugifier = slugifier;
            this.slugManager = slugManager;
        }

        public AddDraftResult AddDraft(string title)
        {
            try
            {
                string postTitle = title.Trim();

                if (String.IsNullOrWhiteSpace(postTitle))
                {
                    return new AddDraftResult { Success = false };
                }

                string postSlug = slugifier.Slugify(postTitle);
                string uniqueSlug = slugManager.GetUniqueSlug(postSlug);
                DateTime currentTime = DateTime.UtcNow;

                var db = Database.Open();
                var post = db.Posts.Insert(Title: postTitle, UrlSlug: uniqueSlug, IsPublished: false, CreatedOn: currentTime, LastUpdatedOn: currentTime);

                var addResult = new AddDraftResult { Success = true, Id = post.PostId, Title = post.Title, Url = "/admin/posts/edit/" + post.PostId };

                return addResult;
            }
            catch (Exception error)
            {
                return new AddDraftResult { Success = false };
            }
        }
    }
}
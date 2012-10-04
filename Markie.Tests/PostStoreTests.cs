using FakeItEasy;
using Markie.Infrastructure;
using NUnit.Framework;
using Simple.Data;

namespace Markie.Tests
{
    [TestFixture]
    public class PostStoreTests
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase(null)]
        public void AddDraft_returns_false_when_title_is_empty_whitespace_or_null(string title)
        {
            var slugifier = A.Fake<ISlugifier>();
            var slugManager = A.Fake<ISlugManager>();

            var postStore = new PostStore(slugifier, slugManager);

            var result = postStore.AddDraft(title);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void AddDraft_returns_correct_result_when_title_is_valid()
        {
            var adapter = new InMemoryAdapter();
            adapter.SetAutoIncrementColumn("Posts", "PostId");
            Database.UseMockAdapter(adapter);

            var slugifier = A.Fake<ISlugifier>();
            var slugManager = A.Fake<ISlugManager>();

            A.CallTo(() => slugifier.Slugify("the title")).Returns("the-title");
            A.CallTo(() => slugManager.GetUniqueSlug("the-title")).Returns("the-title");

            var postStore = new PostStore(slugifier, slugManager);

            var result = postStore.AddDraft("the title");

            Assert.IsTrue(result.Success);
            Assert.AreEqual("the title", result.Title);
            Assert.AreEqual("/admin/posts/edit/1", result.Url);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void AddDraft_adds_draft_to_the_database_when_title_is_valid()
        {
            var adapter = new InMemoryAdapter();
            adapter.SetKeyColumn("Posts", "PostId");
            adapter.SetAutoIncrementColumn("Posts", "PostId");
            Database.UseMockAdapter(adapter);

            var slugifier = A.Fake<ISlugifier>();
            var slugManager = A.Fake<ISlugManager>();

            A.CallTo(() => slugifier.Slugify("the title")).Returns("the-title");
            A.CallTo(() => slugManager.GetUniqueSlug("the-title")).Returns("the-title");

            var postStore = new PostStore(slugifier, slugManager);

            postStore.AddDraft("the title");

            var db = Database.Open();
            var post = db.Posts.Get(1);

            Assert.AreEqual(1, post.PostId);
            Assert.AreEqual("the title", post.Title);
            Assert.AreEqual("the-title", post.UrlSlug);
            Assert.IsFalse(post.IsPublished);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase(null)]
        public void AddDraft_adds_nothing_to_the_database_when_title_is_empty_whitespace_or_null(string title)
        {
            var adapter = new InMemoryAdapter();
            adapter.SetKeyColumn("Posts", "PostId");
            adapter.SetAutoIncrementColumn("Posts", "PostId");
            Database.UseMockAdapter(adapter);

            var slugifier = A.Fake<ISlugifier>();
            var slugManager = A.Fake<ISlugManager>();

            var postStore = new PostStore(slugifier, slugManager);

            postStore.AddDraft(title);

            var db = Database.Open();
            var postCount = db.Posts.All().Count();

            Assert.AreEqual(0, postCount);
        }
    }
}
using System;
using Markie.Infrastructure;
using NUnit.Framework;
using Simple.Data;

namespace Markie.Tests
{
    [TestFixture]
    public class SlugManagerTests
    {
        [Test]
        public void Returns_same_string_when_database_is_empty()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var slugManager = new SlugManager();

            var input = "a-new-slug";
            var uniqueSlug = slugManager.GetUniqueSlug(input);

            Assert.AreEqual(input, uniqueSlug);
        }

        [Test]
        public void Returns_same_string_when_slug_is_not_found()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var db = Database.Open();
            db.Posts.Insert(Title: "A title", UrlSlug: "a-slug", IsPublised: false, CreatedOn: DateTime.UtcNow, LastUpdatedOn: DateTime.UtcNow);

            var slugManager = new SlugManager();

            var input = "a-new-slug";
            var uniqueSlug = slugManager.GetUniqueSlug(input);

            Assert.AreEqual(input, uniqueSlug);
        }

        [Test]
        public void Increments_slug_when_it_already_exists()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var db = Database.Open();
            db.Posts.Insert(Title: "A title", UrlSlug: "a-slug", IsPublised: false, CreatedOn: DateTime.UtcNow, LastUpdatedOn: DateTime.UtcNow);

            var slugManager = new SlugManager();

            var input = "a-slug";
            var uniqueSlug = slugManager.GetUniqueSlug(input);

            Assert.AreEqual("a-slug-2", uniqueSlug);
        }

        [Test]
        public void Increments_slug_multiple_times_when_it_already_exists()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var db = Database.Open();
            db.Posts.Insert(Title: "A title", UrlSlug: "a-slug", IsPublised: false, CreatedOn: DateTime.UtcNow, LastUpdatedOn: DateTime.UtcNow);
            db.Posts.Insert(Title: "A title", UrlSlug: "a-slug-2", IsPublised: false, CreatedOn: DateTime.UtcNow, LastUpdatedOn: DateTime.UtcNow);
            db.Posts.Insert(Title: "A title", UrlSlug: "a-slug-3", IsPublised: false, CreatedOn: DateTime.UtcNow, LastUpdatedOn: DateTime.UtcNow);

            var slugManager = new SlugManager();

            var input = "a-slug";
            var uniqueSlug = slugManager.GetUniqueSlug(input);

            Assert.AreEqual("a-slug-4", uniqueSlug);
        }
    }
}
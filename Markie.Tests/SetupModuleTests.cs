using NUnit.Framework;
using Nancy;
using Nancy.Testing;
using Simple.Data;

namespace Markie.Tests
{
    [TestFixture]
    public class SetupModuleTests
    {
        [Test]
        public void Get_redirects_to_login_when_at_least_one_user_exists()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var db = Database.Open();
            db.Users.Insert(Login: "me@example.com", HashedPassword: "hash", Salt: "salt", Guid: "5240cdc7-f32c-4d7f-9d27-f76a4a87d881");

            var browser = new Browser(BootstrapperFactory.Create());

            var response = browser.Get("/admin/setup", with => with.HttpRequest());

            response.ShouldHaveRedirectedTo("/admin/login");
        }

        [Test]
        public void Post_redirects_to_login_when_at_least_one_user_exists()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var db = Database.Open();
            db.Users.Insert(Login: "me@example.com", HashedPassword: "hash", Salt: "salt", Guid: "5240cdc7-f32c-4d7f-9d27-f76a4a87d881");

            var browser = new Browser(BootstrapperFactory.Create());

            var response = browser.Post("/admin/setup", with => with.HttpRequest());

            response.ShouldHaveRedirectedTo("/admin/login");
        }

        [Test]
        public void Returns_view_when_no_users_exists()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var browser = new Browser(BootstrapperFactory.Create());

            var response = browser.Get("/admin/setup", with => with.HttpRequest());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestCase("", "")]
        [TestCase("something", "")]
        [TestCase("", "something")]
        public void Returns_view_with_errormessage_when_invalid_data_is_posted(string login, string password)
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var browser = new Browser(BootstrapperFactory.Create());

            var response = browser.Post("/admin/setup", with =>
                {
                    with.HttpRequest();
                    with.FormValue("Login", login);
                    with.FormValue("Password", password);
                });

            response.ShouldHaveRedirectedTo("/admin/setup?error=true");
        }

        [Test]
        public void Creates_user_when_valid_data_is_posted()
        {
            const string login = "me@example.com";
            const string password = "password";

            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var browser = new Browser(BootstrapperFactory.Create());

            var response = browser.Post("/admin/setup", with =>
            {
                with.HttpRequest();
                with.FormValue("Login", login);
                with.FormValue("Password", password);
            });

            var db = Database.Open();

            var allUsers = db.Users.All().ToList();

            Assert.AreEqual(1, allUsers.Count);
            Assert.AreEqual(login, allUsers[0].Login);
            Assert.AreEqual(password + "salt", allUsers[0].HashedPassword);
            Assert.AreEqual("salt", allUsers[0].Salt);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
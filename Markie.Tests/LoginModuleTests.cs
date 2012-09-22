using System;
using System.Diagnostics;
using NUnit.Framework;
using Nancy;
using Nancy.Helpers;
using Nancy.Testing;
using Simple.Data;

namespace Markie.Tests
{
    [TestFixture]
    public class LoginModuleTests
    {
        [Test]
        public void Redirects_to_setup_when_no_user_exists()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var browser = new Browser(BootstrapperFactory.Create());

            var response = browser.Get("/admin/login", with => with.HttpRequest());

            response.ShouldHaveRedirectedTo("/admin/setup");
        }

        [Test]
        public void Returns_view_when_at_least_one_user_exists()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var db = Database.Open();
            db.Users.Insert(Login: "me@example.com", HashedPassword: "hash", Salt: "salt", Guid: "5240cdc7-f32c-4d7f-9d27-f76a4a87d881");

            var browser = new Browser(BootstrapperFactory.Create());

            var response = browser.Get("/admin/login", with => with.HttpRequest());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestCase("wrong", "wrong")]
        [TestCase("me@example.com", "wrong")]
        public void Shows_error_when_login_fails(string login, string password)
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var db = Database.Open();
            db.Users.Insert(Login: "me@example.com", HashedPassword: "hash", Salt: "salt", Guid: "5240cdc7-f32c-4d7f-9d27-f76a4a87d881");

            var browser = new Browser(BootstrapperFactory.Create());

            var response = browser.Post("/admin/login", with =>
                {
                    with.HttpRequest();
                    with.FormValue("Login", login);
                    with.FormValue("Password", password);
                });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            response.Body["#login-error"].ShouldExistOnce().And.ShouldContain("Invalid combination of login and password.", StringComparison.OrdinalIgnoreCase);
        }

        [Test]
        public void Redirects_when_login_succeeds()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var db = Database.Open();
            db.Users.Insert(Login: "me@example.com", HashedPassword: "passwordsalt", Salt: "salt", Guid: "5240cdc7-f32c-4d7f-9d27-f76a4a87d881");

            var browser = new Browser(BootstrapperFactory.Create());

            var response = browser.Post("/admin/login", with =>
                {
                    with.HttpRequest();
                    with.FormValue("Login", "me@example.com");
                    with.FormValue("Password", "password");
                });

            response.ShouldHaveRedirectedTo("/admin/drafts");
        }
    }
}
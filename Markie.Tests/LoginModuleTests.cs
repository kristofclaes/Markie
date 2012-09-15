using Markie.Modules;
using NUnit.Framework;
using Nancy;
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

            var bootstrapper = new ConfigurableBootstrapper(with => with.Module<LoginModule>());
            var browser = new Browser(bootstrapper);

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

            var bootstrapper = new ConfigurableBootstrapper(with =>
                {
                    with.Module<LoginModule>();
                    with.ViewEngine<Nancy.ViewEngines.Razor.RazorViewEngine>();
                });
            var browser = new Browser(bootstrapper);

            var response = browser.Get("/admin/login", with => with.HttpRequest());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
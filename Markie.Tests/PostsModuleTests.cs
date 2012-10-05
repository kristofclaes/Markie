using FakeItEasy;
using Markie.Authentication;
using Markie.Infrastructure;
using Markie.Tests.Fakes;
using NUnit.Framework;
using Nancy.Authentication.Forms;
using Nancy.Testing;
using Nancy.Testing.Fakes;
using Simple.Data;

namespace Markie.Tests
{
    [TestFixture]
    public class PostsModuleTests
    {
        [Test]
        public void Add_returns_correct_json()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var postStore = A.Fake<IPostStore>();
            var addDraftResult = new AddDraftResult
                {
                    Id = 1,
                    Success = true,
                    Title = "The Title",
                    Url = "/admin/posts/edit/1"
                };

            A.CallTo(() => postStore.AddDraft("The Title")).Returns(addDraftResult);

            FakeRootPathProvider.RootPath = "../../../Markie";

            var bootstrapper = new ConfigurableBootstrapper(with =>
                {
                    with.RootPathProvider(new FakeRootPathProvider());
                    with.Dependency<FakePasswordService>();
                    with.Dependency(postStore);
                    with.RequestStartup((container, pipelines, context) =>
                        {
                            var formsAuthConfiguration = new FormsAuthenticationConfiguration()
                                {
                                    RedirectUrl = "~/admin/login",
                                    UserMapper = new UserMapper()
                                };

                            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
                        });
                });
            var browser = new Browser(bootstrapper);

            var jsonResult = browser.Post("/admin/posts/add", with =>
                {
                    with.HttpRequest();
                    with.FormValue("title", "The Title");
                });

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<AddDraftResult>(jsonResult.Body.AsString());

            Assert.AreEqual(addDraftResult.Id, result.Id);
            Assert.AreEqual(addDraftResult.Success, result.Success);
            Assert.AreEqual(addDraftResult.Title, result.Title);
            Assert.AreEqual(addDraftResult.Url, result.Url);
        }
    }
}
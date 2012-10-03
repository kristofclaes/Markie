using Markie.Authentication;
using Markie.Tests.Fakes;
using Nancy.Authentication.Forms;
using Nancy.Testing;
using Nancy.Testing.Fakes;

namespace Markie.Tests
{
    public static class BootstrapperFactory
    {
        public static ConfigurableBootstrapper Create()
        {
            FakeRootPathProvider.RootPath = "../../../Markie";

            return new ConfigurableBootstrapper(with =>
                {
                    with.RootPathProvider(new FakeRootPathProvider());
                    with.Dependency<FakePasswordService>();
                    with.Dependency<FakePostStore>();
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
        }
    }
}
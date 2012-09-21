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
                });
        }
    }
}
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using TinyIoC;

namespace Markie
{
    public class MarkieBootstrapper : DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            var formsAuthConfiguration = new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/admin/login",
                    UserMapper = container.Resolve<IUserMapper>()
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
    }
}
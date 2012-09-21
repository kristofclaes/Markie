using Nancy;
using Simple.Data;
using System.Dynamic;

namespace Markie.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule()
        {
            Before += context =>
                {
                    var db = Database.Open();

                    if (db.Users.All().Count() == 0)
                    {
                        return Response.AsRedirect("~/admin/setup");
                    }

                    return null;
                };

            Get["/admin/login"] = parameters =>
                {
                    dynamic model = new ExpandoObject();
                    model.HasError = this.Request.Query.error.HasValue;

                    return View["Index.cshtml", model];
                };
        }
    }
}
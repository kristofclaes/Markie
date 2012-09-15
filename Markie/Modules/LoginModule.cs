using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Responses;
using Simple.Data;

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

            Get["/admin/login"] = parameters => View["Index.cshtml"];
        }
    }
}
using Markie.Authentication;
using Markie.ViewModels;
using Nancy;
using Nancy.ModelBinding;
using Simple.Data;
using System;

namespace Markie.Modules
{
    public class SetupModule : NancyModule
    {
        public SetupModule(IPasswordService passwordService)
        {
            Before += context =>
                {
                    var db = Database.Open();

                    if (db.Users.All().Count() > 0)
                    {
                        return Response.AsRedirect("~/admin/login");
                    }

                    return null;
                };

            Get["/admin/setup"] = parameters => View["Index.cshtml", new SetupViewModel()];

            Post["/admin/setup"] = parameters =>
                {
                    var model = this.Bind<SetupViewModel>();
                    model.Validate();

                    if (model.HasError)
                    {
                        return View["Index.cshtml", model];
                    }

                    string salt = passwordService.GenerateSalt();
                    string hashedPassword = passwordService.HashPassword(model.Password, salt);
                    string guid = Guid.NewGuid().ToString();

                    var db = Database.Open();
                    db.Users.Insert(Login: model.Login, HashedPassword: hashedPassword, Salt: salt, Guid: guid);

                    return View["Done.cshtml"];
                };
        }
    }
}
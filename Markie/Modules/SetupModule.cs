using Markie.Authentication;
using Markie.ViewModels;
using Nancy;
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
                    string login = Request.Form.Login;
                    string password = Request.Form.Password;

                    if (login.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace() || !login.IsValidEmailAddress())
                    {
                        return View["Index.cshtml", new SetupViewModel { Login = login, Password = password, HasError = true }];
                    }

                    string salt = passwordService.GenerateSalt();
                    string hashedPassword = passwordService.HashPassword(password, salt);
                    string guid = Guid.NewGuid().ToString();

                    var db = Database.Open();
                    db.Users.Insert(Login: login, HashedPassword: hashedPassword, Salt: salt, Guid: guid);

                    return View["Done.cshtml"];
                };
        }
    }
}
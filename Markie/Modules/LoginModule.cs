using Markie.Authentication;
using Markie.ViewModels;
using Nancy;
using Nancy.Authentication.Forms;
using Simple.Data;
using System;

namespace Markie.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule(IPasswordService passwordService)
        {
            Before += context =>
                {
                    var db = Database.Open();

                    if (db.Users.GetCount() == 0)
                    {
                        return Response.AsRedirect("~/admin/setup");
                    }

                    return null;
                };

            Get["/admin/login"] = parameters => View["Index.cshtml", new LoginViewModel()];

            Post["/admin/login"] = parameters =>
                {
                    string login = Request.Form.Login;
                    string password = Request.Form.Password;

                    var db = Database.Open();
                    var user = db.Users.FindAllByLogin(login).FirstOrDefault();

                    if (user == null)
                    {
                        return View["Index.cshtml", new LoginViewModel { Login = login, HasError = true }];
                    }

                    string salt = user.Salt;
                    string correctPassword = user.HashedPassword;

                    if (!passwordService.VerifyHashedPassword(password, salt, correctPassword))
                    {
                        return View["Index.cshtml", new LoginViewModel { Login = login, HasError = true }];
                    }

                    var guid = new Guid(user.Guid);

                    return this.LoginAndRedirect(guid, DateTime.Now.AddDays(7), "~/admin/posts");
                };
        }
    }
}
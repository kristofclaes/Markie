using System;
using Markie.Authentication;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Helpers;
using Simple.Data;
using System.Dynamic;

namespace Markie.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule(IPasswordService passwordService)
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
                    model.Login = HttpUtility.UrlDecode(this.Request.Query.login ?? "");

                    return View["Index.cshtml", model];
                };

            Post["/admin/login"] = parameters =>
                {
                    string login = Request.Form.Login;
                    string password = Request.Form.Password;

                    var db = Database.Open();
                    var user = db.Users.FindByLogin(login);

                    if (user == null)
                    {
                        return Response.AsRedirect("~/admin/login?error=true&login=" + HttpUtility.UrlEncode(login));
                    }

                    string salt = user.Salt;
                    string correctPassword = user.HashedPassword;

                    if (!passwordService.VerifyHashedPassword(password, salt, correctPassword))
                    {
                        return Response.AsRedirect("~/admin/login?error=true&login=" + HttpUtility.UrlEncode(login));
                    }

                    var guid = new Guid(user.Guid);

                    return this.LoginAndRedirect(guid, DateTime.Now.AddDays(7), "~/admin/drafts");
                };
        }
    }
}
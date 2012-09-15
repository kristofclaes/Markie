using System;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using Simple.Data;

namespace Markie.Authentication
{
    public class UserMapper : IUserMapper
    {
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var db = Database.Open();
            var user = db.Users.FindByGuid(identifier.ToString());

            return user == null ? null : new UserIdentity { UserName = user.Login };
        }
    }
}
using System.Web.Helpers;

namespace Markie.Authentication
{
    public interface IPasswordService
    {
        string GenerateSalt();
        string HashPassword(string password, string salt);
        bool VerifyHashedPassword(string password, string salt, string hashedPassword);
    }

    public class PasswordService : IPasswordService
    {
        public string GenerateSalt()
        {
            return Crypto.GenerateSalt();
        }

        public string HashPassword(string password, string salt)
        {
            return Crypto.HashPassword(password + salt);
        }

        public bool VerifyHashedPassword(string password, string salt, string hashedPassword)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, password + salt);
        }
    }
}
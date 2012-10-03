using Markie.Authentication;

namespace Markie.Tests.Fakes
{
    public class FakePasswordService : IPasswordService
    {
        public string GenerateSalt()
        {
            return "salt";
        }

        public string HashPassword(string password, string salt)
        {
            return password + salt;
        }

        public bool VerifyHashedPassword(string password, string salt, string hashedPassword)
        {
            return hashedPassword.Equals(password + salt);
        }
    }
}
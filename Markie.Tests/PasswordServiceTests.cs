using Markie.Authentication;
using NUnit.Framework;

namespace Markie.Tests
{
    [TestFixture]
    public class PasswordServiceTests
    {
        private const string Salt = "salt";
        private const string Password = "test";

        [Test]
        public void VerifyHashedPassword_returns_true_for_correct_password_and_correct_salt()
        {
            var passwordService = new PasswordService();

            var hashedPassword = passwordService.HashPassword(Password, Salt);

            Assert.IsTrue(passwordService.VerifyHashedPassword(Password, Salt, hashedPassword));
        }

        [Test]
        public void VerifyHashedPassword_returns_false_for_incorrect_password_and_correct_salt()
        {
            var passwordService = new PasswordService();

            var hashedPassword = passwordService.HashPassword(Password, Salt);

            Assert.IsFalse(passwordService.VerifyHashedPassword("nottest", Salt, hashedPassword));
        }

        [Test]
        public void VerifyHashedPassword_returns_false_for_correct_password_and_incorrect_salt()
        {
            var passwordService = new PasswordService();

            var hashedPassword = passwordService.HashPassword(Password, Salt);

            Assert.IsFalse(passwordService.VerifyHashedPassword(Password, "notsalt", hashedPassword));
        }

        [Test]
        public void VerifyHashedPassword_returns_false_for_incorrect_password_and_incorrect_salt()
        {
            var passwordService = new PasswordService();

            var hashedPassword = passwordService.HashPassword(Password, Salt);

            Assert.IsFalse(passwordService.VerifyHashedPassword("nottest", "notsalt", hashedPassword));
        }
    }
}
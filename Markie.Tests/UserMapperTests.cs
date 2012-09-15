using System;
using Markie.Authentication;
using NUnit.Framework;
using Simple.Data;

namespace Markie.Tests
{
    [TestFixture]
    public class UserMapperTests
    {
        [Test]
        public void returns_useridenty_when_guid_is_found()
        {
            const string guid = "91e2403b-c568-4116-9c87-b6b71b6acbe4";
            const string userName = "user@example.com";

            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var db = Database.Open();
            db.Users.Insert(Login: userName, HashedPassword: "pass", Salt: "salt", Guid: guid);

            var userMapper = new UserMapper();
            var identity = userMapper.GetUserFromIdentifier(new Guid(guid), null);

            Assert.AreEqual(userName, identity.UserName);
        }

        [Test]
        public void returns_null_when_guid_is_not_found()
        {
            var adapter = new InMemoryAdapter();
            Database.UseMockAdapter(adapter);

            var userMapper = new UserMapper();
            var identity = userMapper.GetUserFromIdentifier(Guid.NewGuid(), null);

            Assert.IsNull(identity);
        }
    }
}
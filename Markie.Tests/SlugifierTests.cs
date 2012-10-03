using Markie.Infrastructure;
using NUnit.Framework;

namespace Markie.Tests
{
    [TestFixture]
    public class SlugifierTests
    {
        [TestCase("test", "test")]
        [TestCase("", "")]
        [TestCase(null, "")]
        [TestCase("test ", "test")]
        [TestCase("test/with{incorrect)-characters", "test-with-incorrect-characters")]
        [TestCase("test something", "test-something")]
        [TestCase("Test", "test")]
        [TestCase("éåäöíØç", "eaaoioc")]
        public void Correctly_slugifies(string input, string expectedOutput)
        {
            var slugifier = new Slugifier();
            var slug = slugifier.Slugify(input);

            Assert.AreEqual(expectedOutput, slug);
        }
    }
}
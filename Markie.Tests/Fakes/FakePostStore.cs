using Markie.Infrastructure;

namespace Markie.Tests.Fakes
{
    public class FakePostStore : IPostStore
    {
        public AddDraftResult AddDraft(string title)
        {
            return new AddDraftResult();
        }
    }
}
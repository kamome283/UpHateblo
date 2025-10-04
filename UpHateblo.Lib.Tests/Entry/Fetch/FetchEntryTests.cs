using UpHateblo.Lib.Entry.Fetch;
using UpHateblo.Lib.Entry.Post;
using UpHateblo.Lib.Tests.Entry.Shared;
using UpHateblo.Lib.Tests.Shared;

namespace UpHateblo.Lib.Tests.Entry.Fetch;

public class FetchEntryTests : WebRequestTestBase
{
    [Fact]
    public async Task ItCanFetchEntry()
    {
        var posted =
            await PostEntry.Run(HttpClient, BlogConfig, EntityExamples.PostableEntryExample());
        EntryIdsToDispose.Enqueue(posted.EntryId);
        var fetched = await FetchEntry.Run(HttpClient, BlogConfig, posted.EntryId);
        Assert.Equal(posted, fetched);
    }
}
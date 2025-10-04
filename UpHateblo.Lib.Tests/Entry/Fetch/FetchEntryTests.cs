using UpHateblo.Lib.Entry.Fetch;
using UpHateblo.Lib.Entry.Post;
using UpHateblo.Lib.Tests.Entry.Shared;
using UpHateblo.Lib.Tests.Shared;

namespace UpHateblo.Lib.Tests.Entry.Fetch;

public class FetchEntryTests : WebRequestTestBase
{
    private static readonly PostableEntry Postable =
        EntityExamples.PostableEntryExample() with { Date = DateTime.Today };

    private static string? _entryId;

    public FetchEntryTests()
    {
        Task.Run(async () =>
        {
            var entry = await PostEntry.Run(HttpClient, BlogConfig, Postable);
            _entryId = entry.EntryId;
            EntryIdsToDispose.Enqueue(entry.EntryId);
        }).Wait();
    }

    [Fact]
    public async Task ItCanFetchEntry()
    {
        var fetched = await FetchEntry.Run(HttpClient, BlogConfig, _entryId!);
        Assert.Equal(Postable, fetched);
    }
}
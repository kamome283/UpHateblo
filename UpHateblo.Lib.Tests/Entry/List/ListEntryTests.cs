using UpHateblo.Lib.Entry.List;
using UpHateblo.Lib.Entry.Post;
using UpHateblo.Lib.Tests.Entry.Shared;
using UpHateblo.Lib.Tests.Shared;

namespace UpHateblo.Lib.Tests.Entry.List;

public class ListEntryTests : WebRequestTestBase
{
    public ListEntryTests()
    {
        foreach (var _ in Enumerable.Range(0, 10))
        {
            PostAndQueueEntry().Wait();
        }
    }

    [Fact]
    public Task ItCanListEntries()
    {
        var entries = ListEntry.Run(HttpClient, BlogConfig);
        Assert.Contains(entries, e => e.EntryId == FixedTargetEntryId);
        return Task.CompletedTask;
    }

    private async Task PostAndQueueEntry()
    {
        var entry = EntryExamples.PostableEntry();
        var res = await PostEntry.Run(HttpClient, BlogConfig, entry);
        EntryIdsToDispose.Enqueue(res.EntryId);
    }
}
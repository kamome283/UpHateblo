using UpHateblo.Lib.Entry.List;
using UpHateblo.Lib.Tests.Shared;

namespace UpHateblo.Lib.Tests.Entry.List;

public class ListEntryTests : WebRequestTestBase
{
    [Fact]
    public async Task ItCanListEntries()
    {
        var entries = await ListEntry.Run(HttpClient, BlogConfig);
        Assert.Contains(entries, e => e.EntryId == FixedTargetEntryId);
    }
}
using UpHateblo.Lib.Entry.List;
using UpHateblo.Lib.Tests.Shared;

namespace UpHateblo.Lib.Tests.Entry.List;

public class ListEntryCommandTests : WebRequestTestBase
{
    private static readonly HttpClient HttpClient = new();

    [Fact]
    public async Task ItCanListEntries()
    {
        var entries = await ListEntryCommand.Run(HttpClient, BlogConfig);
        Assert.Contains(entries, e => e.EntryId == FixedTargetEntryId);
    }
}
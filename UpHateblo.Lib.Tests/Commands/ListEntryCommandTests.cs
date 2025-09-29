using UpHateblo.Lib.Commands;

namespace UpHateblo.Lib.Tests.Commands;

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
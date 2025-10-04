using UpHateblo.Lib.Entry.Delete;
using UpHateblo.Lib.Entry.Post;
using UpHateblo.Lib.Tests.Entry.Shared;
using UpHateblo.Lib.Tests.Shared;

namespace UpHateblo.Lib.Tests.Entry.Delete;

public class DeleteEntryTests : WebRequestTestBase
{
    [Fact]
    public async Task ItCanDeleteEntry()
    {
        var entry = EntryExamples.PostableEntry();
        var res = await PostEntry.Run(HttpClient, BlogConfig, entry);
        await DeleteEntry.Run(HttpClient, BlogConfig, res.EntryId);
    }
}
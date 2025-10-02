using UpHateblo.Lib.Entry.Post;
using UpHateblo.Lib.Tests.Entry.Shared;
using UpHateblo.Lib.Tests.Shared;

namespace UpHateblo.Lib.Tests.Entry.Post;

public class PostEntryTests : WebRequestTestBase
{
    [Fact]
    public async Task ItCanPostEntry()
    {
        var entry = EntityExamples.PostableEntryExample();
        var fetched = await PostEntry.Run(HttpClient, BlogConfig, entry);

        Assert.Equal(entry.Title, fetched.Title);
        Assert.Equal(entry.Category, fetched.Category);
        Assert.Equal(entry.Content, fetched.Content);
        Assert.Equal(entry.CustomPath, fetched.CustomPath);
        Assert.Equal(entry.Date, fetched.Date);
        Assert.Equal(entry.Draft, fetched.Draft);
        Assert.Equal(entry.Preview, fetched.Preview);
        Assert.NotEmpty(fetched.EntryId);
    }
}
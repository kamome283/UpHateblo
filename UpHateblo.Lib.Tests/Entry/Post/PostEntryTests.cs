using UpHateblo.Lib.Entry.Post;
using UpHateblo.Lib.Tests.Shared;

namespace UpHateblo.Lib.Tests.Entry.Post;

public class PostEntryTests : WebRequestTestBase
{
    private static readonly HttpClient HttpClient = new();

    [Fact]
    public async Task ItCanPostEntry()
    {
        var updated = DateTime.Now;
        PostableEntry entry = new(
            Title: "PostCommandTest",
            Category: ["UpHateblo", "Post", "Test"],
            Content: $"""
                      This is a test content of post command.
                      Posted at {updated}
                      """,
            CustomPath: Guid.CreateVersion7().ToString(),
            Date: updated,
            Draft: true,
            Preview: false
        );
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
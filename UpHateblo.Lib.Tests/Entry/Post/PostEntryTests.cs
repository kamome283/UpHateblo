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
            CustomPath: null,
            Date: updated,
            Draft: true,
            Preview: false
        );
        await PostEntry.Run(HttpClient, BlogConfig, entry);
    }
}
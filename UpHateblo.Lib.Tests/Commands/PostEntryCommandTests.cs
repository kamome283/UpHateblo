using UpHateblo.Lib.Commands;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

public class PostEntryCommandTests : WebRequestTestBase
{
    private static readonly HttpClient HttpClient = new();

    [Fact]
    public async Task ItCanPostEntry()
    {
        var updated = DateTime.Now;
        Entry entry = new(
            Title: "PostCommandTest",
            Category: ["UpHateblo", "Post", "Test"],
            Content: $"""
                      This is a test content of post command.
                      Posted at {updated}
                      """,
            CustomPath: null,
            Updated: updated,
            Draft: true,
            Preview: false
        );
        await PostEntryCommand.Run(HttpClient, BlogConfig, entry);
    }
}
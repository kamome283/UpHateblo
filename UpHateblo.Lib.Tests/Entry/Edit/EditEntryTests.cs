using UpHateblo.Lib.Entry.Edit;
using UpHateblo.Lib.Tests.Shared;

namespace UpHateblo.Lib.Tests.Entry.Edit;

public class EditEntryTests : WebRequestTestBase
{
    private static readonly HttpClient HttpClient = new();

    [Fact]
    public async Task ItCanEditEntry()
    {
        var updated = DateTime.Now;
        EditableEntry entry = new(
            EntryId: FixedTargetEntryId,
            Title: "EditCommandTest",
            Category: ["UpHateblo", "Edit", "Test"],
            Content: $"""
                      This is a test content of edit command.
                      Updated at {updated}
                      """,
            CustomPath: Guid.CreateVersion7().ToString(),
            Date: updated,
            Draft: true,
            Preview: true
        );
        var fetched = await EditEntry.Run(HttpClient, BlogConfig, entry);

        Assert.Equal(entry.EntryId, fetched.EntryId);
        Assert.Equal(entry.Title, fetched.Title);
        Assert.Equal(entry.Category, fetched.Category);
        Assert.Equal(entry.Content, fetched.Content);
        Assert.Equal(entry.CustomPath, fetched.CustomPath);
        Assert.Equal(entry.Date, fetched.Date);
        Assert.Equal(entry.Draft, fetched.Draft);
        Assert.Equal(entry.Preview, fetched.Preview);
    }
}
using UpHateblo.Lib.Commands;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

public class EditEntryCommandTests : WebRequestTestBase
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
            CustomPath: null,
            Updated: updated,
            Draft: true,
            Preview: false
        );
        await EditEntryCommand.Run(HttpClient, BlogConfig, entry);
    }
}
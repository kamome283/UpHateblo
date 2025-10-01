using UpHateblo.Lib.Entry.Edit;
using UpHateblo.Lib.Tests.Shared;
using EditableEntry = UpHateblo.Lib.Entry.Edit.EditableEntry;

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
            CustomPath: null,
            Date: updated,
            Draft: true,
            Preview: false
        );
        await EditEntry.Run(HttpClient, BlogConfig, entry);
    }
}
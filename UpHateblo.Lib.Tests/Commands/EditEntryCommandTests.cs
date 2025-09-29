using UpHateblo.Lib.Commands;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

// 結合テストでないのでこのコマンドのテスト対象のエントリーは決め打ちにしている
public class EditEntryCommandTests : WebRequestTestBase
{
    private const string EntryId = "6802888565257240236";
    private static readonly HttpClient HttpClient = new();

    [Fact]
    public async Task ItCanPushEntry()
    {
        var updated = DateTime.Now;
        EditableEntry entry = new(
            EntryId: EntryId,
            Title: "PushCommandTest",
            Category: ["Push", "Test"],
            Content: $"""
                      This is a test content of Push command.
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
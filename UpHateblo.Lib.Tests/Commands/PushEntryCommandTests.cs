using UpHateblo.Lib.Commands;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

// 結合テストでないのでこのコマンドのテスト対象のエントリーは決め打ちにしている
public class PushEntryCommandTests : CommandTestsBase<BlogConfigSecrets>
{
    private const string EntryId = "6802888565257240236";
    private static readonly HttpClient HttpClient = new();

    // TODO: 1箇所にまとめる
    private BlogConfig BlogConfig =>
        new(Get("Blog:BlogId"), Get("Blog:Username"), Get("Blog:Password"));

    [Fact]
    public async Task ItCanPushEntry()
    {
        var updated = DateTime.Now;
        PushableEntry entry = new(
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
        await PushEntryCommand.Run(HttpClient, BlogConfig, entry);
    }
}
using UpHateblo.Lib.Commands;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

public class PushEntryCommandTests : CommandTestsBase<BlogConfigSecrets>
{
    // TODO: Need to dynamically fetch the entry ID of the posted blog entry during test execution
    private const string EntryId = "6802888565257240236";
    private static readonly HttpClient HttpClient = new();

    private BlogConfig BlogConfig =>
        new(Get("Blog:BlogId"), Get("Blog:Username"), Get("Blog:Password"));

    [Fact]
    public async Task ItCanPushEntry()
    {
        var customPath = Guid.CreateVersion7().ToString();
        Entry postEntry = new(
            Title: "PostCommandTest",
            Category: ["Test"],
            Date: DateTime.Parse("2022-12-31"),
            Content: "This is test content of Post command.",
            CustomPath: customPath,
            EntryId: EntryId,
            Draft: true,
            Preview: false
        );
        // TODO: 投稿したエントリーのエントリーIDを動的に取得できるようにした後にコメントアウトを外す
        // await PostEntryCommand.Run(HttpClient, BlogConfig, postEntry);
        Entry pushEntry = postEntry with
        {
            Title = "PushCommandTest",
            Category = ["Push", "Test"],
            Date = DateTime.Parse("2023-01-01"),
            Content = """
                      This is test content of Push command.
                      This is the second line of the test content.
                      """,
            Preview = true,
        };
        await PushEntryCommand.Run(HttpClient, BlogConfig, pushEntry);
    }
}
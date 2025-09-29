using UpHateblo.Lib.Commands;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

public class PostEntryCommandTests : CommandTestsBase<BlogConfigSecrets>
{
    private static readonly HttpClient HttpClient = new();

    private BlogConfig Blog =>
        new(Get("Blog:BlogId"), Get("Blog:Username"), Get("Blog:Password"));

    internal static Entry Entry => new(
        Title: "テスト",
        Category: ["Test"],
        Content: """
                 うまくいっているといいな
                 複数行
                 """,
        CustomPath: "test-path",
        Updated: DateTime.Parse("2025-09-23T21:29:00"),
        Draft: true,
        Preview: false
    );

    private static Entry CustomPathRandomizedEntry =>
        Entry with { CustomPath = Guid.CreateVersion7().ToString() };

    [Fact]
    public async Task ItCanPostEntry()
    {
        await PostEntryCommand.Run(HttpClient, Blog, CustomPathRandomizedEntry);
    }

    [Fact]
    public async Task ItCanPostProductionEntry()
    {
        var entry = CustomPathRandomizedEntry with { Draft = false };
        await PostEntryCommand.Run(HttpClient, Blog, entry);
    }

    [Fact(Skip = "プレビューフラグをオンにして投稿した結果がどのようなものになるか私がよくわかっていない")]
    public async Task ItCanPostPreviewEntry()
    {
        var entry = CustomPathRandomizedEntry with { Preview = true };
        await PostEntryCommand.Run(HttpClient, Blog, entry);
    }

    [Fact]
    public async Task ItCanPostMultipleCategoryEntry()
    {
        var entry = CustomPathRandomizedEntry with { Category = ["技術", "Test"] };
        await PostEntryCommand.Run(HttpClient, Blog, entry);
    }

    /// <summary>
    ///     同じUrlPathで投稿した場合:
    ///     二度目以降の投稿はカスタムURLの後ろに _1 などのサフィックスがつく
    /// </summary>
    [Fact]
    public async Task ItCanPostOnSameUrlPath()
    {
        var entry = CustomPathRandomizedEntry;
        await PostEntryCommand.Run(HttpClient, Blog, entry);
        await PostEntryCommand.Run(HttpClient, Blog, entry);
    }

    /// <summary>
    ///     空文字列をUrlPathに指定した場合:
    ///     下書きはカスタムURLが空の状態で投稿される。
    ///     本投稿はそのブログのデフォルトのルールでカスタムURLが採番される。
    /// </summary>
    [Fact]
    public async Task ItCanPostOnEmptyUrlPath()
    {
        var entry = CustomPathRandomizedEntry with { CustomPath = "" };
        await PostEntryCommand.Run(HttpClient, Blog, entry);
    }

    [Fact]
    public async Task ItCanPostWhenUrlPathIsNull()
    {
        var entry = CustomPathRandomizedEntry with { CustomPath = null };
        await PostEntryCommand.Run(HttpClient, Blog, entry);
    }
}
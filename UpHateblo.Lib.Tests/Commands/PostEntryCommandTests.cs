using System.Diagnostics.CodeAnalysis;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record PostEntryCommandSecrets(BlogConfig Blog);

public class PostEntryCommandTests : CommandTestsBase<PostEntryCommandSecrets>
{
    private static readonly HttpClient HttpClient = new();

    private BlogConfig Blog =>
        new(Get("Blog:BlogId"), Get("Blog:Username"), Get("Blog:Password"));

    internal static Entry Entry => new(
        "テスト",
        ["Test"],
        DateTime.Parse("2025-09-23T21:29:00"),
        """
        うまくいっているといいな
        複数行
        """,
        "test-path",
        true
    );

    private static Entry CustomPathRandomizedEntry =>
        Entry with { CustomPath = Guid.CreateVersion7().ToString() };

    [Fact]
    public async Task ItCanPostEntry()
    {
        await EntryCommands.Post(HttpClient, Blog, CustomPathRandomizedEntry);
    }

    [Fact]
    public async Task ItCanPostProductionEntry()
    {
        var entry = CustomPathRandomizedEntry with { Draft = false };
        await EntryCommands.Post(HttpClient, Blog, entry);
    }

    [Fact(Skip = "プレビューフラグをオンにして投稿した結果がどのようなものになるか私がよくわかっていない")]
    public async Task ItCanPostPreviewEntry()
    {
        var entry = CustomPathRandomizedEntry with { Preview = true };
        await EntryCommands.Post(HttpClient, Blog, entry);
    }

    [Fact]
    public async Task ItCanPostMultipleCategoryEntry()
    {
        var entry = CustomPathRandomizedEntry with { Category = ["技術", "Test"] };
        await EntryCommands.Post(HttpClient, Blog, entry);
    }

    /// <summary>
    ///     同じUrlPathで投稿した場合:
    ///     二度目以降の投稿はカスタムURLの後ろに _1 などのサフィックスがつく
    /// </summary>
    [Fact]
    public async Task ItCanPostOnSameUrlPath()
    {
        var entry = CustomPathRandomizedEntry;
        await EntryCommands.Post(HttpClient, Blog, entry);
        await EntryCommands.Post(HttpClient, Blog, entry);
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
        await EntryCommands.Post(HttpClient, Blog, entry);
    }

    [Fact]
    public async Task ItCanPostWhenUrlPathIsNull()
    {
        var entry = CustomPathRandomizedEntry with { CustomPath = null };
        await EntryCommands.Post(HttpClient, Blog, entry);
    }
}
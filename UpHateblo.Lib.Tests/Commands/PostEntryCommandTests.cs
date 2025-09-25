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

    internal static Entry Header => new(
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

    [Fact]
    public async Task ItCanPostEntry()
    {
        await EntryCommands.Post(HttpClient, Blog, UrlPathRandomizedHeader());
    }

    [Fact]
    public async Task ItCanPostProductionEntry()
    {
        var header = UrlPathRandomizedHeader() with { Draft = false };
        await EntryCommands.Post(HttpClient, Blog, header);
    }

    [Fact(Skip = "プレビューフラグをオンにして投稿した結果がどのようなものになるか私がよくわかっていない")]
    public async Task ItCanPostPreviewEntry()
    {
        var header = UrlPathRandomizedHeader() with { Preview = true };
        await EntryCommands.Post(HttpClient, Blog, header);
    }

    [Fact]
    public async Task ItCanPostMultipleCategoryEntry()
    {
        var header = UrlPathRandomizedHeader() with { Category = ["技術", "Test"] };
        await EntryCommands.Post(HttpClient, Blog, header);
    }

    /// <summary>
    ///     同じUrlPathで投稿した場合:
    ///     二度目以降の投稿はカスタムURLの後ろに _1 などのサフィックスがつく
    /// </summary>
    [Fact]
    public async Task ItCanPostOnSameUrlPath()
    {
        var header = UrlPathRandomizedHeader();
        await EntryCommands.Post(HttpClient, Blog, header);
        await EntryCommands.Post(HttpClient, Blog, header);
    }

    /// <summary>
    ///     空文字列をUrlPathに指定した場合:
    ///     下書きはカスタムURLが空の状態で投稿される。
    ///     本投稿はそのブログのデフォルトのルールでカスタムURLが採番される。
    /// </summary>
    [Fact]
    public async Task PostingOnEmptyUrlPathThrows()
    {
        var header = UrlPathRandomizedHeader() with { UrlPath = "" };
        await EntryCommands.Post(HttpClient, Blog, header);
    }

    private Entry UrlPathRandomizedHeader()
    {
        return Header with { UrlPath = Guid.CreateVersion7().ToString() };
    }
}
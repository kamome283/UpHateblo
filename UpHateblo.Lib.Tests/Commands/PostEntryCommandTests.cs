using System.Diagnostics.CodeAnalysis;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record BaseEntry(
    string Title,
    string Category,
    string Date,
    string UrlPath,
    string Content
);

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record PostEntrySecrets(BlogConfig BlogConfig, BaseEntry BaseEntry);

public class PostEntryCommandTests : CommandTestsBase<PostEntrySecrets>
{
    private static readonly HttpClient HttpClient = new();

    private BlogConfig Blog => new(Get("BlogConfig:BlogId"), Get("BlogConfig:Username"),
        Get("BlogConfig:Password"));

    private EntryHeader Header => new(
        Get("BaseEntry:Title"),
        Get("BaseEntry:Category").Split(","),
        DateTime.Parse(Get("BaseEntry:Date")),
        Get("BaseEntry:UrlPath"),
        Get("BaseEntry:Draft") == "true",
        Get("BaseEntry:Preview") == "true"
    );

    private string Content => Get("BaseEntry:Content");

    [Fact]
    public async Task ItCanPostEntry()
    {
        await EntryCommands.Post(HttpClient, Blog, UrlPathRandomizedHeader(), Content);
    }

    [Fact]
    public async Task ItCanPostProductionEntry()
    {
        var header = UrlPathRandomizedHeader() with { Draft = false };
        await EntryCommands.Post(HttpClient, Blog, header, Content);
    }

    [Fact(Skip = "プレビューフラグをオンにして投稿した結果がどのようなものになるか私がよくわかっていない")]
    public async Task ItCanPostPreviewEntry()
    {
        var header = UrlPathRandomizedHeader() with { Preview = true };
        await EntryCommands.Post(HttpClient, Blog, header, Content);
    }

    [Fact]
    public async Task ItCanPostMultipleCategoryEntry()
    {
        var header = UrlPathRandomizedHeader() with { Category = ["技術", "Test"] };
        await EntryCommands.Post(HttpClient, Blog, header, Content);
    }

    /// <summary>
    /// 同じUrlPathで投稿した場合:
    /// 二度目以降の投稿はカスタムURLの後ろに _1 などのサフィックスがつく
    /// </summary>
    [Fact]
    public async Task ItCanPostOnSameUrlPath()
    {
        var header = UrlPathRandomizedHeader();
        await EntryCommands.Post(HttpClient, Blog, header, Content);
        await EntryCommands.Post(HttpClient, Blog, header, Content);
    }

    /// <summary>
    /// 空文字列をUrlPathに指定した場合:
    /// 下書きはカスタムURLが空の状態で投稿される。
    /// 本投稿はそのブログのデフォルトのルールでカスタムURLが採番される。
    /// </summary>
    [Fact]
    public async Task PostingOnEmptyUrlPathThrows()
    {
        var header = UrlPathRandomizedHeader() with { UrlPath = "" };
        await EntryCommands.Post(HttpClient, Blog, header, Content);
    }

    private EntryHeader UrlPathRandomizedHeader()
    {
        return Header with { UrlPath = Guid.CreateVersion7().ToString() };
    }
}
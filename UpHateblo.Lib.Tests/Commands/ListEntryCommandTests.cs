using UpHateblo.Lib.Commands;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

public class ListEntryCommandTests : WebRequestTestBase<BlogConfigSecrets>
{
    private static readonly HttpClient HttpClient = new();

    private BlogConfig Blog =>
        new(Get("Blog:BlogId"), Get("Blog:Username"), Get("Blog:Password"));

    [Fact]
    public async Task ItCanListEntries()
    {
        var entries = await ListEntryCommand.Run(HttpClient, Blog);
        // 最低限のテスト
        // 実際に正しく動作しているかはデバッガーをつかって`entries`の値を確認する必要がある
        Assert.NotNull(entries);
    }
}
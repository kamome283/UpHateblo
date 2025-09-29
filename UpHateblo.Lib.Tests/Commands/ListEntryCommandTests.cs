using UpHateblo.Lib.Commands;

namespace UpHateblo.Lib.Tests.Commands;

public class ListEntryCommandTests : WebRequestTestBase
{
    private static readonly HttpClient HttpClient = new();

    [Fact]
    public async Task ItCanListEntries()
    {
        var entries = await ListEntryCommand.Run(HttpClient, BlogConfig);
        // 最低限のテスト
        // 実際に正しく動作しているかはデバッガーをつかって`entries`の値を確認する必要がある
        Assert.NotNull(entries);
    }
}
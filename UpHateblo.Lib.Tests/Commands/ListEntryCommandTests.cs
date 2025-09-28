using UpHateblo.Lib.Commands;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

public class ListEntryCommandTests : CommandTestsBase<BlogConfigSecrets>
{
    private static readonly HttpClient HttpClient = new();

    private BlogConfig Blog =>
        new(Get("Blog:BlogId"), Get("Blog:Username"), Get("Blog:Password"));

    [Fact]
    public async Task ItCanListEntries()
    {
        await ListEntryCommand.Run(HttpClient, Blog);
    }
}
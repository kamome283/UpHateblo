using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Schema;

namespace UpHateblo.Lib.Commands;

public static class PushEntryCommand
{
    private static readonly PostEntrySchema PostEntrySchema = new();

    public static async Task Run(
        HttpClient httpClient,
        BlogConfig blog,
        PushableEntry entry,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var httpContent =
            CommandHelper.GenHatenaContent(PostEntrySchema, blog, entry, wsseNonce, wsseDateTime);
        var memberEndpoint = new Uri($"{blog.EntryEndPoint}/{entry.EntryId}");
        var res = await httpClient.PutAsync(memberEndpoint, httpContent);
        res.EnsureSuccessStatusCode();
    }
}
using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Schema;

namespace UpHateblo.Lib.Commands;

public static class PostEntryCommand
{
    private static readonly PostEntrySchema PostEntrySchema = new();

    public static async Task Run(
        HttpClient httpClient,
        BlogConfig blog,
        Entry entry,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var httpContent =
            CommandHelper.GenHatenaContent(PostEntrySchema, blog, entry, wsseNonce, wsseDateTime);
        var res = await httpClient.PostAsync(blog.EntryEndPoint, httpContent);
        res.EnsureSuccessStatusCode();
    }
}
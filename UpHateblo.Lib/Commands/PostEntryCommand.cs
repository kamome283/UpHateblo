using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Http;
using UpHateblo.Lib.Schema;

namespace UpHateblo.Lib.Commands;

public static class PostEntryCommand
{
    public static async Task Run(
        HttpClient httpClient,
        BlogConfig blog,
        Entry entry,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var body = PostingEntrySchema.Serialize(blog, entry);
        var wsse = CommandHelper.GenerateWsse(blog, wsseNonce, wsseDateTime);
        var request = new HatenaRequest(HttpMethod.Post, blog.EntryEndPoint, body, wsse);

        var res = await httpClient.SendAsync(request);
        res.EnsureSuccessStatusCode();
    }
}
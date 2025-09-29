using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Http;
using UpHateblo.Lib.Schema;

namespace UpHateblo.Lib.Commands;

public static class EditEntryCommand
{
    public static async Task Run(
        HttpClient httpClient,
        BlogConfig blog,
        EditableEntry entry,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var endpoint = new Uri($"{blog.EntryEndPoint}/{entry.EntryId}");
        var body = PostEntrySchema.Serialize(blog, entry);
        var wsse = CommandHelper.GenerateWsse(blog, wsseNonce, wsseDateTime);
        var request = new HatenaRequest(HttpMethod.Put, endpoint, body, wsse);

        var res = await httpClient.SendAsync(request);
        res.EnsureSuccessStatusCode();
    }
}
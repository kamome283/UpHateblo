using UpHateblo.Lib.Shared;

namespace UpHateblo.Lib.Entry.Delete;

public static class DeleteEntry
{
    public static async Task Run(
        HttpClient httpClient,
        BlogConfig blog,
        string entryId,
        string? wsseNonce = null,
        DateTime? wsseDatetime = null)
    {
        var wsse = WsseHelper.GenerateWsse(blog, wsseNonce, wsseDatetime);
        var endpoint = new Uri($"{blog.EntryEndPoint}/{entryId}");
        var request = new HatenaRequest(HttpMethod.Delete, endpoint, null, wsse);

        var res = await httpClient.SendAsync(request);
        res.EnsureSuccessStatusCode();
    }
}
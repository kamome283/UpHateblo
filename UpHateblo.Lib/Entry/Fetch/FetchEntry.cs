using System.Xml.Linq;
using UpHateblo.Lib.Entry.Shared;
using UpHateblo.Lib.Shared;

namespace UpHateblo.Lib.Entry.Fetch;

public static class FetchEntry
{
    public static async Task<FetchedEntry> Run(
        HttpClient httpClient,
        BlogConfig blog,
        string entryId,
        string? wsseNonce = null,
        DateTime? wsseDatetime = null)
    {
        var endpoint = new Uri($"{blog.EntryEndPoint}/{entryId}");
        var wsse = CommandHelper.GenerateWsse(blog, wsseNonce, wsseDatetime);
        var request = new HatenaRequest(HttpMethod.Get, endpoint, null, wsse);

        var res = await httpClient.SendAsync(request);
        res.EnsureSuccessStatusCode();

        var content = await res.Content.ReadAsStringAsync();
        var xml = XDocument.Parse(content);
        var root = xml.Root!;
        var entry = FetchedEntrySchema.Deserialize(root);
        return entry;
    }
}
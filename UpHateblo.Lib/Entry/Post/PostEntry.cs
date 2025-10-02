using System.Xml.Linq;
using UpHateblo.Lib.Entry.List;
using UpHateblo.Lib.Entry.Shared;
using UpHateblo.Lib.Shared;

namespace UpHateblo.Lib.Entry.Post;

public static class PostEntry
{
    public static async Task Run(
    public static async Task<FetchedEntry> Run(
        HttpClient httpClient,
        BlogConfig blog,
        PostableEntry entry,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var body = PostingEntrySchema.Serialize(blog, entry);
        var wsse = WsseHelper.GenerateWsse(blog, wsseNonce, wsseDateTime);
        var request = new HatenaRequest(HttpMethod.Post, blog.EntryEndPoint, body, wsse);

        var res = await httpClient.SendAsync(request);
        res.EnsureSuccessStatusCode();

        var content = await res.Content.ReadAsStringAsync();
        var xml = XDocument.Parse(content);
        var root = xml.Root!;
        var fetchedEntry = FetchedEntrySchema.Deserialize(root);
        return fetchedEntry;
    }
}
using System.Xml.Linq;
using UpHateblo.Lib.Entry.Shared;
using UpHateblo.Lib.Shared;
using static UpHateblo.Lib.Shared.SchemaNamespaces;

namespace UpHateblo.Lib.Entry.List;

public static class ListEntry
{
    public static async Task<List<FetchedEntry>> Run(
        HttpClient httpClient,
        BlogConfig blog,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        Uri? next = blog.EntryEndPoint;
        List<FetchedEntry> results = [];
        while (next != null)
        {
            var (n, e) = await RunSingleIteration(next, httpClient, blog, wsseNonce, wsseDateTime);
            next = n;
            results.AddRange(e);
        }

        return results;
    }

    private static async Task<(Uri? next, IEnumerable<FetchedEntry> entries)> RunSingleIteration(
        Uri endpoint,
        HttpClient httpClient,
        BlogConfig blog,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var wsse = CommandHelper.GenerateWsse(blog, wsseNonce, wsseDateTime);
        var request = new HatenaRequest(HttpMethod.Get, endpoint, null, wsse);

        var res = await httpClient.SendAsync(request);
        res.EnsureSuccessStatusCode();

        var content = await res.Content.ReadAsStringAsync();
        var xml = XDocument.Parse(content);
        var root = xml.Root!;
        var next = NextUriSchema.Deserialize(root);
        var entries = root.Elements(AtomNs + "entry").Select(FetchedEntrySchema.Deserialize);
        return (next, entries);
    }
}
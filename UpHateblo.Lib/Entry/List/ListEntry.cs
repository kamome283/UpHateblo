using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UpHateblo.Lib.Entry.Shared;
using UpHateblo.Lib.Shared;
using static UpHateblo.Lib.Shared.SchemaNamespaces;

namespace UpHateblo.Lib.Entry.List;

public static class ListEntry
{
    public static async IAsyncEnumerable<FetchedEntry> Run(
        HttpClient httpClient,
        BlogConfig blog,
        [EnumeratorCancellation] CancellationToken cancellationToken = default,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        Uri? next = blog.EntryEndPoint;
        while (next != null && !cancellationToken.IsCancellationRequested)
        {
            var (n, entries) = await RunSingleIteration(next, httpClient, blog, cancellationToken,
                wsseNonce, wsseDateTime);
            next = n;
            foreach (var entry in entries)
            {
                yield return entry;
            }
        }
    }

    private static async Task<(Uri? next, IEnumerable<FetchedEntry> entries)> RunSingleIteration(
        Uri endpoint,
        HttpClient httpClient,
        BlogConfig blog,
        CancellationToken cancellationToken = default,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var wsse = CommandHelper.GenerateWsse(blog, wsseNonce, wsseDateTime);
        var request = new HatenaRequest(HttpMethod.Get, endpoint, null, wsse);

        var res = await httpClient.SendAsync(request, cancellationToken);
        res.EnsureSuccessStatusCode();

        var content = await res.Content.ReadAsStringAsync(cancellationToken);
        var xml = XDocument.Parse(content);
        var root = xml.Root!;
        var next = NextUriSchema.Deserialize(root);
        var entries = root.Elements(AtomNs + "entry").Select(FetchedEntrySchema.Deserialize);
        return (next, entries);
    }
}
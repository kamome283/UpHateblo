using System.Xml.Linq;
using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Http;
using UpHateblo.Lib.Schema;
using static UpHateblo.Lib.Schema.SchemaNamespaces;

namespace UpHateblo.Lib.Commands;

public static class ListEntryCommand
{
    // TODO: ペジネーションに対応させる
    public static async Task<FetchedEntry[]> Run(
        HttpClient httpClient,
        BlogConfig blog,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var wsse = CommandHelper.GenerateWsse(blog, wsseNonce, wsseDateTime);
        var request = new HatenaRequest(HttpMethod.Get, blog.EntryEndPoint, null, wsse);

        var res = await httpClient.SendAsync(request);
        res.EnsureSuccessStatusCode();

        var content = await res.Content.ReadAsStringAsync();
        var xml = XDocument.Parse(content);
        var entries = xml.Root!.Elements(AtomNs + "entry").Select(FetchedEntrySchema.Deserialize);
        return entries.ToArray();
    }
}
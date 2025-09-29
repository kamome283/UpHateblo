using System.Xml.Linq;
using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Http;
using UpHateblo.Lib.Schema;

namespace UpHateblo.Lib.Commands;

public static class ListEntryCommand
{
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
        // TODO: XMLの名前空間を一箇所に集める
        XNamespace atomNs = "http://www.w3.org/2005/Atom";
        var entries = xml.Root!.Elements(atomNs + "entry").Select(FetchedEntrySchema.Deserialize);
        return entries.ToArray();
    }
}
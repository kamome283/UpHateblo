using System.Xml.Linq;
using UpHateblo.Lib.Entry.Shared;
using UpHateblo.Lib.Shared;

namespace UpHateblo.Lib.Entry.Edit;

public static class EditEntry
{
    /// <remarks>下書きではないエントリに対してプレビューフラグを指定しても無効。</remarks>
    /// <remarks>一度プレビューフラグを有効にした場合、API経由でプレビューを無効にすることはできない。</remarks>
    /// <remarks>Dateプロパティのミリ秒部分は無視される</remarks>
    public static async Task<FetchedEntry> Run(
        HttpClient httpClient,
        BlogConfig blog,
        EditableEntry entry,
        CancellationToken cancellationToken = default,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var endpoint = new Uri($"{blog.EntryEndPoint}/{entry.EntryId}");
        var body = PostingEntrySchema.Serialize(blog, entry);
        var wsse = CommandHelper.GenerateWsse(blog, wsseNonce, wsseDateTime);
        var request = new HatenaRequest(HttpMethod.Put, endpoint, body, wsse);

        var res = await httpClient.SendAsync(request, cancellationToken);
        res.EnsureSuccessStatusCode();

        var content = await res.Content.ReadAsStringAsync(cancellationToken);
        var xml = XDocument.Parse(content);
        var root = xml.Root!;
        var fetchedEntry = FetchedEntrySchema.Deserialize(root);
        return fetchedEntry;
    }
}
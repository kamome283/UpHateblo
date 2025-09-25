using System.Diagnostics.CodeAnalysis;
using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Http;
using UpHateblo.Lib.Schema;

namespace UpHateblo.Lib;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class EntryCommands
{
    private static readonly PostEntrySchema PostEntrySchema = new();

    public static async Task Post(
        HttpClient httpClient,
        BlogConfig blog,
        Entry entry,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var httpContent =
            GenHatenaContent(PostEntrySchema, blog, entry, wsseNonce, wsseDateTime);
        var res = await httpClient.PostAsync(blog.EntryEndPoint, httpContent);
        res.EnsureSuccessStatusCode();
    }

    private static HatenaContent GenHatenaContent(
        EntrySchemaBase? schema,
        BlogConfig blog,
        Entry entry,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var xml = schema?.Serialize(blog, entry);
        var wsse = new Wsse(
            blog.Username,
            blog.Password,
            wsseNonce ?? Guid.CreateVersion7().ToString(),
            wsseDateTime ?? DateTime.Now
        );
        return new HatenaContent(xml, wsse);
    }
}
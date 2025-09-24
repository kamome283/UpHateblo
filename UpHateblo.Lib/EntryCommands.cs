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
        EntryHeader header,
        string content,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var httpContent =
            GenHatenaContent(PostEntrySchema, blog, header, content, wsseNonce, wsseDateTime);
        var res = await httpClient.PostAsync(blog.EntryEndPoint, httpContent);
        res.EnsureSuccessStatusCode();
    }

    private static HatenaContent GenHatenaContent(
        EntrySchemaBase? schema,
        BlogConfig blog,
        EntryHeader header,
        string content,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var entry = new Entry(blog, header, content);
        var xml = schema?.Serialize(entry);
        var wsse = new Wsse(
            entry.Blog.Username,
            entry.Blog.Password,
            wsseNonce ?? Guid.CreateVersion7().ToString(),
            wsseDateTime ?? DateTime.Now
        );
        return new HatenaContent(xml, wsse);
    }
}
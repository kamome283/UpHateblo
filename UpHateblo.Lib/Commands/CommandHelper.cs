using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Http;
using UpHateblo.Lib.Schema;

namespace UpHateblo.Lib.Commands;

internal static class CommandHelper
{
    public static HatenaContent GenHatenaContent(
        EntrySchemaBase? schema,
        BlogConfig blog,
        Entry? entry,
        string? wsseNonce = null,
        DateTime? wsseDateTime = null
    )
    {
        var xml = schema is not null && entry is not null ? schema.Serialize(blog, entry) : null;
        var wsse = new Wsse(
            blog.Username,
            blog.Password,
            wsseNonce ?? Guid.CreateVersion7().ToString(),
            wsseDateTime ?? DateTime.Now
        );
        return new HatenaContent(xml, wsse);
    }
}
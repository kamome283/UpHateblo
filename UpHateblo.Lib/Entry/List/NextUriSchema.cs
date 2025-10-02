using System.Xml.Linq;
using static UpHateblo.Lib.Shared.SchemaNamespaces;

namespace UpHateblo.Lib.Entry.List;

public static class NextUriSchema
{
    public static Uri? Deserialize(XElement element)
    {
        var node = element.Elements(AtomNs + "link")
            .FirstOrDefault(x => x.Attribute("rel")?.Value == "next");
        return node is not null ? new Uri(node.Attribute("href")!.Value) : null;
    }
}
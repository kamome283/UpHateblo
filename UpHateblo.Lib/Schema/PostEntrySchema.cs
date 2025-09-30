using System.Xml.Linq;
using UpHateblo.Lib.Entities;
using static UpHateblo.Lib.Schema.SchemaNamespaces;

namespace UpHateblo.Lib.Schema;

/// <remarks>
///     Reference:
///     https://developer.hatena.ne.jp/ja/documents/blog/apis/atom/#%E3%83%96%E3%83%AD%E3%82%B0%E3%82%A8%E3%83%B3%E3%83%88%E3%83%AA%E3%81%AE%E6%8A%95%E7%A8%BF
/// </remarks>
internal static class PostEntrySchema
{
    public static XDocument Serialize(BlogConfig blog, Entry entry)
    {
        return new XDocument(
            new XElement(AtomNs + "entry",
                new XAttribute(XNamespace.Xmlns + "app", AppNs),
                new XAttribute(XNamespace.Xmlns + "hatenablog", HatenaBlogNs),
                new XElement(AtomNs + "title", entry.Title),
                new XElement(AtomNs + "author",
                    new XElement(AtomNs + "name", blog.Username)
                ),
                new XElement(AtomNs + "content",
                    new XAttribute("type", "text/plain"),
                    new XText(entry.Content)
                ),
                entry.Updated is not null
                    ? new XElement(AtomNs + "updated", entry.Updated)
                    : null,
                entry.Category.Select(c => new XElement(AtomNs + "category",
                    new XAttribute("term", c))),
                new XElement(AppNs + "control",
                    new XElement(AppNs + "draft", entry.Draft == true ? "yes" : "no"),
                    new XElement(AppNs + "preview", entry.Preview == true ? "yes" : "no")
                ),
                entry.CustomPath is not null
                    ? new XElement(HatenaBlogNs + "custom-url",
                        new XText(entry.CustomPath)
                    )
                    : null
            )
        );
    }
}
using System.Xml.Linq;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Schema;

/// <remarks>
/// Reference:
/// https://developer.hatena.ne.jp/ja/documents/blog/apis/atom/#%E3%83%96%E3%83%AD%E3%82%B0%E3%82%A8%E3%83%B3%E3%83%88%E3%83%AA%E3%81%AE%E6%8A%95%E7%A8%BF
/// </remarks>
internal class PostEntrySchema: SchemaBase
{
    public override XDocument Serialize(Entry entry)
    {
        var blog = entry.Blog;
        var header = entry.Header;
        var content = entry.Content;

        return new XDocument(
            new XElement(AtomNs + "entry",
                new XAttribute(XNamespace.Xmlns + "app", AppNs),
                new XAttribute(XNamespace.Xmlns + "hatenablog", HatenaBlogNs),
                new XElement(AtomNs + "title", header.Title),
                new XElement(AtomNs + "author",
                    new XElement(AtomNs + "name", blog.Username)
                ),
                new XElement(AtomNs + "content",
                    new XAttribute("type", "text/plain"),
                    new XText(content)
                ),
                new XElement(AtomNs + "updated", header.Date),
                new XElement(AtomNs + "category",
                    header.Category.Select(c => new XAttribute("term", c))
                ),
                new XElement(AppNs + "control",
                    // Set yes before ready to use
                    new XElement(AppNs + "draft", "yes"),
                    // Not supported currently
                    new XElement(AppNs + "preview", "no")
                ),
                new XElement(HatenaBlogNs + "custom-url",
                    new XText(header.UrlPath)
                )
            )
        );
    }
}
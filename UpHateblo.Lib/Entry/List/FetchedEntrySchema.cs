using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using static UpHateblo.Lib.Shared.SchemaNamespaces;

namespace UpHateblo.Lib.Entry.List;

internal static class FetchedEntrySchema
{
    private static readonly Regex UrlEntryPathRegex = new("/entry/(.*)$");

    private static readonly string[] ContentTypes =
        ["text/x-markdown", "text/x-hatena-syntax", "text/html"];

    public static FetchedEntry Deserialize(XElement entryElement)
    {
        try
        {
            var (content, contentType) = entryElement.ParseContentAndType();
            var entry = new FetchedEntry(
                EntryId: entryElement.ParseEntryId(),
                Title: entryElement.ParseTitle(),
                Category: entryElement.ParseCategory(),
                Content: content,
                AbsoluteCustomPath: entryElement.ParseCustomPath(),
                AbsoluteDate: entryElement.ParseUpdated(),
                AbsoluteDraft: entryElement.ParseDraft(),
                AbsolutePreview: entryElement.ParseAbsolutePreview(),
                Published: entryElement.ParsePublished(),
                ContentType: contentType
            );
            return entry;
        }
        catch (Exception e) when (e is InvalidOperationException or NullReferenceException)
        {
            throw new XmlException("Failed to deserialize to FetchedEntry", e);
        }
    }

    private static string ParseEntryId(this XElement root)
    {
        var node = root.Elements(AtomNs + "link").First(x => x.Attribute("rel")?.Value == "edit");
        var href = node.Attribute("href")!.Value;
        return GetEntryPath(href);
    }

    private static string ParseTitle(this XElement root)
    {
        return root.Element(AtomNs + "title")!.Value;
    }

    private static HashSet<string> ParseCategory(this XElement root)
    {
        var nodes = root.Elements(AtomNs + "category");
        return nodes.Select(x => x.Attribute("term")!.Value).ToHashSet();
    }

    private static string ParseCustomPath(this XElement root)
    {
        var node = root.Elements(AtomNs + "link")
            .First(x => x.Attribute("rel")?.Value == "alternate");
        var href = node.Attribute("href")!.Value;
        return GetEntryPath(href);
    }

    private static DateTime ParseUpdated(this XElement root)
    {
        return DateTime.Parse(root.Element(AtomNs + "updated")!.Value);
    }

    private static bool ParseDraft(this XElement root)
    {
        var text = root.Element(AppNs + "control")!.Element(AppNs + "draft")!.Value;
        return text == "yes";
    }

    private static bool ParseAbsolutePreview(this XElement root)
    {
        var text = root.Element(AppNs + "control")!.Element(AppNs + "preview")!.Value;
        return text == "yes";
    }

    private static DateTime ParsePublished(this XElement root)
    {
        return DateTime.Parse(root.Element(AtomNs + "published")!.Value);
    }

    private static (string Content, string ContentType) ParseContentAndType(this XElement root)
    {
        // var nodes = root.Descendants().Where(x => x.Name.LocalName == "content").ToArray();
        var nodes = root.Elements(AtomNs + "content").ToArray();

        foreach (var type in ContentTypes)
        {
            var content = nodes
                .FirstOrDefault(x => x?.Attribute("type")?.Value == type, null)
                ?.Value;
            if (content != null)
            {
                return (content, type);
            }
        }

        throw new InvalidOperationException();
    }

    private static string GetEntryPath(string url)
    {
        return UrlEntryPathRegex.Match(url).Groups[1].Value;
    }
}
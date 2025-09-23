using System.Xml.Serialization;

namespace UpHateblo.Lib;

public record BlogConfig(
    string BlogId,
    string Username,
    string Password
);

public record EntryHeader(
    string Title,
    string[] Category,
    DateTime Date,
    string UrlPath
);

internal static class PostRequestSchema
{
    // Reference: https://developer.hatena.ne.jp/ja/documents/blog/apis/atom/
    [XmlRoot("entry")]
    public class PostEntryRequestSchema(BlogConfig config, EntryHeader header, string content)
    {
        [XmlElement("title")] public readonly string Title = header.Title;
        [XmlElement("author")] public readonly Author Author = new(config.Username);
        [XmlElement("content")] public readonly Content Content = new(content);
        [XmlElement("updated")] public readonly DateTime Updated = header.Date;
        [XmlElement("category")] public readonly Category Category = new(header.Category);

        // No support for draft and preview post currently
        [XmlElement("app:control")] public readonly AppControl AppControl = new(false, false);

        [XmlElement("hatenablog:custom-url")]
        public readonly CustomUrl CustomUrl = new(header.UrlPath);
    }

    public class Author(string name)
    {
        [XmlElement("name")] public readonly string Name = name;
    }

    public class Content(string content)
    {
        [XmlAttribute("type")] public readonly string Type = "text/plain";
        [XmlText] public readonly string Text = content;
    }

    public class Category(string[] category)
    {
        [XmlAttribute("term")] public readonly string[] Term = category;
    }

    public class AppControl(bool draft, bool preview)
    {
        [XmlElement("app:draft")] public readonly string Draft = draft ? "yes" : "no";
        [XmlElement("app:preview")] public readonly string Preview = preview ? "yes" : "no";
    }

    public class CustomUrl(string urlPath)
    {
        [XmlAttribute("xmlns:hatenablog")]
        public readonly string HatenaBlog = "http://www.hatena.ne.jp/info/xmlns#hatenablog";

        [XmlText] public readonly string Text = urlPath;
    }
}

public static class Post
{
    public static async Task Run(
        HttpClient client,
        BlogConfig blog,
        EntryHeader header,
        string content)
    {
        throw new NotImplementedException();
    }
}
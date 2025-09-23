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
    public class PostEntryRequestSchema(BlogConfig config, EntryHeader header, string content)
    {
        public readonly string Title = header.Title;
        public readonly Author Author = new(config.Username);
        public readonly Content Content = new(content);
        public readonly DateTime Updated = header.Date;
        public readonly Category Category = new(header.Category);

        // No support for draft and preview post currently
        [XmlArray("app:control")] public readonly AppControl AppControl = new(false, false);

        [XmlArray("hatenablog:custom-url")]
        public readonly CustomUrl CustomUrl = new(header.UrlPath);
    }

    public class Author(string name)
    {
        public readonly string Name = name;
    }

    public class Content(string content)
    {
        [XmlAttribute] public readonly string Type = "text/plain";
        [XmlElement] public readonly string Element = content;
    }

    public class Category(string[] category)
    {
        [XmlAttribute] public readonly string[] Term = category;
    }

    public class AppControl(bool draft, bool preview)
    {
        [XmlArray("app:draft")] public readonly string Draft = draft ? "yes" : "no";
        [XmlArray("app:preview")] public readonly string Preview = preview ? "yes" : "no";
    }

    public class CustomUrl(string urlPath)
    {
        [XmlAttribute("xmlns:hatenablog")]
        public readonly string HatenaBlog = "http://www.hatena.ne.jp/info/xmlns#hatenablog";

        [XmlElement] public readonly string Element = urlPath;
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
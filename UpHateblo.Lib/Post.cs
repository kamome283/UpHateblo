using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Xml.Serialization;

namespace UpHateblo.Lib;

public record BlogConfig(
    string BlogId,
    string Username,
    string Password
)
{
    public Uri EntryEndPoint =>
        new Uri($"https://blog.hatena.ne.jp/${Username}/${BlogId}/atom/entry");
}

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

public sealed class Utf8StringWriter : StringWriter
{
    public override Encoding Encoding => Encoding.UTF8;
}

public class Post(HttpClient client, BlogConfig blog, EntryHeader header, string content)
{
    private static readonly XmlSerializer Serializer =
        new XmlSerializer(typeof(PostRequestSchema.PostEntryRequestSchema));

    private static readonly XmlSerializerNamespaces XmlNamespaces = GetHatenaXmlNamespaces();

    public async Task Run()
    {
        var serialized = GetSerializedContent();
        var wsseToken =
            new Wsse(blog.Username, blog.Password, Guid.NewGuid().ToString(), DateTime.Now)
                .GetToken();
        var httpContent = GetHttpContent(serialized, wsseToken);
        var res = await client.PostAsync(blog.EntryEndPoint, httpContent);
        res.EnsureSuccessStatusCode();
    }

    private StringContent GetHttpContent(string body, string wsseToken)
    {
        StringContent httpContent = new StringContent(body);
        httpContent.Headers.ContentType =
            MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Xml);
        httpContent.Headers.Add("X-WSSE", wsseToken);
        return httpContent;
    }

    private string GetSerializedContent()
    {
        var entry = new PostRequestSchema.PostEntryRequestSchema(blog, header, content);
        // staticにして使いまわすこともできるけど、
        // 毎回フラッシュしないといけないのでバグの原因になりうる
        // 別にIOを行うわけでもなし、毎回インスタンスを生成してもいいと考えている
        using var writer = new Utf8StringWriter();
        Serializer.Serialize(writer, entry, XmlNamespaces);
        return writer.GetStringBuilder().ToString();
    }

    private static XmlSerializerNamespaces GetHatenaXmlNamespaces()
    {
        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add("", "http://www.w3.org/2005/Atom");
        namespaces.Add("app", "http://www.w3.org/2007/app");
        return namespaces;
    }
}
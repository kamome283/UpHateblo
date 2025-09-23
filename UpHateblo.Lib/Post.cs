using System.Net.Http.Headers;
using System.Net.Mime;
using System.Xml.Linq;

namespace UpHateblo.Lib;

public record BlogConfig(
    string BlogId,
    string Username,
    string Password
)
{
    public Uri EntryEndPoint =>
        new Uri($"https://blog.hatena.ne.jp/{Username}/{BlogId}/atom/entry");
}

public record EntryHeader(
    string Title,
    string[] Category,
    DateTime Date,
    string UrlPath
);

public class Post(HttpClient client, BlogConfig blog, EntryHeader header, string content)
{
    public async Task Run()
    {
        var xml = GetRequestXml();
        var serialized = xml.ToString();
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

    /// <returns>エントリーのポスト用のXML</returns>
    /// <remarks>
    /// References:
    /// https://learn.microsoft.com/ja-jp/dotnet/api/system.xml.linq.xdocument?view=net-8.0
    /// https://learn.microsoft.com/ja-jp/dotnet/standard/linq/create-document-namespaces-csharp
    /// https://neue.cc/2009/08/18_189.html
    /// </remarks>
    private XDocument GetRequestXml()
    {
        XNamespace atom = "http://www.w3.org/2005/Atom";
        XNamespace app = "http://www.w3.org/2007/app";
        XNamespace hatenablog = "http://www.hatena.ne.jp/info/xmlns#hatenablog";

        return new XDocument(
            new XElement(atom + "entry",
                new XAttribute(XNamespace.Xmlns + "app", app),
                new XAttribute(XNamespace.Xmlns + "hatenablog", hatenablog),

                new XElement(atom + "title", header.Title),
                new XElement(atom + "author",
                    new XElement(atom + "name", blog.Username)
                ),
                new XElement(atom + "content",
                    new XAttribute("type", "text/plain"),
                    new XText(content)
                ),
                new XElement(atom + "updated", header.Date),
                new XElement(atom + "category",
                    header.Category.Select(c => new XAttribute("term", c))
                ),
                new XElement(app + "control",
                    // Set yes before ready to use
                    new XElement(app + "draft", "yes"),
                    // Not supported currently
                    new XElement(app + "preview", "no")
                ),
                new XElement(hatenablog + "custom-url",
                    new XText(header.UrlPath)
                )
            )
        );
    }
}
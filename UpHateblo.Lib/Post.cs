using System.Net.Http.Headers;
using System.Net.Mime;
using System.Xml.Linq;
using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Http;

namespace UpHateblo.Lib;

public class Post(HttpClient client, Entry entry)
{
    private BlogConfig Blog => entry.Blog;
    private EntryHeader Header => entry.Header;
    private string Content => entry.Content;

    public async Task Run()
    {
        var xml = GetRequestXml();
        var serialized = xml.ToString();
        var wsseToken =
            new Wsse(Blog.Username, Blog.Password, Guid.NewGuid().ToString(), DateTime.Now)
                .GetToken();
        var httpContent = GetHttpContent(serialized, wsseToken);
        var res = await client.PostAsync(Blog.EntryEndPoint, httpContent);
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

                new XElement(atom + "title", Header.Title),
                new XElement(atom + "author",
                    new XElement(atom + "name", Blog.Username)
                ),
                new XElement(atom + "content",
                    new XAttribute("type", "text/plain"),
                    new XText(Content)
                ),
                new XElement(atom + "updated", Header.Date),
                new XElement(atom + "category",
                    Header.Category.Select(c => new XAttribute("term", c))
                ),
                new XElement(app + "control",
                    // Set yes before ready to use
                    new XElement(app + "draft", "yes"),
                    // Not supported currently
                    new XElement(app + "preview", "no")
                ),
                new XElement(hatenablog + "custom-url",
                    new XText(Header.UrlPath)
                )
            )
        );
    }
}
using System.Xml.Linq;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Schema;

/// <summary>HTTPリクエストのボディとして送るXMLのスキーマを定義するクラス</summary>>
/// <remarks>
/// References for Request Body:
/// https://developer.hatena.ne.jp/ja/documents/blog/apis/atom/
/// References for XDocument:
/// https://learn.microsoft.com/ja-jp/dotnet/api/system.xml.linq.xdocument?view=net-8.0
/// https://learn.microsoft.com/ja-jp/dotnet/standard/linq/create-document-namespaces-csharp
/// https://neue.cc/2009/08/18_189.html
/// </remarks>
internal abstract class SchemaBase(Entry entry)
{
    protected static readonly XNamespace AtomNs = "http://www.w3.org/2005/Atom";
    protected static readonly XNamespace AppNs = "http://www.w3.org/2007/app";

    protected static readonly XNamespace HatenaBlogNs =
        "http://www.hatena.ne.jp/info/xmlns#hatenablog";

    protected BlogConfig Blog => entry.Blog;
    protected EntryHeader Header => entry.Header;
    protected string Content => entry.Content;

    public abstract XDocument Serialize();
}
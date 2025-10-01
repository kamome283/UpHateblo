using System.Xml.Linq;

namespace UpHateblo.Lib.Shared;

internal static class SchemaNamespaces
{
    public static readonly XNamespace AtomNs = "http://www.w3.org/2005/Atom";
    public static readonly XNamespace AppNs = "http://www.w3.org/2007/app";

    public static readonly XNamespace HatenaBlogNs =
        "http://www.hatena.ne.jp/info/xmlns#hatenablog";
}
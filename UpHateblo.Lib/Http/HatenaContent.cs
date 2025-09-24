using System.Net.Http.Headers;
using System.Net.Mime;
using System.Xml.Linq;

namespace UpHateblo.Lib.Http;

internal class HatenaContent : StringContent
{
    public HatenaContent(XDocument? xml, Wsse wsse) : base(xml?.ToString() ?? "")
    {
        Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Xml);
        Headers.Add("X-WSSE", wsse.GetToken());
    }
}
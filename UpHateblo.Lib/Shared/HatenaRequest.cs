using System.Net.Mime;
using System.Text;
using System.Xml.Linq;

namespace UpHateblo.Lib.Shared;

internal class HatenaRequest : HttpRequestMessage
{
    public HatenaRequest(HttpMethod method, Uri endpoint, XDocument? body, Wsse wsse)
        : base(method, endpoint)
    {
        Headers.Add("X-WSSE", wsse.GetToken());
        if (body is not null)
        {
            Content = new StringContent(body.ToString(), Encoding.UTF8,
                MediaTypeNames.Application.Xml);
        }
    }
}
using System.Xml.Linq;
using UpHateblo.Lib.Http;

namespace UpHateblo.Lib.Tests.Http;

public class HatenaRequestTests
{
    private static string BodyExample =>
        """
        <?xml version="1.0" encoding="utf-8"?>
        <entry xmlns="http://www.w3.org/2005/Atom"
               xmlns:app="http://www.w3.org/2007/app">
            <title>新しいタイトル</title>
            <author><name>name</name></author>
            <content type="text/plain">
                ** 新しい本文
            </content>
            <updated>2008-01-01T00:00:00</updated>
            <category term="Scala" />
            <app:control>
                <app:draft>no</app:draft>
                <app:preview>no</app:preview>
            </app:control>
             <hatenablog:custom-url xmlns:hatenablog="http://www.hatena.ne.jp/info/xmlns#hatenablog">2009-happy-new-year</hatenablog:custom-url>
        </entry>
        """;

    [Fact]
    public async Task IsValidHttpRequestMessageWithBody()
    {
        var body = XDocument.Parse(BodyExample);
        var request = new HatenaRequest(
            HttpMethod.Get,
            new Uri("http://localhost:3000"),
            body,
            WsseTests.ValidityTestInstance
        );

        Assert.Equal(
            request.Headers.First(kv => kv.Key == "X-WSSE").Value.First(),
            WsseTests.ValidityTestInstance.GetToken()
        );

        var actualContent = await request.Content!.ReadAsStringAsync();
        var actualBody = XDocument.Parse(actualContent);
        Assert.Equal(body.ToString(), actualBody.ToString());
    }
}
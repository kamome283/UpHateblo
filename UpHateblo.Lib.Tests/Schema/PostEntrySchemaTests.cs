using System.Xml.Linq;
using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Schema;
using UpHateblo.Lib.Tests.Commands;

namespace UpHateblo.Lib.Tests.Schema;

public class PostEntrySchemaTests
{
    static public string ExpectedContent =>
        """
        <entry xmlns:app="http://www.w3.org/2007/app" xmlns:hatenablog="http://www.hatena.ne.jp/info/xmlns#hatenablog" xmlns="http://www.w3.org/2005/Atom">
          <title>テスト</title>
          <author>
            <name>Kamome283</name>
          </author>
          <content type="text/plain">うまくいっているといいな
        複数行</content>
          <updated>2025-09-23T21:29:00</updated>
          <category term="Test" />
          <app:control>
            <app:draft>yes</app:draft>
            <app:preview>no</app:preview>
          </app:control>
          <hatenablog:custom-url>test-path</hatenablog:custom-url>
        </entry>
        """;

    [Fact]
    public void IsValidSchema()
    {
        var expected = XDocument.Parse(ExpectedContent);

        var schema = new PostEntrySchema();
        var blog = new BlogConfig("kamome283.hatenablog.com", "Kamome283", "foobar");
        var entry = new Entry(blog, PostEntryCommandTests.Header, PostEntryCommandTests.Content);
        var actual = schema.Serialize(entry);

        Assert.Equal(expected.ToString(), actual.ToString());
    }
}
using System.Xml.Linq;
using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Schema;

namespace UpHateblo.Lib.Tests.Schema;

public class PostEntrySchemaTests
{
    private static Entry Entry => new(
        Title: "テスト",
        Category: ["Test"],
        Content: """
                 うまくいっているといいな
                 複数行
                 """,
        CustomPath: "test-path",
        Updated: DateTime.Parse("2025-09-23T21:29:00"),
        Draft: true,
        Preview: false
    );

    private static BlogConfig DefaultBlogConfig =>
        new("kamome283.hatenablog.com", "Kamome283", "foobar");

    private static string DefaultExpectedContent =>
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

    private static string ExpectedContentWhenUrlPathIsNull =>
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
        </entry>
        """;

    public static object[][] TestCases =>
    [
        [DefaultBlogConfig, Entry, DefaultExpectedContent],
        [
            DefaultBlogConfig, Entry with { CustomPath = null },
            ExpectedContentWhenUrlPathIsNull
        ]
    ];

    [Theory]
    [MemberData(nameof(TestCases))]
    public void SchemaIsValid(BlogConfig blog, Entry entry, string expectedContent)
    {
        var expected = XDocument.Parse(expectedContent);
        var actual = PostEntrySchema.Serialize(blog, entry);
        Assert.Equal(expected.ToString(), actual.ToString());
    }
}
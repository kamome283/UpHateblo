using System.Xml.Linq;
using UpHateblo.Lib.Entry.Shared;
using UpHateblo.Lib.Shared;

namespace UpHateblo.Lib.Tests.Schema;

public class PostingEntrySchemaTests
{
    private static readonly BlogConfig BlogConfig =
        new("kamome283.hatenablog.com", "Kamome283", "foobar");

    [Fact]
    public void IsValidWhenAllFieldsAreFulfilled()
    {
        var entry = new Entry.Post.Entry(
            Title: "Test",
            Category: ["Category1", "Category2"],
            Content: """
                     First line.
                     Second line.
                     """,
            CustomPath: "test/custom/path",
            Date: DateTime.Parse("2025-09-23T21:29:00"),
            Draft: true,
            Preview: false
        );
        const string expectedBody =
            """
            <entry xmlns:app="http://www.w3.org/2007/app" xmlns:hatenablog="http://www.hatena.ne.jp/info/xmlns#hatenablog" xmlns="http://www.w3.org/2005/Atom">
              <title>Test</title>
              <author>
                <name>Kamome283</name>
              </author>
              <content type="text/plain">First line.
            Second line.</content>
              <updated>2025-09-23T21:29:00</updated>
              <category term="Category1" />
              <category term="Category2" />
              <app:control>
                <app:draft>yes</app:draft>
                <app:preview>no</app:preview>
              </app:control>
              <hatenablog:custom-url>test/custom/path</hatenablog:custom-url>
            </entry>
            """;
        var expected = XDocument.Parse(expectedBody);
        var actual = PostingEntrySchema.Serialize(BlogConfig, entry);
        Assert.Equal(expected.ToString(), actual.ToString());
    }

    [Fact]
    public void IsValidWhenNullableFieldsAreNull()
    {
        var entry = new Entry.Post.Entry(
            Title: "Test",
            Category: ["Category1", "Category2"],
            Content: """
                     First line.
                     Second line.
                     """,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null
        );
        const string expectedBody =
            """
            <entry xmlns:app="http://www.w3.org/2007/app" xmlns:hatenablog="http://www.hatena.ne.jp/info/xmlns#hatenablog" xmlns="http://www.w3.org/2005/Atom">
              <title>Test</title>
              <author>
                <name>Kamome283</name>
              </author>
              <content type="text/plain">First line.
            Second line.</content>
              <category term="Category1" />
              <category term="Category2" />
              <app:control>
                <app:draft>no</app:draft>
                <app:preview>no</app:preview>
              </app:control>
            </entry>
            """;
        var expected = XDocument.Parse(expectedBody);
        var actual = PostingEntrySchema.Serialize(BlogConfig, entry);
        Assert.Equal(expected.ToString(), actual.ToString());
    }
}
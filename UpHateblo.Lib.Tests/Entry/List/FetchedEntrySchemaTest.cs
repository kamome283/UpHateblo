using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using UpHateblo.Lib.Entry.List;
using static UpHateblo.Lib.Shared.SchemaNamespaces;

namespace UpHateblo.Lib.Tests.Entry.List;

public class FetchedEntrySchemaTest
{
    private static readonly string ResponseBody =
        """
        <?xml version="1.0" encoding="utf-8" standalone="no"?>
        <feed xmlns="http://www.w3.org/2005/Atom" xmlns:app="http://www.w3.org/2007/app">
            <link href="https://blog.hatena.ne.jp/Kamome283/kamome283.hatenablog.com/atom/entry"
                  rel="first"/>
            <title>Kamome283</title>
            <link href="https://kamome283.hatenablog.com/" rel="alternate"/>
            <updated>2025-09-23T21:29:00+09:00</updated>
            <author>
                <name>Kamome283</name>
            </author>
            <generator uri="https://blog.hatena.ne.jp/" version="e106732cc5d5b9efb3b4458b2dbb2a">
                Hatena::Blog
            </generator>
            <id>hatenablog://blog/8454420450069751439</id>
            <entry>
                <id>tag:blog.hatena.ne.jp,2013:blog-Kamome283-8454420450069751439-6802888565257240236</id>
                <link href="https://blog.hatena.ne.jp/Kamome283/kamome283.hatenablog.com/atom/entry/6802888565257240236"
                      rel="edit"/>
                <link href="https://kamome283.hatenablog.com/entry/01998a00-4a65-7eb1-9d79-ddb822fc17b5"
                      rel="alternate" type="text/html"/>
                <link href="https://kamome283.hatenablog.com/draft/entry/qESFitds1djAmaB3ZhwO8qCWhT4"
                      rel="preview" type="text/html"/>
                <author>
                    <name>Kamome283</name>
                </author>
                <title>HasCategories</title>
                <updated>2023-01-01T00:00:00+09:00</updated>
                <published>2022-09-27T15:55:17+09:00</published>
                <app:edited>2025-09-27T16:08:19+09:00</app:edited>
                <summary type="text">First line. Second line.
                </summary>
                <content type="text/x-markdown">First line.
        Second line.</content>
                <hatena:formatted-content xmlns:hatena="http://www.hatena.ne.jp/info/xmlns#"
                                          type="text/html">&lt;p&gt;First line.
                    Second line.&lt;/p&gt;
                </hatena:formatted-content>
                <category term="Deserialize"/>
                <category term="Test"/>
                <app:control>
                    <app:draft>yes</app:draft>
                    <app:preview>yes</app:preview>
                </app:control>
            </entry>
            <entry>
                <id>tag:blog.hatena.ne.jp,2013:blog-Kamome283-8454420450069751439-6802888565257240236</id>
                <link href="https://blog.hatena.ne.jp/Kamome283/kamome283.hatenablog.com/atom/entry/6802888565257240236"
                      rel="edit"/>
                <link href="https://kamome283.hatenablog.com/entry/01998a00-4a65-7eb1-9d79-ddb822fc17b5"
                      rel="alternate" type="text/html"/>
                <link href="https://kamome283.hatenablog.com/draft/entry/qESFitds1djAmaB3ZhwO8qCWhT4"
                      rel="preview" type="text/html"/>
                <author>
                    <name>Kamome283</name>
                </author>
                <title>NoCategories</title>
                <updated>2023-01-01T00:00:00+09:00</updated>
                <published>2022-09-27T15:55:17+09:00</published>
                <app:edited>2025-09-27T16:08:19+09:00</app:edited>
                <summary type="text">First line. Second line.
                </summary>
                <content type="text/x-markdown">First line.
        Second line.</content>
                <hatena:formatted-content xmlns:hatena="http://www.hatena.ne.jp/info/xmlns#"
                                          type="text/html">&lt;p&gt;First line.
                    Second line.&lt;/p&gt;
                </hatena:formatted-content>
                <app:control>
                    <app:draft>yes</app:draft>
                    <app:preview>yes</app:preview>
                </app:control>
            </entry>
        </feed>
        """;

    [Fact]
    public void ItCanDeserializeValidResponseBody()
    {
        var xml = XDocument.Parse(ResponseBody);
        var elements = xml
            .Elements(AtomNs + "feed")
            .Elements(AtomNs + "entry")
            .Select(FetchedEntrySchema.Deserialize)
            .ToArray();
        Assert.Equal(2, elements.Length);
        Assert.All(elements, TestEntry);
        return;

        void TestEntry(FetchedEntry entry)
        {
            switch (entry.Title)
            {
                case "HasCategories":
                    TestHasCategories(entry);
                    break;
                case "NoCategories":
                    TestHasNoCategories(entry);
                    break;
                default:
                    throw new ValidationException($"Unexpected title: {entry.Title}");
            }

            Assert.Equal("6802888565257240236", entry.EntryId);
            Assert.Equal("""
                         First line.
                         Second line.
                         """, entry.Content);
            Assert.Equal("01998a00-4a65-7eb1-9d79-ddb822fc17b5", entry.CustomPath);
            Assert.Equal(DateTime.Parse("2023-01-01T00:00:00+09:00"), entry.Date);
            Assert.True(entry.Draft);
            Assert.True(entry.Preview);
            Assert.Equal(DateTime.Parse("2022-09-27T15:55:17+09:00"), entry.Published);
            Assert.Equal("text/x-markdown", entry.ContentType);
        }

        void TestHasCategories(FetchedEntry entry) =>
            Assert.Equal(entry.Category, ["Test", "Deserialize"]);

        void TestHasNoCategories(FetchedEntry entry) =>
            Assert.Empty(entry.Category);
    }
}
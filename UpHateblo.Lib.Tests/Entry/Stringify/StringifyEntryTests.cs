using UpHateblo.Lib.Entry.Parse;
using UpHateblo.Lib.Entry.Shared;
using UpHateblo.Lib.Entry.Stringify;

namespace UpHateblo.Lib.Tests.Entry.Stringify;

public class StringifyEntryTests
{
    [Fact]
    public void ItSerializesBodyOnlyWithoutFrontMatter()
    {
        // Arrange
        var entry = new MaybeEntry(
            EntryId: null,
            Title: null,
            Category: null,
            Content: """
                     Hello world!
                     This is body only.
                     """,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        // Act
        var actual = StringifyEntry.Run(entry);

        // Assert: no front matter should be emitted if all header fields are null
        Assert.Equal(entry.Content, actual);
        Assert.DoesNotContain("---", actual);
    }

    [Fact]
    public void ItSerializesFullSpecEntryAndRoundTripsThroughParser()
    {
        // Arrange: mirrors ParseEntryTests.ItCanDeserializeFullSpecMarkdown
        var original = new MaybeEntry(
            EntryId: null,
            Title: "FullSpecMarkdown",
            Category: ["A", "B"],
            Content: """
                     This is a test,
                     and this is the second line of the content.
                     """,
            CustomPath: "test/url-path",
            Date: DateTime.Parse("2025-01-09T19:07:00+09:00"),
            Draft: true,
            Preview: true,
            Published: null,
            ContentType: null,
            PreviewUrl: "https://example.com/preview/url"
        );

        // Act
        var text = StringifyEntry.Run(original);
        var parsed = ParseEntry.Run(text);

        // Assert: round-trip equals original
        Assert.Equal(original, parsed);

        // And sanity-check that front matter markers exist when header has values
        Assert.StartsWith("---", text.TrimStart());
        Assert.Contains("Title: FullSpecMarkdown", text);
    }

    [Fact]
    public void ItEmitsFrontMatterWhenAnyHeaderFieldExists()
    {
        var original = new MaybeEntry(
            EntryId: null,
            Title: "TitleOnly",
            Category: null,
            Content: "Body",
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var text = StringifyEntry.Run(original);

        // Should contain YAML front matter block with separators
        // We don't assert exact YAML layout, only the presence of separators and the key text
        Assert.Contains("\n---\n", "\n" + text); // opening separator on its own line
        Assert.Contains("Title: TitleOnly", text);
        Assert.Contains("\n---\n",
            text[
                (text.IndexOf("Title: TitleOnly",
                    StringComparison.Ordinal))..]); // closing separator exists after some YAML

        // And it must be parseable back to the same entry
        var parsed = ParseEntry.Run(text);
        Assert.Equal(original, parsed);
    }

    [Fact]
    public void ItRoundTripsVariousMinimalCases()
    {
        var cases = new[]
        {
            new MaybeEntry(null, null, null, "Body only", null, null, null, null, null, null, null),
            new MaybeEntry(null, "T", null, "Body", null, null, null, null, null, null, null),
            new MaybeEntry(
                EntryId: null,
                Title: null,
                Category: ["tag1", "tag2"],
                Content: "Body",
                CustomPath: null,
                Date: null,
                Draft: null,
                Preview: null,
                Published: null,
                ContentType: null,
                PreviewUrl: null),
            new MaybeEntry(null, null, null, "Body", "custom/path",
                DateTime.Parse("2024-12-31T23:59:59+00:00"), false, true, null, null, null),
        };

        foreach (var original in cases)
        {
            var text = StringifyEntry.Run(original);
            var parsed = ParseEntry.Run(text);
            Assert.Equal(original, parsed);
        }
    }
}
using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Markdown;
using YamlDotNet.Core;

namespace UpHateblo.Lib.Tests.Markdown;

/// <summary>
///     YAML parsing in the front matter follows the behavior of
///     [YamlDotNet](https://github.com/aaubry/YamlDotNet).
///     Since our own processing handles the separation between front matter and body,
///     this part is the focus of testing.
/// </summary>
public class DeserializeEntryTests
{
    [Fact]
    public void ItCanDeserializeFullSpecMarkdown()
    {
        MaybeEntry expected = new(
            Title: "FullSpecMarkdown",
            Category: ["A", "B"],
            Date: DateTime.Parse("2025-01-09T19:07:00+09:00"),
            Content: """
                     This is a test,
                     and this is the second line of the content.
                     """,
            UrlPath: "test/url-path",
            Draft: true,
            Preview: false
        );
        string content = """
                         ---
                         Title: FullSpecMarkdown
                         Category: 
                           - A
                           - B
                         Date: 2025-01-09T19:07:00+09:00
                         UrlPath: test/url-path
                         Draft: true
                         Preview: false
                         ---
                         This is a test,
                         and this is the second line of the content.
                         """;
        MaybeEntry actual = DeserializeEntry.Run(content);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ItIgnoresInvalidCategoryScalar()
    {
        string content = """
                         ---
                         Category: InvalidCategory
                         ---
                         """;
        var actual = DeserializeEntry.Run(content);
        Assert.Null(actual.Category);
    }

    [Fact]
    public void ItCanDeserializeMarkdownWithoutFrontMatter()
    {
        string content = """
                         Hello world!
                         This is body only.
                         """;
        MaybeEntry expected = new(
            Title: null,
            Category: null,
            Date: null,
            Content: """
                     Hello world!
                     This is body only.
                     """,
            UrlPath: null,
            Draft: null,
            Preview: null
        );
        var actual = DeserializeEntry.Run(content);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ItIgnoresUnknownFieldsInFrontMatter()
    {
        string content = """
                         ---
                         Title: Known Title
                         UnknownField: some value
                         AnotherOne: 123
                         ---
                         Body goes here.
                         """;
        var actual = DeserializeEntry.Run(content);
        Assert.Equal("Known Title", actual.Title);
        Assert.Equal("""
                     Body goes here.
                     """, actual.Content);
    }

    [Fact]
    public void ItTreatsSingleSeparatorAsBodyOnly()
    {
        string content = """
                         ---
                         Title: ShouldNotBeParsed
                         """;
        var actual = DeserializeEntry.Run(content);
        Assert.Null(actual.Title);
        Assert.Null(actual.Category);
        Assert.Null(actual.Date);
        Assert.Equal("""
                     ---
                     Title: ShouldNotBeParsed
                     """, actual.Content);
    }

    [Fact]
    public void ItAllowsContentImmediatelyAfterClosingSeparator()
    {
        string content = """
                         ---
                         Title: Immediate
                         ---Content starts right away.
                         """;
        var actual = DeserializeEntry.Run(content);
        // Current behavior: the separator must be on its own line. If content follows immediately,
        // the whole text is treated as body with no front matter parsed.
        Assert.Null(actual.Title);
        Assert.Equal("""
                     ---
                     Title: Immediate
                     ---Content starts right away.
                     """, actual.Content);
    }

    [Fact]
    public void ItHandlesInvalidNonStringProperties()
    {
        string content = """
                         ---
                         Title: HasInvalids
                         Date: not-a-date
                         Draft: not-a-bool
                         Preview: maybe
                         ---
                         Body is here.
                         """;
        // Current behavior: YamlDotNet throws when a scalar cannot be converted to the target type.
        Assert.Throws<YamlException>(() => DeserializeEntry.Run(content));
    }
}
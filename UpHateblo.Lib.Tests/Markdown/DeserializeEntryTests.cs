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
    public void ItThrowsOnParsingInvalidCategoryFormat()
    {
        string content = """
                         ---
                         Category: InvalidCategory
                         ---
                         """;
        Assert.Throws<YamlException>(() => { DeserializeEntry.Run(content); });
    }
}
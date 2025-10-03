using UpHateblo.Lib.Entry.Parse;
using VYaml.Serialization;

namespace UpHateblo.Lib.Tests.Entry.Parse;

/// <summary>
///     YAML parsing in the front matter follows the behavior of
///     [YamlDotNet](https://github.com/aaubry/YamlDotNet).
///     Since our own processing handles the separation between front matter and body,
///     this part is the focus of testing.
/// </summary>
public class ParseEntryTests
{
    [Fact]
    public void ItCanDeserializeFullSpecMarkdown()
    {
        MaybeEntry expected = new MaybeEntry(
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
        string content = """
                         ---
                         Title: FullSpecMarkdown
                         Category: 
                           - A
                           - B
                         Date: 2025-01-09T19:07:00+09:00
                         CustomPath: test/url-path
                         Draft: true
                         Preview: true
                         PreviewUrl: https://example.com/preview/url
                         ---
                         This is a test,
                         and this is the second line of the content.
                         """;
        MaybeEntry actual = ParseEntry.Run(content);
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
        var actual = ParseEntry.Run(content);
        Assert.Null(actual.Category);
    }

    [Fact]
    public void ItCanDeserializeMarkdownWithoutFrontMatter()
    {
        string content = """
                         Hello world!
                         This is body only.
                         """;
        MaybeEntry expected = new MaybeEntry(
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
        var actual = ParseEntry.Run(content);
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
        var actual = ParseEntry.Run(content);
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
        var actual = ParseEntry.Run(content);
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
        var actual = ParseEntry.Run(content);
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
        // Current behavior: VYaml throws when a scalar cannot be converted to the target type.
        Assert.Throws<YamlSerializerException>(() => ParseEntry.Run(content));
    }

    [Fact]
    public async Task ItIsThreadSafeForConcurrentDeserialization()
    {
        // Prepare a set of representative inputs
        string fullSpec = """
                          ---
                          Title: FullSpecMarkdown
                          Category: 
                            - A
                            - B
                          Date: 2025-01-09T19:07:00+09:00
                          CustomPath: test/url-path
                          Draft: true
                          Preview: false
                          ---
                          This is a test,
                          and this is the second line of the content.
                          """;
        string bodyOnly = """
                          Hello world!
                          This is body only.
                          """;
        string unknownFields = """
                               ---
                               Title: Known Title
                               UnknownField: some value
                               AnotherOne: 123
                               ---
                               Body goes here.
                               """;

        string[] inputs = [fullSpec, bodyOnly, unknownFields];

        // Take single-threaded baselines to compare against
        var baselines = inputs.Select(ParseEntry.Run).ToArray();

        const int iterations = 200;
        var tasks = Enumerable.Range(0, iterations)
            .SelectMany(_ => inputs.Select((input, idx) => Task.Run(() =>
            {
                var actual = ParseEntry.Run(input);
                Assert.Equal(baselines[idx], actual);
            })));

        await Task.WhenAll(tasks);
    }

    [Fact]
    public async Task ItConsistentlyThrowsConcurrentlyForInvalidNonStringProperties()
    {
        string invalidContent = """
                                ---
                                Title: HasInvalids
                                Date: not-a-date
                                Draft: not-a-bool
                                Preview: maybe
                                ---
                                Body is here.
                                """;

        const int iterations = 200;
        var tasks = Enumerable.Range(0, iterations)
            .Select(_ => Task.Run(() =>
                Assert.ThrowsAny<Exception>(() =>
                    ParseEntry.Run(invalidContent))
            ));

        await Task.WhenAll(tasks);
    }
}
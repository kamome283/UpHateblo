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
        MaybeEntry expected = new MaybeEntry
        {
            Title = "FullSpecMarkdown", Category = ["A", "B"],
            Updated = DateTime.Parse("2025-01-09T19:07:00+09:00"),
            Content = """
                      This is a test,
                      and this is the second line of the content.
                      """,
            CustomPath = "test/url-path",
            Draft = true,
            Preview = false
        };
        string content = """
                         ---
                         Title: FullSpecMarkdown
                         Category: 
                           - A
                           - B
                         Updated: 2025-01-09T19:07:00+09:00
                         CustomPath: test/url-path
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
        MaybeEntry expected = new MaybeEntry
        {
            Content = """
                      Hello world!
                      This is body only.
                      """
        };
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
        Assert.Null(actual.Updated);
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
                         Updated: not-a-date
                         Draft: not-a-bool
                         Preview: maybe
                         ---
                         Body is here.
                         """;
        // Current behavior: YamlDotNet throws when a scalar cannot be converted to the target type.
        Assert.Throws<YamlException>(() => DeserializeEntry.Run(content));
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
                          Updated: 2025-01-09T19:07:00+09:00
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
        var baselines = inputs.Select(DeserializeEntry.Run).ToArray();

        const int iterations = 200;
        var tasks = Enumerable.Range(0, iterations)
            .SelectMany(_ => inputs.Select((input, idx) => Task.Run(() =>
            {
                var actual = DeserializeEntry.Run(input);
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
                Assert.Throws<YamlException>(() =>
                    DeserializeEntry.Run(invalidContent))
            ));

        await Task.WhenAll(tasks);
    }
}
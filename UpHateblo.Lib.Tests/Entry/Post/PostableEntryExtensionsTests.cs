using System.ComponentModel.DataAnnotations;
using UpHateblo.Lib.Entry.Post;
using MaybeEntry = UpHateblo.Lib.Entry.Read.MaybeEntry;

namespace UpHateblo.Lib.Tests.Entry.Post;

public class PostableEntryExtensionsTests
{
    [Fact]
    public void MaterializeThrowsWhenTitleIsMissing()
    {
        var maybe = new MaybeEntry(
            EntryId: null,
            Title: null,
            Category: new HashSet<string>(["tech"]),
            Content: "body",
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null);

        var ex =
            Assert.Throws<ValidationException>(() => PostableEntryExtensions.Materialize(maybe));
        Assert.Equal("Missing properties: Title", ex.Message);
    }

    [Fact]
    public void MaterializeThrowsWhenCategoryIsMissing()
    {
        var maybe = new MaybeEntry(
            EntryId: null,
            Title: "t",
            Category: null,
            Content: "body",
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null);

        var ex =
            Assert.Throws<ValidationException>(() => PostableEntryExtensions.Materialize(maybe));
        Assert.Equal("Missing properties: Category", ex.Message);
    }

    [Fact]
    public void MaterializeThrowsWhenContentIsMissing()
    {
        var maybe = new MaybeEntry(
            EntryId: null,
            Title: "t",
            Category: new HashSet<string>(["tech"]),
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null);

        var ex =
            Assert.Throws<ValidationException>(() => PostableEntryExtensions.Materialize(maybe));
        Assert.Equal("Missing properties: Content", ex.Message);
    }

    [Fact]
    public void MaterializeThrowsWhenAllMissing()
    {
        var maybe = new MaybeEntry(
            EntryId: null,
            Title: null,
            Category: null,
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null);

        var ex =
            Assert.Throws<ValidationException>(() => PostableEntryExtensions.Materialize(maybe));
        Assert.Equal("Missing properties: Title, Category, Content", ex.Message);
    }

    [Fact]
    public void MaterializeCreatesEntryWithAllFields()
    {
        var categories = new HashSet<string>(["c#", "tech"]);
        var maybe = new MaybeEntry(
            EntryId: "ignored-entry-id",
            Title: "My Title",
            Category: categories,
            Content: "Hello",
            CustomPath: "/custom/path",
            Date: DateTime.Parse("2024-01-02T03:04:05Z").ToUniversalTime(),
            Draft: true,
            Preview: false,
            Published: DateTime.Parse("2024-02-03T04:05:06Z").ToUniversalTime(),
            ContentType: "text/x-markdown");

        var entry = PostableEntryExtensions.Materialize(maybe);

        var expected = new PostableEntry(
            Title: "My Title",
            Category: categories,
            Content: "Hello",
            CustomPath: "/custom/path",
            Date: DateTime.Parse("2024-01-02T03:04:05Z").ToUniversalTime(),
            Draft: true,
            Preview: false);

        Assert.Equal(expected, entry);
        // also assert fields individually to be explicit
        Assert.Equal("My Title", entry.Title);
        Assert.Equal(new[] { "c#", "tech" }.ToHashSet(), entry.Category);
        Assert.Equal("Hello", entry.Content);
        Assert.Equal("/custom/path", entry.CustomPath);
        Assert.Equal(DateTime.Parse("2024-01-02T03:04:05Z").ToUniversalTime(), entry.Date);
        Assert.True(entry.Draft);
        Assert.False(entry.Preview);
    }
}
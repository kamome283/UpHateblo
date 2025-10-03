using System.ComponentModel.DataAnnotations;
using UpHateblo.Lib.Entry.Edit;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Lib.Tests.Entry.Edit;

public class EditableEntryExtensionsTests
{
    [Fact]
    public void MaterializeThrowsWhenEntryIdIsMissing()
    {
        var maybe = new MaybeEntry(
            EntryId: null,
            Title: "t",
            Category: new HashSet<string>(["tech"]),
            Content: "body",
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var ex = Assert.Throws<ValidationException>(() => maybe.MaterializeEditable());
        Assert.Equal("Missing properties: EntryId", ex.Message);
    }

    [Fact]
    public void MaterializeThrowsWhenAllRequiredMissing()
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
            ContentType: null,
            PreviewUrl: null
        );

        var ex = Assert.Throws<ValidationException>(() => maybe.MaterializeEditable());
        Assert.Equal("Missing properties: Title, Category, Content, EntryId", ex.Message);
    }

    [Fact]
    public void MaterializeCreatesEditableEntryWithAllFields()
    {
        var categories = new HashSet<string>(["c#", "tech"]);
        var maybe = new MaybeEntry(
            EntryId: "entry-123",
            Title: "My Title",
            Category: categories,
            Content: "Hello",
            CustomPath: "/custom/path",
            Date: DateTime.Parse("2024-01-02T03:04:05Z").ToUniversalTime(),
            Draft: true,
            Preview: false,
            Published: DateTime.Parse("2024-02-03T04:05:06Z").ToUniversalTime(),
            ContentType: "text/x-markdown",
            PreviewUrl: null
        );

        var editable = maybe.MaterializeEditable();

        Assert.Equal("entry-123", editable.EntryId);
        Assert.Equal("My Title", editable.Title);
        Assert.Equal(new[] { "c#", "tech" }.ToHashSet(), editable.Category);
        Assert.Equal("Hello", editable.Content);
        Assert.Equal("/custom/path", editable.CustomPath);
        Assert.Equal(DateTime.Parse("2024-01-02T03:04:05Z").ToUniversalTime(), editable.Date);
        Assert.True(editable.Draft);
        Assert.False(editable.Preview);
    }
}

internal static class EditableEntryTestHelpers
{
    // Provide an extension to disambiguate which Materialize we call from MaybeEntry
    public static EditableEntry MaterializeEditable(this MaybeEntry maybe) =>
        EditableEntryExtensions.Materialize(maybe);
}
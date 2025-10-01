using UpHateblo.Lib.Entry.Post;
using EditableEntry = UpHateblo.Lib.Entry.Edit.EditableEntry;
using FetchedEntry = UpHateblo.Lib.Entry.List.FetchedEntry;

namespace UpHateblo.Lib.Tests.Entry.Post;

public class PostableEntryOperatorTests
{
    private static readonly PostableEntry BaseEntry = new(
        Title: "Test Title",
        Category: ["tech", "c#"],
        Content: "Test content",
        CustomPath: "/test/custom/path",
        Date: DateTime.Parse("2023-10-02"),
        Draft: false,
        Preview: false
    );

    [Fact]
    public void EntryEqualsFetched_WhenBaseFieldsMatch()
    {
        var fetched = new FetchedEntry(
            EntryId: "entry-id",
            Title: BaseEntry.Title,
            Category: [.. BaseEntry.Category],
            Content: BaseEntry.Content,
            AbsoluteCustomPath: BaseEntry.CustomPath!,
            AbsoluteDate: BaseEntry.Date!.Value,
            AbsoluteDraft: BaseEntry.Draft!.Value,
            AbsolutePreview: BaseEntry.Preview!.Value,
            Published: DateTime.Parse("2023-10-01"),
            ContentType: "text/x-markdown"
        );

        Assert.True(BaseEntry == fetched);
        Assert.False(BaseEntry != fetched);
    }

    [Fact]
    public void EntryNotEqualsFetched_WhenAnyBaseFieldDiffers()
    {
        var fetchedDifferentCategory = new FetchedEntry(
            EntryId: "entry-id",
            Title: BaseEntry.Title,
            Category: ["c#", "dotnet"],
            Content: BaseEntry.Content,
            AbsoluteCustomPath: BaseEntry.CustomPath!,
            AbsoluteDate: BaseEntry.Date!.Value,
            AbsoluteDraft: BaseEntry.Draft!.Value,
            AbsolutePreview: BaseEntry.Preview!.Value,
            Published: DateTime.Parse("2023-10-01"),
            ContentType: "text/x-markdown"
        );

        Assert.False(BaseEntry == fetchedDifferentCategory);
        Assert.True(BaseEntry != fetchedDifferentCategory);
    }

    [Fact]
    public void EntryEqualsEditable_WhenBaseFieldsMatch()
    {
        var editable = new EditableEntry(
            EntryId: "entry-id",
            Title: BaseEntry.Title,
            Category: [.. BaseEntry.Category],
            Content: BaseEntry.Content,
            CustomPath: BaseEntry.CustomPath,
            Date: BaseEntry.Date,
            Draft: BaseEntry.Draft,
            Preview: BaseEntry.Preview
        );

        Assert.True(BaseEntry == editable);
        Assert.False(BaseEntry != editable);
    }

    [Fact]
    public void EntryNotEqualsEditable_WhenBaseFieldDiffers()
    {
        var editableDifferentTitle = new EditableEntry(
            EntryId: "entry-id",
            Title: BaseEntry.Title + "!",
            Category: [.. BaseEntry.Category],
            Content: BaseEntry.Content,
            CustomPath: BaseEntry.CustomPath,
            Date: BaseEntry.Date,
            Draft: BaseEntry.Draft,
            Preview: BaseEntry.Preview
        );

        Assert.False(BaseEntry == editableDifferentTitle);
        Assert.True(BaseEntry != editableDifferentTitle);
    }

    [Fact]
    public void EntryEqualsFetched_WhenCategoryOrderDiffers()
    {
        var fetched = new FetchedEntry(
            EntryId: "entry-id",
            Title: BaseEntry.Title,
            Category: ["c#", "tech"],
            Content: BaseEntry.Content,
            AbsoluteCustomPath: BaseEntry.CustomPath!,
            AbsoluteDate: BaseEntry.Date!.Value,
            AbsoluteDraft: BaseEntry.Draft!.Value,
            AbsolutePreview: BaseEntry.Preview!.Value,
            Published: DateTime.Parse("2023-10-01"),
            ContentType: "text/x-markdown"
        );

        Assert.True(BaseEntry == fetched);
        Assert.False(BaseEntry != fetched);
    }

    [Fact]
    public void EntryEqualsEditable_WhenCategoryOrderDiffers()
    {
        var editable = new EditableEntry(
            EntryId: "entry-id",
            Title: BaseEntry.Title,
            Category: ["c#", "tech"],
            Content: BaseEntry.Content,
            CustomPath: BaseEntry.CustomPath,
            Date: BaseEntry.Date,
            Draft: BaseEntry.Draft,
            Preview: BaseEntry.Preview
        );

        Assert.True(BaseEntry == editable);
        Assert.False(BaseEntry != editable);
    }
}
using EditableEntry = UpHateblo.Lib.Entry.Edit.EditableEntry;
using FetchedEntry = UpHateblo.Lib.Entry.List.FetchedEntry;

namespace UpHateblo.Lib.Tests.Entry.Edit;

public class EditableEntryOperatorTests
{
    private static readonly EditableEntry BaseEditable = new(
        EntryId: "entry-id",
        Title: "Test Title",
        Category: ["tech", "c#"],
        Content: "Test content",
        CustomPath: "/test/custom/path",
        Date: DateTime.Parse("2023-10-02"),
        Draft: false,
        Preview: false
    );

    [Fact]
    public void EditableEqualsFetched_WhenInheritedFieldsMatch()
    {
        var fetched = new FetchedEntry(
            EntryId: BaseEditable.EntryId,
            Title: BaseEditable.Title,
            Category: [.. BaseEditable.Category],
            Content: BaseEditable.Content,
            AbsoluteCustomPath: BaseEditable.CustomPath!,
            AbsoluteDate: BaseEditable.Date!.Value,
            AbsoluteDraft: BaseEditable.Draft!.Value,
            AbsolutePreview: BaseEditable.Preview!.Value,
            Published: DateTime.Parse("2023-10-01"),
            ContentType: "text/x-markdown"
        );

        Assert.True(BaseEditable == fetched);
        Assert.False(BaseEditable != fetched);
    }

    [Fact]
    public void EditableNotEqualsFetched_WhenAnyInheritedFieldDiffers()
    {
        var fetchedDifferentCategory = new FetchedEntry(
            EntryId: BaseEditable.EntryId,
            Title: BaseEditable.Title,
            Category: ["c#", "dotnet"],
            Content: BaseEditable.Content,
            AbsoluteCustomPath: BaseEditable.CustomPath!,
            AbsoluteDate: BaseEditable.Date!.Value,
            AbsoluteDraft: BaseEditable.Draft!.Value,
            AbsolutePreview: BaseEditable.Preview!.Value,
            Published: DateTime.Parse("2023-10-01"),
            ContentType: "text/x-markdown"
        );

        Assert.False(BaseEditable == fetchedDifferentCategory);
        Assert.True(BaseEditable != fetchedDifferentCategory);
    }

    [Fact]
    public void EditableEqualsEntry_WhenBaseFieldsMatch()
    {
        var entry = new Lib.Entry.Post.Entry(
            Title: BaseEditable.Title,
            Category: [.. BaseEditable.Category],
            Content: BaseEditable.Content,
            CustomPath: BaseEditable.CustomPath,
            Date: BaseEditable.Date,
            Draft: BaseEditable.Draft,
            Preview: BaseEditable.Preview
        );

        Assert.True(BaseEditable == entry);
        Assert.False(BaseEditable != entry);
    }

    [Fact]
    public void EditableNotEqualsEntry_WhenBaseFieldDiffers()
    {
        var entryDifferentTitle = new Lib.Entry.Post.Entry(
            Title: BaseEditable.Title + "!",
            Category: [.. BaseEditable.Category],
            Content: BaseEditable.Content,
            CustomPath: BaseEditable.CustomPath,
            Date: BaseEditable.Date,
            Draft: BaseEditable.Draft,
            Preview: BaseEditable.Preview
        );

        Assert.False(BaseEditable == entryDifferentTitle);
        Assert.True(BaseEditable != entryDifferentTitle);
    }

    [Fact]
    public void EditableEqualsFetched_WhenCategoryOrderDiffers()
    {
        var fetched = new FetchedEntry(
            EntryId: BaseEditable.EntryId,
            Title: BaseEditable.Title,
            Category: ["c#", "tech"],
            Content: BaseEditable.Content,
            AbsoluteCustomPath: BaseEditable.CustomPath!,
            AbsoluteDate: BaseEditable.Date!.Value,
            AbsoluteDraft: BaseEditable.Draft!.Value,
            AbsolutePreview: BaseEditable.Preview!.Value,
            Published: DateTime.Parse("2023-10-01"),
            ContentType: "text/x-markdown"
        );

        Assert.True(BaseEditable == fetched);
        Assert.False(BaseEditable != fetched);
    }

    [Fact]
    public void EditableEqualsEntry_WhenCategoryOrderDiffers()
    {
        var entry = new Lib.Entry.Post.Entry(
            Title: BaseEditable.Title,
            Category: ["c#", "tech"],
            Content: BaseEditable.Content,
            CustomPath: BaseEditable.CustomPath,
            Date: BaseEditable.Date,
            Draft: BaseEditable.Draft,
            Preview: BaseEditable.Preview
        );

        Assert.True(BaseEditable == entry);
        Assert.False(BaseEditable != entry);
    }
}
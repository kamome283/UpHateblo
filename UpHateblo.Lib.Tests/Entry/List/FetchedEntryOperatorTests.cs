using EditableEntry = UpHateblo.Lib.Entry.Edit.EditableEntry;
using FetchedEntry = UpHateblo.Lib.Entry.List.FetchedEntry;

namespace UpHateblo.Lib.Tests.Entry.List;

public class FetchedEntryOperatorTests
{
    private static readonly FetchedEntry BaseFetched = new(
        EntryId: "entry-id",
        Title: "Test Title",
        Category: ["tech", "c#"],
        Content: "Test content",
        AbsoluteCustomPath: "/test/custom/path",
        AbsoluteDate: DateTime.Parse("2023-10-02"),
        AbsoluteDraft: false,
        AbsolutePreview: false,
        Published: DateTime.Parse("2023-10-01"),
        ContentType: "text/x-markdown"
    );

    [Fact]
    public void FetchedEqualsEditable_WhenInheritedFieldsMatch()
    {
        var editable = new EditableEntry(
            EntryId: "entry-id",
            Title: "Test Title",
            Category: ["tech", "c#"],
            Content: "Test content",
            CustomPath: "/test/custom/path",
            Date: DateTime.Parse("2023-10-02"),
            Draft: false,
            Preview: false
        );

        Assert.True(BaseFetched == editable);
        Assert.False(BaseFetched != editable);
    }

    [Fact]
    public void FetchedNotEqualsEditable_WhenAnyInheritedFieldDiffers()
    {
        var editableDifferentCategory = new EditableEntry(
            EntryId: BaseFetched.EntryId,
            Title: BaseFetched.Title,
            Category: ["c#", "dotnet"],
            Content: BaseFetched.Content,
            CustomPath: BaseFetched.AbsoluteCustomPath,
            Date: BaseFetched.AbsoluteDate,
            Draft: BaseFetched.AbsoluteDraft,
            Preview: BaseFetched.AbsolutePreview
        );

        Assert.False(BaseFetched == editableDifferentCategory);
        Assert.True(BaseFetched != editableDifferentCategory);
    }

    [Fact]
    public void FetchedEqualsEntry_WhenBaseFieldsMatch()
    {
        var entry = new Lib.Entry.Post.Entry(
            Title: BaseFetched.Title,
            Category: [.. BaseFetched.Category],
            Content: BaseFetched.Content,
            CustomPath: BaseFetched.AbsoluteCustomPath,
            Date: BaseFetched.AbsoluteDate,
            Draft: BaseFetched.AbsoluteDraft,
            Preview: BaseFetched.AbsolutePreview
        );

        Assert.True(BaseFetched == entry);
        Assert.False(BaseFetched != entry);
    }

    [Fact]
    public void FetchedNotEqualsEntry_WhenBaseFieldDiffers()
    {
        var entryDifferentTitle = new Lib.Entry.Post.Entry(
            Title: BaseFetched.Title + "!",
            Category: [.. BaseFetched.Category],
            Content: BaseFetched.Content,
            CustomPath: BaseFetched.AbsoluteCustomPath,
            Date: BaseFetched.AbsoluteDate,
            Draft: BaseFetched.AbsoluteDraft,
            Preview: BaseFetched.AbsolutePreview
        );

        Assert.False(BaseFetched == entryDifferentTitle);
        Assert.True(BaseFetched != entryDifferentTitle);
    }

    [Fact]
    public void FetchedEqualsEditable_WhenCategoryOrderDiffers()
    {
        var editable = new EditableEntry(
            EntryId: BaseFetched.EntryId,
            Title: BaseFetched.Title,
            Category: ["c#", "tech"],
            Content: BaseFetched.Content,
            CustomPath: BaseFetched.AbsoluteCustomPath,
            Date: BaseFetched.AbsoluteDate,
            Draft: BaseFetched.AbsoluteDraft,
            Preview: BaseFetched.AbsolutePreview
        );

        Assert.True(BaseFetched == editable);
        Assert.False(BaseFetched != editable);
    }

    [Fact]
    public void FetchedEqualsEntry_WhenCategoryOrderDiffers()
    {
        var entry = new Lib.Entry.Post.Entry(
            Title: BaseFetched.Title,
            Category: ["c#", "tech"],
            Content: BaseFetched.Content,
            CustomPath: BaseFetched.AbsoluteCustomPath,
            Date: BaseFetched.AbsoluteDate,
            Draft: BaseFetched.AbsoluteDraft,
            Preview: BaseFetched.AbsolutePreview
        );

        Assert.True(BaseFetched == entry);
        Assert.False(BaseFetched != entry);
    }
}
using UpHateblo.Lib.Entry.Edit;
using UpHateblo.Lib.Entry.List;
using UpHateblo.Lib.Entry.Post;

namespace UpHateblo.Lib.Tests.Entry.List;

public class FetchedEntryOperatorTests
{
    private static readonly FetchedEntry BaseFetched = new(
        EntryId: "entry-id",
        Title: "Test Title",
        Category: ["tech", "c#"],
        Content: "Test content",
        CustomPath: "/test/custom/path",
        Date: DateTime.Parse("2023-10-02"),
        Draft: false,
        Preview: false,
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
            CustomPath: BaseFetched.CustomPath,
            Date: BaseFetched.Date,
            Draft: BaseFetched.Draft,
            Preview: BaseFetched.Preview
        );

        Assert.False(BaseFetched == editableDifferentCategory);
        Assert.True(BaseFetched != editableDifferentCategory);
    }

    [Fact]
    public void FetchedEqualsEntry_WhenBaseFieldsMatch()
    {
        var entry = new PostableEntry(
            Title: BaseFetched.Title,
            Category: [.. BaseFetched.Category],
            Content: BaseFetched.Content,
            CustomPath: BaseFetched.CustomPath,
            Date: BaseFetched.Date,
            Draft: BaseFetched.Draft,
            Preview: BaseFetched.Preview
        );

        Assert.True(BaseFetched == entry);
        Assert.False(BaseFetched != entry);
    }

    [Fact]
    public void FetchedNotEqualsEntry_WhenBaseFieldDiffers()
    {
        var entryDifferentTitle = new PostableEntry(
            Title: BaseFetched.Title + "!",
            Category: [.. BaseFetched.Category],
            Content: BaseFetched.Content,
            CustomPath: BaseFetched.CustomPath,
            Date: BaseFetched.Date,
            Draft: BaseFetched.Draft,
            Preview: BaseFetched.Preview
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
            CustomPath: BaseFetched.CustomPath,
            Date: BaseFetched.Date,
            Draft: BaseFetched.Draft,
            Preview: BaseFetched.Preview
        );

        Assert.True(BaseFetched == editable);
        Assert.False(BaseFetched != editable);
    }

    [Fact]
    public void FetchedEqualsEntry_WhenCategoryOrderDiffers()
    {
        var entry = new PostableEntry(
            Title: BaseFetched.Title,
            Category: ["c#", "tech"],
            Content: BaseFetched.Content,
            CustomPath: BaseFetched.CustomPath,
            Date: BaseFetched.Date,
            Draft: BaseFetched.Draft,
            Preview: BaseFetched.Preview
        );

        Assert.True(BaseFetched == entry);
        Assert.False(BaseFetched != entry);
    }
}
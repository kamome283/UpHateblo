using FetchedEntry = UpHateblo.Lib.Entry.List.FetchedEntry;

namespace UpHateblo.Lib.Tests.Entry.List;

public class FetchedEntryTests
{
    private static readonly FetchedEntry BaseEntry = new(
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
    public void EqualsWhenCategoryContainsSameContents()
    {
        var modified = BaseEntry with { Category = ["c#", "tech"] };
        Assert.Equal(BaseEntry, modified);
        Assert.Equal(BaseEntry.GetHashCode(), modified.GetHashCode());
    }

    [Fact]
    public void NotEqualsWhenCategoryContainsDifferentContents()
    {
        var modified = BaseEntry with { Category = ["c#", "dotnet"] };
        Assert.NotEqual(BaseEntry, modified);
        Assert.NotEqual(BaseEntry.GetHashCode(), modified.GetHashCode());
    }
}
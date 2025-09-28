using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Entities;

public class FetchedEntryTests
{
    private static readonly FetchedEntry BaseEntry = new(
        EntryId: "entry-id",
        Title: "Test Title",
        Category: [],
        Content: "Test content",
        AbsoluteCustomPath: "/test/custom/path",
        AbsoluteUpdated: DateTime.Parse("2023-10-02"),
        AbsoluteDraft: false,
        AbsolutePreview: false,
        Published: DateTime.Parse("2023-10-01"),
        ContentType: "text/x-markdown"
    );

    [Fact]
    public void EqualsWhenCategoryContainsSameContents()
    {
        var entry1 = BaseEntry with { Category = ["tech", "c#"] };
        var entry2 = BaseEntry with { Category = ["c#", "tech"] };
        Assert.Equal(entry1, entry2);
        Assert.Equal(entry1.GetHashCode(), entry2.GetHashCode());
    }

    [Fact]
    public void NotEqualsWhenCategoryContainsDifferentContents()
    {
        var entry1 = BaseEntry with { Category = ["tech", "c#"] };
        var entry2 = BaseEntry with { Category = ["c#", "dotnet"] };
        Assert.NotEqual(entry1, entry2);
        Assert.NotEqual(entry1.GetHashCode(), entry2.GetHashCode());
    }
}
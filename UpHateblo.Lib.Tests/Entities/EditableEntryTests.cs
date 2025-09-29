using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Entities;

public class EditableEntryTests
{
    private static readonly EditableEntry BaseEntry = new(
        EntryId: "entry-id",
        Title: "Title",
        Category: ["tech", "c#"],
        Content: "Content",
        CustomPath: "/url",
        Updated: DateTime.Parse("2023-10-01"),
        Draft: false,
        Preview: false
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
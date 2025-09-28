using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Entities;

public class PushableEntryTests
{
    private static readonly PushableEntry BaseEntry = new(
        EntryId: "entry-id",
        Title: "Title",
        Category: [],
        Content: "Content",
        CustomPath: "/url",
        Updated: DateTime.Parse("2023-10-01"),
        Draft: false,
        Preview: false
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
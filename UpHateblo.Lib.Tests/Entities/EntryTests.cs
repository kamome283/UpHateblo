using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Entities;

public class EntryTests
{
    // Use a fixed date for deterministic tests
    private static readonly DateTime FixedDate =
        DateTime.Parse("2023-10-27T10:00:00Z").ToUniversalTime();

    [Fact]
    public void EqualsWhenHashSetInstancesAreDifferentButContentsAreSame()
    {
        // Arrange
        var entry1 = new Entry(
            Title: "Title",
            Category: ["tech", "c#"],
            Content: "Content",
            CustomPath: "/url",
            Updated: FixedDate,
            Draft: false,
            Preview: false
        );
        var entry2 = new Entry(
            Title: "Title",
            Category: ["c#", "tech"], // Order does not matter for HashSet
            Content: "Content",
            CustomPath: "/url",
            Updated: FixedDate,
            Draft: false,
            Preview: false
        );

        // Act & Assert
        Assert.Equal(entry1, entry2);
        Assert.Equal(entry1.GetHashCode(), entry2.GetHashCode());
    }

    [Fact]
    public void NotEqualsWhenHashSetContentsDiffer()
    {
        // Arrange
        var entry1 = new Entry(
            Title: "Title",
            Category: ["tech", "c#"],
            Content: "Content",
            CustomPath: "/url",
            Updated: FixedDate,
            Draft: false,
            Preview: false
        );
        var entry2 = new Entry(
            Title: "Title",
            Category: ["tech", "dotnet"],
            Content: "Content",
            CustomPath: "/url",
            Updated: FixedDate,
            Draft: false,
            Preview: false
        );

        // Act & Assert
        Assert.NotEqual(entry1, entry2);
    }
}
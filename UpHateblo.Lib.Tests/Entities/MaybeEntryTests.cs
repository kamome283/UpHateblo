using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Entities;

public class MaybeEntryTests
{
    // Use a fixed date for deterministic tests
    private static readonly DateTime FixedDate =
        DateTime.Parse("2023-10-27T10:00:00Z").ToUniversalTime();

    [Fact]
    public void
        EqualsWhenPropertiesAreSameAndHashSetInstancesAreDifferentButEquivalent()
    {
        // Arrange
        var entry1 = new MaybeEntry("Title", ["tech", "c#"], FixedDate,
            "Content", "/url", false, false);
        var entry2 = new MaybeEntry("Title", ["c#", "tech"], FixedDate,
            "Content", "/url", false, false); // Order doesn't matter for HashSet

        // Act & Assert
        // This should be true because the [HashSetEquality] attribute on the Category property
        // should compare the contents of the HashSets, not their references.
        Assert.Equal(entry1, entry2);
        Assert.Equal(entry1.GetHashCode(), entry2.GetHashCode());
    }

    [Fact]
    public void NotEqualsWhenHashSetContentsAreDifferent()
    {
        // Arrange
        var entry1 = new MaybeEntry("Title", ["tech", "c#"], FixedDate,
            "Content", "/url", false, false);
        var entry2 = new MaybeEntry("Title", ["tech", "dotnet"], FixedDate,
            "Content", "/url", false, false);

        // Act & Assert
        Assert.NotEqual(entry1, entry2);
    }
}
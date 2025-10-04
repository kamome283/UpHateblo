using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Lib.Tests.Entry.Shared;

public class MaybeEntryMergeTests
{
    [Fact]
    public void MergePrefersLeftNonNullValues()
    {
        var left = new MaybeEntry(
            EntryId: null,
            Title: "LeftTitle",
            Category: null,
            Content: "LeftContent",
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var right = new MaybeEntry(
            EntryId: null,
            Title: "RightTitle",
            Category: null,
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var merged = left.Merge(right);

        Assert.Equal("LeftTitle", merged.Title);
        Assert.Equal("LeftContent", merged.Content);
    }

    [Fact]
    public void MergeTakesFirstNonNullFromSubsequent()
    {
        var left = new MaybeEntry(
            EntryId: null,
            Title: null,
            Category: null,
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var e1 = new MaybeEntry(
            EntryId: null,
            Title: null,
            Category: null,
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var e2 = new MaybeEntry(
            EntryId: null,
            Title: "TitleFromE2",
            Category: null,
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var e3 = new MaybeEntry(
            EntryId: null,
            Title: "TitleFromE3",
            Category: null,
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var merged = left.Merge(e1, e2, e3);

        Assert.Equal("TitleFromE2", merged.Title);
    }

    [Fact]
    public void MergeDoesNotOverwriteWithNullLater()
    {
        var left = new MaybeEntry(
            EntryId: null,
            Title: null,
            Category: null,
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var e1 = new MaybeEntry(
            EntryId: null,
            Title: "First",
            Category: null,
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var e2 = new MaybeEntry(
            EntryId: null,
            Title: null,
            Category: null,
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var merged = left.Merge(e1, e2);

        Assert.Equal("First", merged.Title);
    }

    [Fact]
    public void MergeHandlesAllFieldsIndependently()
    {
        var left = new MaybeEntry(
            EntryId: null,
            Title: null,
            Category: null,
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: null,
            Preview: null,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );

        var e1 = new MaybeEntry(
            EntryId: "id-1",
            Title: null,
            Category: ["tech", "c#"],
            Content: null,
            CustomPath: null,
            Date: null,
            Draft: true,
            Preview: null,
            Published: null,
            ContentType: "text/x-markdown",
            PreviewUrl: "https://example.com/preview"
        );

        var e2 = new MaybeEntry(
            EntryId: null,
            Title: "my title",
            Category: null,
            Content: "body",
            CustomPath: "/custom",
            Date: DateTime.Parse("2024-01-02"),
            Draft: null,
            Preview: true,
            Published: DateTime.Parse("2024-01-01"),
            ContentType: null,
            PreviewUrl: null
        );

        var merged = left.Merge(e1, e2);

        Assert.Equal("id-1", merged.EntryId);
        Assert.Equal("my title", merged.Title);
        Assert.Equal(["tech", "c#"], merged.Category);
        Assert.Equal("body", merged.Content);
        Assert.Equal("/custom", merged.CustomPath);
        Assert.Equal(DateTime.Parse("2024-01-02"), merged.Date);
        Assert.True(merged.Draft);
        Assert.True(merged.Preview);
        Assert.Equal(DateTime.Parse("2024-01-01"), merged.Published);
        Assert.Equal("text/x-markdown", merged.ContentType);
        Assert.Equal("https://example.com/preview", merged.PreviewUrl);
    }
}
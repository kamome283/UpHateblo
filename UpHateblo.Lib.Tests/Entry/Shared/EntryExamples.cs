using UpHateblo.Lib.Entry.Post;

namespace UpHateblo.Lib.Tests.Entry.Shared;

public static class EntryExamples
{
    public static PostableEntry PostableEntry()
    {
        var now = DateTime.Now;
        var updated = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second,
            0);
        return new PostableEntry(
            Title: "PostCommandTest",
            Category: ["UpHateblo", "Post", "Test"],
            Content: $"""
                      This is a test content of post command.
                      Posted at {updated}
                      """,
            CustomPath: Guid.CreateVersion7().ToString(),
            Date: updated,
            Draft: true,
            Preview: false
        );
    }
}
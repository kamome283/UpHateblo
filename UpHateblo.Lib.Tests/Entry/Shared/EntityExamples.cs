using UpHateblo.Lib.Entry.Post;

namespace UpHateblo.Lib.Tests.Entry.Shared;

public static class EntityExamples
{
    public static PostableEntry PostableEntryExample()
    {
        var updated = DateTime.Now;
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
using System.ComponentModel.DataAnnotations;
using Equatable.Attributes;
using UpHateblo.Lib.Entry.Edit;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Lib.Entry.Post;

[Equatable]
public partial record PostableEntry(
    [property: Required] string Title,
    [property: Required, HashSetEquality] HashSet<string> Category,
    [property: Required] string Content,
    string? CustomPath,
    DateTime? Date,
    bool? Draft,
    bool? Preview
)
{
    public static bool operator ==(PostableEntry postable, FetchedEntry fetched)
    {
        return fetched == postable;
    }

    public static bool operator !=(PostableEntry postable, FetchedEntry fetched) =>
        !(postable == fetched);

    public static bool operator ==(PostableEntry postable, EditableEntry editable)
    {
        return editable == postable;
    }

    public static bool operator !=(PostableEntry postable, EditableEntry editable) =>
        !(postable == editable);
}

public static class PostableEntryExtensions
{
    public static PostableEntry Materialize(this MaybeEntry maybeEntry)
    {
        var postable = new PostableEntry(
            Title: maybeEntry.Title!,
            Category: maybeEntry.Category!,
            Content: maybeEntry.Content!,
            CustomPath: maybeEntry.CustomPath,
            Date: maybeEntry.Date,
            Draft: maybeEntry.Draft,
            Preview: maybeEntry.Preview
        );

        var missingProps = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(postable, new ValidationContext(postable),
            missingProps, true);
        if (isValid) return postable;

        var concatenated = string.Join(", ", missingProps.Select(x => x.MemberNames.First()));
        throw new ValidationException($"Missing properties: {concatenated}");
    }
}
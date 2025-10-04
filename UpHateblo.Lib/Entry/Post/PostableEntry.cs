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
    public static explicit operator PostableEntry(MaybeEntry maybe)
    {
        var postable = new PostableEntry(
            Title: maybe.Title!,
            Category: maybe.Category!,
            Content: maybe.Content!,
            CustomPath: maybe.CustomPath,
            Date: maybe.Date,
            Draft: maybe.Draft,
            Preview: maybe.Preview
        );

        var missingProps = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(postable, new ValidationContext(postable),
            missingProps, true);
        if (isValid) return postable;

        var concatenated = string.Join(", ", missingProps.Select(x => x.MemberNames.First()));
        throw new ValidationException($"Missing properties: {concatenated}");
    }

    public static implicit operator MaybeEntry(PostableEntry postable)
    {
        return new MaybeEntry(
            EntryId: null,
            Title: postable.Title,
            Category: postable.Category,
            Content: postable.Content,
            CustomPath: postable.CustomPath,
            Date: postable.Date,
            Draft: postable.Draft,
            Preview: postable.Preview,
            Published: null,
            ContentType: null,
            PreviewUrl: null
        );
    }

    public static bool operator ==(PostableEntry postable, FetchedEntry fetched)
        => fetched == postable;

    public static bool operator !=(PostableEntry postable, FetchedEntry fetched)
        => fetched != postable;

    public static bool operator ==(PostableEntry postable, EditableEntry editable)
        => editable == postable;

    public static bool operator !=(PostableEntry postable, EditableEntry editable)
        => editable != postable;
}
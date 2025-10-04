using System.ComponentModel.DataAnnotations;
using Equatable.Attributes;
using UpHateblo.Lib.Entry.Post;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Lib.Entry.Edit;

[Equatable]
public partial record EditableEntry(
    [property: Required] string EntryId,
    [property: Required] string Title,
    [property: Required, HashSetEquality] HashSet<string> Category,
    [property: Required] string Content,
    string? CustomPath,
    DateTime? Date,
    bool? Draft,
    bool? Preview
)
{
    public static explicit operator EditableEntry(MaybeEntry maybe)
    {
        var editable = new EditableEntry(
            EntryId: maybe.EntryId!,
            Title: maybe.Title!,
            Category: maybe.Category!,
            Content: maybe.Content!,
            CustomPath: maybe.CustomPath,
            Date: maybe.Date,
            Draft: maybe.Draft,
            Preview: maybe.Preview
        );

        var missingProps = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(editable, new ValidationContext(editable),
            missingProps, true);
        if (isValid) return editable;

        var concatenated = string.Join(", ", missingProps.Select(x => x.MemberNames.First()));
        throw new ValidationException($"Missing properties: {concatenated}");
    }

    public static implicit operator PostableEntry(EditableEntry editable)
    {
        return new PostableEntry(
            Title: editable.Title,
            Category: editable.Category,
            Content: editable.Content,
            CustomPath: editable.CustomPath,
            Date: editable.Date,
            Draft: editable.Draft,
            Preview: editable.Preview
        );
    }

    public static bool operator ==(EditableEntry editable, FetchedEntry fetched)
    {
        return fetched == editable;
    }

    public static bool operator !=(EditableEntry editable, FetchedEntry fetched) =>
        !(editable == fetched);

    public static bool operator ==(EditableEntry editable, PostableEntry postable)
    {
        var x = new PostableEntry(
            Title: editable.Title,
            Category: editable.Category,
            Content: editable.Content,
            CustomPath: editable.CustomPath,
            Date: editable.Date,
            Draft: editable.Draft,
            Preview: editable.Preview
        );
        return x == postable;
    }

    public static bool operator !=(EditableEntry editable, PostableEntry postable) =>
        !(editable == postable);
}
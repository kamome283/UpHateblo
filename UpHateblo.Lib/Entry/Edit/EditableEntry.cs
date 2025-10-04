using System.ComponentModel.DataAnnotations;
using Equatable.Attributes;
using UpHateblo.Lib.Entry.Post;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Lib.Entry.Edit;

/// <remarks>PostingEntrySchemaを使うためにPostableEntryのサブクラスにしている</remarks>
[Equatable]
public partial record EditableEntry(
    // Newly defined fields
    [property: Required] string EntryId,
    // Inherited fields
    string Title,
    HashSet<string> Category,
    string Content,
    string? CustomPath,
    DateTime? Date,
    bool? Draft,
    bool? Preview
) : PostableEntry(Title, Category, Content, CustomPath, Date, Draft, Preview)
{
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

public static class EditableEntryExtensions
{
    public static EditableEntry Materialize(this MaybeEntry maybeEntry)
    {
        var editable = new EditableEntry(
            EntryId: maybeEntry.EntryId!,
            Title: maybeEntry.Title!,
            Category: maybeEntry.Category!,
            Content: maybeEntry.Content!,
            CustomPath: maybeEntry.CustomPath,
            Date: maybeEntry.Date,
            Draft: maybeEntry.Draft,
            Preview: maybeEntry.Preview
        );

        var missingProps = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(editable, new ValidationContext(editable),
            missingProps, true);
        if (isValid) return editable;

        var concatenated = string.Join(", ", missingProps.Select(x => x.MemberNames.First()));
        throw new ValidationException($"Missing properties: {concatenated}");
    }
}
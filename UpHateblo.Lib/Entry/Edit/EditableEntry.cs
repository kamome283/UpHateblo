using System.ComponentModel.DataAnnotations;
using Equatable.Attributes;
using UpHateblo.Lib.Entry.Post;
using UpHateblo.Lib.Entry.Read;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Lib.Entry.Edit;

/// <remarks>PostingEntrySchemaを使うためにPostableEntryのサブクラスにしている</remarks>
[Equatable]
public partial record EditableEntry(
    // Newly defined fields
    string EntryId,
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
        var lackingProperties = LackingProperties(maybeEntry);
        if (lackingProperties.Count != 0)
            throw new ValidationException(
                "Missing properties: " + string.Join(", ", lackingProperties)
            );

        return new EditableEntry(
            maybeEntry.EntryId!,
            maybeEntry.Title!,
            maybeEntry.Category!,
            maybeEntry.Content!,
            maybeEntry.CustomPath,
            maybeEntry.Date,
            maybeEntry.Draft,
            maybeEntry.Preview
        );
    }

    private static List<string> LackingProperties(MaybeEntry maybeEntry)
    {
        var entryLackingProperties = PostableEntryExtensions.LackingProperties(maybeEntry);
        if (maybeEntry.EntryId == null) entryLackingProperties.Add("EntryId");
        return entryLackingProperties;
    }
}
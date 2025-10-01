using System.ComponentModel.DataAnnotations;
using Equatable.Attributes;

namespace UpHateblo.Lib.Entities;

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
) : Entry(Title, Category, Content, CustomPath, Date, Draft, Preview);

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
        var entryLackingProperties = EntryExtensions.LackingProperties(maybeEntry);
        if (maybeEntry.EntryId == null) entryLackingProperties.Add("EntryId");
        return entryLackingProperties;
    }
}
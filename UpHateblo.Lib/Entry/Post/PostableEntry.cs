using System.ComponentModel.DataAnnotations;
using Equatable.Attributes;
using UpHateblo.Lib.Entry.Edit;
using UpHateblo.Lib.Entry.Read;
using UpHateblo.Lib.Entry.Shared;

namespace UpHateblo.Lib.Entry.Post;

[Equatable]
public partial record PostableEntry(
    string Title,
    [property: HashSetEquality] HashSet<string> Category,
    string Content,
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
        var lackingProperties = LackingProperties(maybeEntry);
        if (lackingProperties.Count != 0)
            throw new ValidationException(
                "Missing properties: " + string.Join(", ", lackingProperties)
            );

        return new PostableEntry(
            maybeEntry.Title!,
            maybeEntry.Category!,
            maybeEntry.Content!,
            maybeEntry.CustomPath,
            maybeEntry.Date,
            maybeEntry.Draft,
            maybeEntry.Preview
        );
    }

    internal static List<string> LackingProperties(MaybeEntry maybeEntry)
    {
        var properties = new List<string>();
        if (maybeEntry.Title == null) properties.Add("Title");
        if (maybeEntry.Category == null) properties.Add("Category");
        if (maybeEntry.Content == null) properties.Add("Content");
        return properties;
    }
}
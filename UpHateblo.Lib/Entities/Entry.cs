using System.ComponentModel.DataAnnotations;
using Equatable.Attributes;

namespace UpHateblo.Lib.Entities;

[Equatable]
public partial record Entry(
    string Title,
    [property: HashSetEquality] HashSet<string> Category,
    string Content,
    string? CustomPath,
    DateTime? Date,
    bool? Draft,
    bool? Preview
);

public static class EntryExtensions
{
    public static Entry Materialize(this MaybeEntry maybeEntry)
    {
        var lackingProperties = LackingProperties(maybeEntry);
        if (lackingProperties.Count != 0)
            throw new ValidationException(
                "Missing properties: " + string.Join(", ", lackingProperties)
            );

        return new Entry(
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
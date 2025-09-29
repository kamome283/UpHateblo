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
    DateTime? Updated,
    bool? Draft,
    bool? Preview
) : Entry(Title, Category, Content, CustomPath, Updated, Draft, Preview);
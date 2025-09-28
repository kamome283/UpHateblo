using Equatable.Attributes;

namespace UpHateblo.Lib.Entities;

[Equatable]
public partial record PushableEntry(
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
) : PostableEntry(Title, Category, Content, CustomPath, Updated, Draft, Preview);
using Equatable.Attributes;

namespace UpHateblo.Lib.Entities;

[Equatable]
public partial record PostableEntry(
    string Title,
    [property: HashSetEquality] HashSet<string> Category,
    string Content,
    string? CustomPath,
    DateTime? Updated,
    bool? Draft,
    bool? Preview
);
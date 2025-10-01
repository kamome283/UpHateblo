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
using Equatable.Attributes;

namespace UpHateblo.Lib.Entities;

[Equatable]
public partial record Entry(
    string Title,
    [property: HashSetEquality] HashSet<string> Category,
    DateTime Date,
    string Content,
    string? CustomPath,
    string? EntryId,
    bool? Draft = null,
    bool? Preview = null
);
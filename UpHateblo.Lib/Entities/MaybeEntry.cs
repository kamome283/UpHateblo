using Equatable.Attributes;
using VYaml.Annotations;

namespace UpHateblo.Lib.Entities;

/// <summary>
///     ファイルやCLIのパラメーターから読み出すエントリーの断片
/// </summary>
[Equatable]
[YamlObject]
public partial record MaybeEntry(
    string? EntryId,
    string? Title,
    [property: HashSetEquality] HashSet<string>? Category,
    string? Content,
    string? CustomPath,
    DateTime? Updated,
    bool? Draft,
    bool? Preview,
    DateTime? Published,
    string? ContentType
);
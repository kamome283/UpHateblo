using Equatable.Attributes;

namespace UpHateblo.Lib.Entities;

/// <summary>
///     ファイルやCLIのパラメーターから読み出すエントリーの断片
/// </summary>
[Equatable]
public partial record MaybeEntry(
    // Inherited fields
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
)
{
    public MaybeEntry() : this(null, null, null, null, null, null, null, null, null, null)
    {
    }
}
using Equatable.Attributes;

namespace UpHateblo.Lib.Entities;

/// <summary>
///     ファイルやCLIのパラメーターから読み出すエントリーの断片
/// </summary>
[Equatable]
public partial record MaybeEntry(
    string? Title,
    [property: HashSetEquality] HashSet<string>? Category,
    DateTime? Date,
    string? Content,
    string? UrlPath,
    bool? Draft,
    bool? Preview
)
{
    public MaybeEntry() : this(null, null, null, null, null, null, null)
    {
    }
}
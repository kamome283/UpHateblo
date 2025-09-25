namespace UpHateblo.Lib.Entities;

/// <summary>
/// ファイルやCLIのパラメーターから読み出すエントリーの断片
/// </summary>
public record MaybeEntry(
    string? Title,
    HashSet<string>? Category,
    DateTime? Date,
    string? Content,
    string? UrlPath,
    bool? Draft,
    bool? Preview
);
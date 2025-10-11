using Equatable.Attributes;
using VYaml.Annotations;

namespace UpHateblo.Lib.Entry.Shared;

/// <summary>ファイルやCLIのパラメーターから読み出すエントリーの断片</summary>
[Equatable]
[YamlObject]
public partial record MaybeEntry(
    string? EntryId,
    string? Title,
    [property: HashSetEquality] HashSet<string>? Category,
    string? Content,
    string? CustomPath,
    DateTime? Date,
    bool? Draft,
    bool? Preview,
    DateTime? Published,
    string? ContentType,
    string? PreviewUrl
)
{
    public static MaybeEntry Construct(string? entryId, string? title,
        IEnumerable<string>? category, string? content, string? customPath, DateTime? date,
        bool? draft, bool? preview, DateTime? published, string? contentType, string? previewUrl
    ) => new(entryId, title, category?.ToHashSet(), content, customPath, date, draft, preview,
        published, contentType, previewUrl);

    public MaybeEntry Merge(params MaybeEntry[] merged)
    {
        var result = this;
        foreach (var e in merged)
        {
            var entryId = result.EntryId ?? e.EntryId;
            var title = result.Title ?? e.Title;
            var category = result.Category ?? e.Category;
            var content = result.Content ?? e.Content;
            var customPath = result.CustomPath ?? e.CustomPath;
            var date = result.Date ?? e.Date;
            var draft = result.Draft ?? e.Draft;
            var preview = result.Preview ?? e.Preview;
            var published = result.Published ?? e.Published;
            var contentType = result.ContentType ?? e.ContentType;
            var previewUrl = result.PreviewUrl ?? e.PreviewUrl;

            result = new MaybeEntry(EntryId: entryId, Title: title, Category: category,
                Content: content, CustomPath: customPath, Date: date, Draft: draft,
                Preview: preview, Published: published, ContentType: contentType,
                PreviewUrl: previewUrl);
        }

        return result;
    }
}
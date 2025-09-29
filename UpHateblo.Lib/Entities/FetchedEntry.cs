using Equatable.Attributes;

namespace UpHateblo.Lib.Entities;

// 基底クラスではnullableなフィールドを派生クラスでnon-nullableなフィールドとして再定義することは許可されていない
// そのため基底クラスではnullableなフィールドのnon-nullableなバージョンを`Absolute...`として定義している
[Equatable]
public partial record FetchedEntry(
    // Inherited fields
    string EntryId,
    string Title,
    HashSet<string> Category,
    string Content,
    // Fields that have been made non-nullable from optional fields in the base class
    string AbsoluteCustomPath,
    DateTime AbsoluteUpdated,
    bool AbsoluteDraft,
    bool AbsolutePreview,
    // Newly defined fields
    DateTime Published,
    string ContentType
) : PushableEntry(EntryId, Title, Category, Content, AbsoluteCustomPath, AbsoluteUpdated,
    AbsoluteDraft, AbsolutePreview);
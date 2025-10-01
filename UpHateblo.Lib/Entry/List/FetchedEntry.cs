using Equatable.Attributes;
using UpHateblo.Lib.Entry.Edit;

namespace UpHateblo.Lib.Entry.List;

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
    DateTime AbsoluteDate,
    bool AbsoluteDraft,
    bool AbsolutePreview,
    // Newly defined fields
    DateTime Published,
    string ContentType
) : EditableEntry(EntryId, Title, Category, Content, AbsoluteCustomPath, AbsoluteDate,
    AbsoluteDraft, AbsolutePreview)
{
    public static bool operator ==(FetchedEntry fetched, EditableEntry editable)
    {
        var x = new EditableEntry(
            EntryId: fetched.EntryId,
            Title: fetched.Title,
            Category: fetched.Category,
            Content: fetched.Content,
            CustomPath: fetched.AbsoluteCustomPath,
            Date: fetched.AbsoluteDate,
            Draft: fetched.AbsoluteDraft,
            Preview: fetched.AbsolutePreview
        );
        return x == editable;
    }

    public static bool operator !=(FetchedEntry fetched, EditableEntry editable) =>
        !(fetched == editable);

    public static bool operator ==(FetchedEntry fetched, Post.Entry entry)
    {
        var x = new Post.Entry(
            Title: fetched.Title,
            Category: fetched.Category,
            Content: fetched.Content,
            CustomPath: fetched.AbsoluteCustomPath,
            Date: fetched.AbsoluteDate,
            Draft: fetched.AbsoluteDraft,
            Preview: fetched.AbsolutePreview
        );
        return x == entry;
    }

    public static bool operator !=(FetchedEntry fetched, Post.Entry entry) =>
        !(fetched == entry);
}
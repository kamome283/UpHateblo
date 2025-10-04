using Equatable.Attributes;
using UpHateblo.Lib.Entry.Edit;
using UpHateblo.Lib.Entry.Post;

namespace UpHateblo.Lib.Entry.Shared;

[Equatable]
public partial record FetchedEntry(
    string EntryId,
    string Title,
    [property: HashSetEquality] HashSet<string> Category,
    string Content,
    string CustomPath,
    DateTime Date,
    bool Draft,
    bool Preview,
    DateTime Published,
    string ContentType,
    string? PreviewUrl
)
{
    public static implicit operator MaybeEntry(FetchedEntry fetched)
    {
        return new MaybeEntry(
            EntryId: fetched.EntryId,
            Title: fetched.Title,
            Category: fetched.Category,
            Content: fetched.Content,
            CustomPath: fetched.CustomPath,
            Date: fetched.Date,
            Draft: fetched.Draft,
            Preview: fetched.Preview,
            Published: fetched.Published,
            ContentType: fetched.ContentType,
            PreviewUrl: fetched.PreviewUrl
        );
    }

    public static implicit operator EditableEntry(FetchedEntry fetched)
    {
        return new EditableEntry(
            EntryId: fetched.EntryId,
            Title: fetched.Title,
            Category: fetched.Category,
            Content: fetched.Content,
            CustomPath: fetched.CustomPath,
            Date: fetched.Date,
            Draft: fetched.Draft,
            Preview: fetched.Preview
        );
    }

    public static implicit operator PostableEntry(FetchedEntry fetched)
        => (EditableEntry)fetched;

    public static bool operator ==(FetchedEntry fetched, EditableEntry editable)
        => (EditableEntry)fetched == editable;

    public static bool operator !=(FetchedEntry fetched, EditableEntry editable)
        => !(fetched == editable);

    public static bool operator ==(FetchedEntry fetched, PostableEntry postable)
        => (PostableEntry)fetched == postable;

    public static bool operator !=(FetchedEntry fetched, PostableEntry postable)
        => !(fetched == postable);
}
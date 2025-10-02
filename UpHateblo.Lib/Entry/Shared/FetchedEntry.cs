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
    string ContentType
)
{
    public static bool operator ==(FetchedEntry fetched, EditableEntry editable)
    {
        var x = new EditableEntry(
            EntryId: fetched.EntryId,
            Title: fetched.Title,
            Category: fetched.Category,
            Content: fetched.Content,
            CustomPath: fetched.CustomPath,
            Date: fetched.Date,
            Draft: fetched.Draft,
            Preview: fetched.Preview
        );
        return x == editable;
    }

    public static bool operator !=(FetchedEntry fetched, EditableEntry editable) =>
        !(fetched == editable);

    public static bool operator ==(FetchedEntry fetched, PostableEntry postable)
    {
        var x = new PostableEntry(
            Title: fetched.Title,
            Category: fetched.Category,
            Content: fetched.Content,
            CustomPath: fetched.CustomPath,
            Date: fetched.Date,
            Draft: fetched.Draft,
            Preview: fetched.Preview
        );
        return x == postable;
    }

    public static bool operator !=(FetchedEntry fetched, PostableEntry postable) =>
        !(fetched == postable);
}
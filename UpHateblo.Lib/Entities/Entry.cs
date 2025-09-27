namespace UpHateblo.Lib.Entities;

public record Entry(
    string Title,
    string[] Category,
    DateTime Date,
    string Content,
    string? CustomPath,
    string? EntryId,
    bool? Draft = null,
    bool? Preview = null
);
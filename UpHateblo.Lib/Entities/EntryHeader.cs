namespace UpHateblo.Lib.Entities;

public record EntryHeader(
    string Title,
    string[] Category,
    DateTime Date,
    string? UrlPath,
    bool? Draft = null,
    bool? Preview = null
);
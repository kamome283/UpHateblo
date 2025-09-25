namespace UpHateblo.Lib.Entities;

public record Entry(
    string Title,
    string[] Category,
    DateTime Date,
    string Content,
    string? CustomPath,
    bool? Draft = null,
    bool? Preview = null
);
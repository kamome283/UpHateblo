namespace UpHateblo.Lib.Entities;

public record struct Entry(BlogConfig Blog, EntryHeader Header, string Content);
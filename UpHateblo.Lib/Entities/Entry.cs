namespace UpHateblo.Lib.Entities;

public record Entry(BlogConfig Blog, EntryHeader Header, string Content);
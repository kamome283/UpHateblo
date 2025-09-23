namespace UpHateblo.Lib;

public record BlogConfig(
    string BlogId,
    string Username,
    string Password
);

public record EntryHeader(
    string Title,
    string[] Category,
    DateTime Date,
    string UrlPath
);

public static class Post
{
    public static async Task Run(
        HttpClient client,
        BlogConfig blog,
        EntryHeader header,
        string content)
    {
        throw new NotImplementedException();
    }
}
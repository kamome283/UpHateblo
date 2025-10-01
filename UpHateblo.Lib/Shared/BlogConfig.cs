namespace UpHateblo.Lib.Shared;

public record BlogConfig(
    string BlogId,
    string Username,
    string Password
)
{
    public Uri EntryEndPoint => new($"https://blog.hatena.ne.jp/{Username}/{BlogId}/atom/entry");
}
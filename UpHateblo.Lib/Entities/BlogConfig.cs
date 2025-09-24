namespace UpHateblo.Lib.Entities;

public record BlogConfig(
    string BlogId,
    string Username,
    string Password
)
{
    public Uri EntryEndPoint =>
        new Uri($"https://blog.hatena.ne.jp/{Username}/{BlogId}/atom/entry");
}
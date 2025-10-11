namespace UpHateblo.Lib.Shared;

internal static class CommandHelper
{
    public static Wsse GenerateWsse(BlogConfig blog, string? nonce, DateTime? dateTime) =>
        new(
            blog.Username,
            blog.Password,
            nonce ?? Guid.NewGuid().ToString(),
            dateTime ?? DateTime.Now
        );
}
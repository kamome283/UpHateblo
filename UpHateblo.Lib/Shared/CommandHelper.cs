namespace UpHateblo.Lib.Shared;

internal static class CommandHelper
{
    public static Wsse GenerateWsse(BlogConfig blog, string? nonce, DateTime? dateTime) =>
        new(
            blog.Username,
            blog.Password,
            nonce ?? Guid.CreateVersion7().ToString(),
            dateTime ?? DateTime.Now
        );
}
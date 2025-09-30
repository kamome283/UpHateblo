using UpHateblo.Lib.Entities;
using UpHateblo.Lib.Http;

namespace UpHateblo.Lib.Commands;

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
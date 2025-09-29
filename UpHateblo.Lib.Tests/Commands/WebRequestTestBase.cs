using Microsoft.Extensions.Configuration;
using UpHateblo.Lib.Entities;

namespace UpHateblo.Lib.Tests.Commands;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once NotAccessedPositionalProperty.Global
public record BlogConfigSecrets(BlogConfig Blog);

[Trait("Category", "WebRequest")]
public abstract class WebRequestTestBase
{
    protected WebRequestTestBase()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<BlogConfigSecrets>()
            .Build();
        BlogConfig = new(
            config["Blog:BlogId"]!,
            config["Blog:Username"]!,
            config["Blog:Password"]!
        );
    }

    protected BlogConfig BlogConfig { get; }
}
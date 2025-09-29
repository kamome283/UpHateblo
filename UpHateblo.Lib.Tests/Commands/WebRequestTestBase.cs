using Microsoft.Extensions.Configuration;

namespace UpHateblo.Lib.Tests.Commands;

[Trait("Category", "WebRequest")]
public abstract class WebRequestTestBase<TSecrets> where TSecrets : class
{
    protected WebRequestTestBase()
    {
        var configBuilder = new ConfigurationBuilder().AddUserSecrets<TSecrets>();
        Config = configBuilder.Build();
    }

    private IConfigurationRoot Config { get; }

    protected string Get(string key)
    {
        return Config[key] ?? throw new KeyNotFoundException(key);
    }
}
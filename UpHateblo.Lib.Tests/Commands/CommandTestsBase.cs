using Microsoft.Extensions.Configuration;

namespace UpHateblo.Lib.Tests.Commands;

[Trait("Category", "Command")]
public abstract class CommandTestsBase<TSecrets> where TSecrets : class
{
    protected CommandTestsBase()
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
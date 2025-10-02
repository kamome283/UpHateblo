using Microsoft.Extensions.Configuration;
using UpHateblo.Lib.Shared;

namespace UpHateblo.Lib.Tests.Shared;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once NotAccessedPositionalProperty.Global
public record BlogConfigSecrets(BlogConfig Blog);

[Trait("Category", "WebRequest")]
public abstract class WebRequestTestBase
{
    // TODO: このプロパティはEntryのコマンドに関するものなので、より適切な場所に移動させたい
    // この名前空間のテストは結合テストでないので、テスト対象のエントリーは決め打ちにしている
    protected const string FixedTargetEntryId = "6802888565257240236";
    protected static readonly HttpClient HttpClient = new();

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